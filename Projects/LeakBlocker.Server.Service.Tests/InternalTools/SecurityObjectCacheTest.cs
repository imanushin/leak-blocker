using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Implementations;
using LeakBlocker.Libraries.Common.SystemTools;
using LeakBlocker.Libraries.Common.Tests;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;
using LeakBlocker.Libraries.Common.Tests.Entities.Settings;
using LeakBlocker.Libraries.Storage;
using LeakBlocker.Libraries.SystemTools;
using LeakBlocker.Libraries.SystemTools.Entities;
using LeakBlocker.Libraries.SystemTools.Tests.Entities;
using LeakBlocker.Libraries.SystemTools.Tests.Entities.Implementations;
using LeakBlocker.Server.Service.InternalTools;
using LeakBlocker.Server.Service.InternalTools.LocalStorages;
using LeakBlocker.Server.Service.InternalTools.SingletonImplementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace LeakBlocker.Server.Service.Tests.InternalTools
{
    [TestClass]
    public sealed class SecurityObjectCacheTest : BaseTest
    {
        private IDomainSnapshot domainSnapshotSet;
        private IConfigurationStorage configurationStorage;

        [TestInitialize]
        public void Init()
        {
            configurationStorage = Mocks.StrictMock<IConfigurationStorage>();
            domainSnapshotSet = DomainSnapshotTest.Second;

            configurationStorage.Stub(x => x.CurrentFullConfiguration).Return(ProgramConfigurationTest.First);

            InternalObjects.Singletons.ConfigurationStorage.SetTestImplementation(configurationStorage);
            SystemObjects.Factories.DomainSnapshot.EnqueueTestImplementation(domainSnapshotSet);
            StorageObjects.Singletons.CredentialsManager.SetTestImplementation(new FakeCredentialsManager());
            SharedObjects.Factories.Scheduler.EnqueueConstructor((action, time, flag) =>
            {
                action();
                return new FakeScheduler();
            });
            SharedObjects.Singletons.ExceptionSuppressor.SetTestImplementation(new ExceptionSuppressor());
            SharedObjects.Factories.ThreadPool.EnqueueConstructor(val => new NativeThreadPool(val));
            SharedObjects.Singletons.AsyncInvoker.SetTestImplementation(new AsyncInvoker());
        }

        [TestMethod]
        public void ConstructorTest1()
        {
            //var testObject = Mocks.StrictMock<IDomainSnapshot>();

            //domainSnapshotSet.Stub(x => x.Data).Return(testObject);

            Mocks.ReplayAll();

            using (var item = new SecurityObjectCache())
            {
                //Assert.AreEqual(testObject, item.Data);
            }
        }

        [TestMethod]
        public void GetBaseDomainAccountByNameImmediatelyTest1()
        {
            IDomainSnapshot data = DomainSnapshotTest.First;

            const string computerName = "123";
            const string userName = "1234";
            const string password = "521";

            var systemAccountTools = Mocks.StrictMock<ISystemAccountTools>();

            systemAccountTools.Expect(x => x.GetBaseDomainAccountByName(computerName, new SystemAccessOptions(userName, password, computerName))).Return(BaseDomainAccountTest.objects.First());

            //domainSnapshotSet.Stub(x => x.Load(null)).IgnoreArguments();
            //domainSnapshotSet.Stub(x => x.Data).Return(data);

            SystemObjects.Singletons.SystemAccountTools.SetTestImplementation(systemAccountTools);

            Mocks.ReplayAll();

            using (var item = new SecurityObjectCache())
            {
                var result = item.GetBaseDomainAccountByNameImmediately(computerName, new SystemAccessOptions(userName, password, computerName));
                Assert.IsNotNull(result);
            }

            Mocks.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RequestImmediatelyTest1()
        {
            domainSnapshotSet = DomainSnapshotTest.First;

            Mocks.ReplayAll();

            using (var item = new SecurityObjectCache())
            {
                item.RequestUpdate(null);
            }
        }

        private sealed class FakeCredentialsManager : ICredentialsManager
        {
            public Credentials TryGetCredentials(BaseDomainAccount account)
            {
                return new Credentials(account, "qqq", "www");
            }

            public void UpdateCredentials(Credentials credentials)
            {
            }

            public ReadOnlySet<Credentials> GetAllCredentials()
            {
                return new[] { new Credentials(DomainAccountTest.objects.First(), "qqq", "www") }.ToReadOnlySet();
            }
        }

        private sealed class FakeScheduler : IScheduler
        {
            public TimeSpan Interval
            {
                get
                {
                    return TimeSpan.MaxValue;
                }
                set
                {
                }
            }

            public TimeSpan Duration
            {
                get
                {
                    return TimeSpan.MaxValue;
                }
                set
                {
                }
            }

            public void RunNow()
            {
            }

            public void Dispose()
            {
            }
        }

    }
}
