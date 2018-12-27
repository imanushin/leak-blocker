using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.SystemTools;
using LeakBlocker.Libraries.Common.Tests;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;
using LeakBlocker.Libraries.Common.Tests.SystemTools;
using LeakBlocker.Server.Service.InternalTools;
using LeakBlocker.Server.Service.InternalTools.LocalStorages;
using LeakBlocker.Server.Service.InternalTools.SingletonImplementation;
using LeakBlocker.ServerShared.AdminViewCommunication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace LeakBlocker.Server.Service.Tests.InternalTools
{
    [TestClass]
    public sealed class AgentStatusObserverTest : BaseTest
    {
        private static readonly BaseComputerAccount first = BaseComputerAccountTest.objects.First();
        private static readonly BaseComputerAccount second = BaseComputerAccountTest.objects.Skip(1).First();

        private FakeThreadPool threadPool;
        private IScheduler scheduler;
        private IScopeManager scopeManager;
        private IAgentStatusStore agentsStatusStore;
        private IAgentManager agentManager;
        private IAuditItemHelper auditItemHelper;

        [TestInitialize]
        public void Initialize()
        {
            scheduler = Mocks.StrictMock<IScheduler>();
            scopeManager = Mocks.StrictMock<IScopeManager>();
            agentsStatusStore = Mocks.StrictMock<IAgentStatusStore>();
            agentManager = Mocks.StrictMock<IAgentManager>();
            auditItemHelper = Mocks.StrictMock<IAuditItemHelper>();
            threadPool = new FakeThreadPool();

            InternalObjects.Singletons.AuditItemHelper.SetTestImplementation(auditItemHelper);
            InternalObjects.Singletons.AgentManager.SetTestImplementation(agentManager);
            InternalObjects.Singletons.AgentStatusStore.SetTestImplementation(agentsStatusStore);
            InternalObjects.Singletons.ScopeManager.SetTestImplementation(scopeManager);
            SharedObjects.Factories.ThreadPool.EnqueueTestImplementation(threadPool);
        }

        [TestMethod]
        public void ConstructorTest()
        {
            scheduler.Stub(x => x.Dispose());

            SharedObjects.Factories.Scheduler.EnqueueTestImplementation(scheduler);

            Mocks.ReplayAll();

            using (var target = new AgentStatusObserver())
            {
            }
        }

        [TestMethod]
        public void TurnedOffTest()
        {
            const ManagedComputerStatus fromStatus = ManagedComputerStatus.TurnedOff;
            const ManagedComputerStatus toStatus = ManagedComputerStatus.Unknown;

            agentsStatusStore.Stub(x => x.UpdateLastCommunicationTimeAsCurrent(second));
            agentsStatusStore.Expect(x => x.SetComputerData(second, new ManagedComputerData(toStatus, DeviceAccessMap.Empty)));
            agentsStatusStore.Expect(x => x.UpdateFromScope());//Обязательно должны обновить store
            agentsStatusStore.Stub(x => x.GetManagedScope()).Return(ReadOnlyDictionary<BaseComputerAccount, ManagedComputerData>.Empty);//Используется только для вывода лога

            CheckStatusChanging(fromStatus);
        }

        [TestMethod]
        public void WorkingTest()
        {
            const ManagedComputerStatus fromStatus = ManagedComputerStatus.Working;
            const ManagedComputerStatus toStatus = ManagedComputerStatus.Unknown;//Так как компьютер включен

            auditItemHelper.Expect(x => x.AgentIsNotResponding(second));//Первый компьютер должен будет перейти в статус Unknown и отписаться в базу
            agentsStatusStore.Expect(x => x.SetComputerData(second, new ManagedComputerData(toStatus, DeviceAccessMap.Empty)));
            agentsStatusStore.Expect(x => x.UpdateFromScope());//Обязательно должны обновить store
            agentsStatusStore.Stub(x => x.GetManagedScope()).Return(ReadOnlyDictionary<BaseComputerAccount, ManagedComputerData>.Empty);//Используется только для вывода лога

            CheckStatusChanging(fromStatus);
        }

        /// <summary>
        /// Запускает функцию для обработки статуса <paramref name="fromStatus"/>
        /// Оба компьютера считаются включенными
        /// </summary>
        private void CheckStatusChanging(ManagedComputerStatus fromStatus)
        {
            Action func = null;

            SharedObjects.Factories.Scheduler.EnqueueConstructor(
                (action, interval, flag) =>
                {
                    func = action;
                    return scheduler;
                });

            scheduler.Stub(x => x.Dispose());
            scheduler.Stub(x => x.RunNow());
            scopeManager.Stub(x => x.CurrentScope()).Return(new[] { first, second }.ToReadOnlySet());
            agentsStatusStore.Stub(x => x.GetComputerData(first)).Return(new ManagedComputerData(fromStatus, DeviceAccessMap.Empty));
            agentsStatusStore.Stub(x => x.GetComputerData(second)).Return(new ManagedComputerData(fromStatus, DeviceAccessMap.Empty));
            agentsStatusStore.Stub(x => x.GetLastCommunicationTimeUtc(first)).Return(Time.Now); //Недавно
            agentsStatusStore.Stub(x => x.GetLastCommunicationTimeUtc(second)).Return(new Time(DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)))); //Давно
            agentManager.Stub(x => x.IsComputerTurnedOn(first)).Return(true);
            agentManager.Stub(x => x.IsComputerTurnedOn(second)).Return(true);
            agentsStatusStore.Stub(x => x.GetManagedScope()).Return(ReadOnlyDictionary<BaseComputerAccount, ManagedComputerData>.Empty);//Используется только для вывода лога

            Mocks.ReplayAll();

            using (var target = new AgentStatusObserver())
            {
                target.EnqueueObserving();

                Assert.IsNotNull(func);

                func();

                threadPool.RunAllActions();
            }
        }
    }
}
