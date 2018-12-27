using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Tests;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;
using LeakBlocker.Server.Service.InternalTools;
using LeakBlocker.Server.Service.InternalTools.LocalStorages;
using LeakBlocker.Server.Service.InternalTools.SingletonImplementation;
using LeakBlocker.Server.Service.InternalTools.SingletonImplementation.LocalStorages;
using LeakBlocker.ServerShared.AdminViewCommunication.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace LeakBlocker.Server.Service.Tests.InternalTools
{
    [TestClass]
    public sealed class AgentStatusStoreTest : BaseTest
    {
       private static readonly ReadOnlySet<BaseComputerAccount> firstItems = BaseComputerAccountTest.objects.Take(2).ToReadOnlySet();
       private static readonly ReadOnlySet<BaseComputerAccount> secondItems = BaseComputerAccountTest.objects.Skip(1).Take(3).ToReadOnlySet();


        private IScopeManager scopeManager;
        private ILicenseStorage licenseStorage;
        private IAgentsSetupListStorage agentsSetupListStorage;


        [TestInitialize]
        public void Init()
        {
            scopeManager = Mocks.StrictMock<IScopeManager>();
            licenseStorage = Mocks.StrictMock<ILicenseStorage>();
            agentsSetupListStorage = Mocks.StrictMock<IAgentsSetupListStorage>();

            InternalObjects.Singletons.ScopeManager.SetTestImplementation(scopeManager);
            InternalObjects.Singletons.LicenseStorage.SetTestImplementation(licenseStorage);
            InternalObjects.Singletons.AgentsSetupListStorage.SetTestImplementation(agentsSetupListStorage);
        }

        [TestMethod]
        public void UpdateFromScope()
        {
            scopeManager.Stub(x => x.CurrentScope()).Return(firstItems).Repeat.Once();
            scopeManager.Stub(x => x.CurrentScope()).Return(secondItems).Repeat.Once();
            licenseStorage.Stub(x => x.LicenseCount).Return(999);
            agentsSetupListStorage.Stub(x => x.Current).Return(ReadOnlySet<BaseComputerAccount>.Empty);
            agentsSetupListStorage.Stub(x => x.SaveIfDifferent(null)).IgnoreArguments();

            Mocks.ReplayAll();

            var nonEmptyData = ManagedComputerDataTest.objects.First();

            var target = new AgentStatusStore();

            target.UpdateFromScope();//тут используем firstItems

            Assert.AreEqual(firstItems, target.GetManagedScope().Keys.ToReadOnlySet());

            var nextItem = BaseComputerAccountTest.objects.Skip(2).First();

            target.SetComputerData(nextItem, nonEmptyData);

            Assert.AreEqual(nonEmptyData, target.GetComputerData(nextItem));
            Assert.IsTrue(target.GetManagedScope().Keys.Contains(nextItem));

            target.UpdateFromScope();//тут используем secondItems

            Assert.IsFalse(target.GetManagedScope().Keys.Contains(BaseComputerAccountTest.objects.First()));//Уже удален из Scope
            Assert.AreEqual(nonEmptyData, target.GetComputerData(nextItem));//Старые не тронули

            Assert.AreEqual(secondItems, target.GetManagedScope().Keys.ToReadOnlySet());

            //License

            foreach (BaseComputerAccount computer in secondItems)
            {
                Assert.IsTrue(target.IsUnderLicense(computer));
            }

            BaseComputerAccount outOfLicenseComputer = new LocalComputerAccount("12313", new AccountSecurityIdentifier(WindowsIdentity.GetCurrent().User));

            Assert.IsFalse(target.IsUnderLicense(outOfLicenseComputer));


            //Communication time

            BaseComputerAccount someComputer = secondItems.First();

            Time beforeUpdate = Time.Now;

            target.UpdateLastCommunicationTimeAsCurrent(someComputer);

            Time afterUpdate = Time.Now;

            Time updateTime = target.GetLastCommunicationTimeUtc(someComputer);

            Assert.IsTrue(beforeUpdate <= updateTime);
            Assert.IsTrue(afterUpdate >= updateTime);
        }
    }
}
