using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using LeakBlocker.AdminView.Desktop.Network;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;
using LeakBlocker.Libraries.Storage;
using LeakBlocker.Libraries.SystemTools;
using LeakBlocker.Libraries.SystemTools.Entities.Implementations;
using LeakBlocker.Libraries.SystemTools.Entities.Management;
using LeakBlocker.Libraries.SystemTools.Tests.Entities;
using LeakBlocker.Libraries.SystemTools.Tests.Entities.Implementations;
using LeakBlocker.Server.Service.InternalTools;
using LeakBlocker.Server.Service.Network.AdminView;
using LeakBlocker.ServerShared.AdminViewCommunication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace LeakBlocker.Server.Service.Tests.Network.AdminView
{
    [TestClass]
    public sealed class AccountToolsServerTest : BaseAdminViewNetworkTest
    {
        private ISecurityObjectCache cache;

        [TestInitialize]
        public void Init()
        {
            cache = Mocks.StrictMock<ISecurityObjectCache>();
            InternalObjects.Singletons.SecurityObjectCache.SetTestImplementation(cache);

            cache.Stub(x => x.RequestUpdate(null)).IgnoreArguments().Return(Guid.Empty);

            Initialize();
        }

        [TestMethod]
        public void FindDnsDomains_Cached()
        {
            cache.Stub(x => x.Data).Return(DomainSnapshotTest.Second);

            Mocks.ReplayAll();

            using (InitServer<AccountToolsServer>())
            {
                using (IAccountTools client = new AccountToolsClient())
                {
                    var domains = client.FindDnsDomains();

                    Assert.IsNotNull(domains);

                    var currentMachineName = Environment.MachineName.ToUpperInvariant();
                    Assert.IsNull(domains.FirstOrDefault(item => item.ToUpperInvariant().Contains(currentMachineName)));
                }
            }

            Mocks.VerifyAll();
        }

        [TestMethod]
        public void FindDnsDomains_NewData()
        {
            cache.Stub(x => x.Data).Return(new DomainSnapshot());

            Mocks.ReplayAll();

            using (InitServer<AccountToolsServer>())
            {
                using (IAccountTools client = new AccountToolsClient())
                {
                    var domains = client.FindDnsDomains();

                    Assert.IsNotNull(domains);

                    var currentMachineName = Environment.MachineName.ToUpperInvariant();
                    Assert.IsNotNull(domains.FirstOrDefault(item => item.ToUpperInvariant().Contains(currentMachineName)));
                }
            }

            Mocks.VerifyAll();
        }


        [TestMethod]
        public void GetPreferrableDomain()
        {
            cache.Stub(x => x.Data).Return(DomainSnapshotTest.First);

            Mocks.ReplayAll();

            using (InitServer<AccountToolsServer>())
            {
                using (IAccountTools client = new AccountToolsClient())
                {
                    var domain = client.GetPreferableDomain();

                    Assert.IsTrue(Dns.GetHostEntry("localhost").HostName.ToUpperInvariant().Contains(domain.ToUpperInvariant()));
                }
            }

            Mocks.VerifyAll();
        }

        [TestMethod]
        public void GetAvailableScopes()
        {
            var container = DomainSnapshotTest.Second;
            cache.Stub(x => x.Data).Return(container);

            Mocks.ReplayAll();

            using (InitServer<AccountToolsServer>())
            {
                using (IAccountTools client = new AccountToolsClient())
                {
                    var computerScopes = client.GetAvailableComputerScopes();
                    var userScopes = client.GetAvailableUserScopes();

                    Assert.IsNotNull(computerScopes);
                    Assert.IsNotNull(userScopes);

                    var users = container.Users.Select(item => new Scope(item)).ToReadOnlySet();
                    var computers = container.Computers.Select(item => new Scope(item)).ToReadOnlySet();
                    var localGroups = container.Groups.OfType<LocalGroupAccount>().Select(item => new Scope(item)).ToReadOnlySet();

                    Assert.AreEqual(computerScopes.Without(users).ToReadOnlySet(), computerScopes);//Не содержит пользователей
                    Assert.AreEqual(computerScopes.Without(localGroups).ToReadOnlySet(), computerScopes);//Не содержит локальных групп
                    Assert.AreEqual(userScopes.Without(computers).ToReadOnlySet(), userScopes);//Не содержит компьютеров
                }
            }

            Mocks.VerifyAll();
        }

        [TestMethod]
        public void GetAvailableComputers()
        {
            var container = new DomainSnapshot();
            var accountsManager = Mocks.StrictMock<IAccountManager>();

            accountsManager.Stub(x => x.GetSavedComputers()).Return(BaseComputerAccountTest.objects.ToReadOnlySet());
            cache.Stub(x => x.Data).Return(container);

            StorageObjects.Singletons.AccountManager.SetTestImplementation(accountsManager);

            Mocks.ReplayAll();

            using (InitServer<AccountToolsServer>())
            {
                using (IAccountTools client = new AccountToolsClient())
                {
                    var computers = client.GetAvailableComputers();

                    Assert.IsNotNull(computers);
                    Assert.AreEqual(container.Computers.Count + BaseComputerAccountTest.objects.Objects.Count, computers.Count);
                }
            }

            Mocks.VerifyAll();
        }

        [TestMethod]
        public void GetAvailableUsers()
        {
            var container = new DomainSnapshot();
            var accountsManager = Mocks.StrictMock<IAccountManager>();

            accountsManager.Stub(x => x.GetSavedUsers()).Return(BaseUserAccountTest.objects.ToReadOnlySet());
            cache.Stub(x => x.Data).Return(container);

            StorageObjects.Singletons.AccountManager.SetTestImplementation(accountsManager);

            Mocks.ReplayAll();

            using (InitServer<AccountToolsServer>())
            {
                using (IAccountTools client = new AccountToolsClient())
                {
                    var users = client.GetAvailableUsers();

                    Assert.IsNotNull(users);
                    Assert.AreEqual(container.Users.Count + BaseUserAccountTest.objects.Objects.Count, users.Count);
                }
            }

            Mocks.VerifyAll();
        }

        [TestMethod]
        public void GetComputers()
        {
            var container = DomainSnapshotTest.Second;

            cache.Stub(x => x.Data).Return(container);

            Mocks.ReplayAll();

            using (InitServer<AccountToolsServer>())
            {
                using (IAccountTools client = new AccountToolsClient())
                {
                    foreach (Scope scope in container.Domains.Select(domain => new Scope(domain)))
                    {
                        var computers = client.GetComputers(new ReadOnlySet<Scope>(new[] { scope }));

                        Assert.IsNotNull(computers);
                    }

                    foreach (Scope scope in container.Computers.Select(computer => new Scope(computer)))
                    {
                        var computers = client.GetComputers(new ReadOnlySet<Scope>(new[] { scope }));

                        Assert.IsNotNull(computers);
                    }

                    foreach (Scope scope in container.Groups.OfType<DomainGroupAccount>().Select(group => new Scope(group)))
                    {
                        var computers = client.GetComputers(new ReadOnlySet<Scope>(new[] { scope }));

                        Assert.IsNotNull(computers);
                    }

                    foreach (Scope scope in container.OrganizationalUnits.Select(ou => new Scope(ou)))
                    {
                        var computers = client.GetComputers(new ReadOnlySet<Scope>(new[] { scope }));

                        Assert.IsNotNull(computers);
                    }
                }
            }

            Mocks.VerifyAll();
        }


        [TestMethod]
        public void CheckAndSetCredentials()
        {
            var credentialsManager = Mocks.StrictMock<ICredentialsManager>();

            StorageObjects.Singletons.CredentialsManager.SetTestImplementation(credentialsManager);

            const string user = "user";
            const string password = "password";
            const string domain = "domain";

            BaseDomainAccount expectedResult = BaseDomainAccountTest.objects.First();

            cache.Stub(x => x.GetBaseDomainAccountByNameImmediately(domain, new SystemAccessOptions(user, password, domain))).Return(expectedResult);
            credentialsManager.Stub(x => x.UpdateCredentials(null)).IgnoreArguments();

            Mocks.ReplayAll();

            using (InitServer<AccountToolsServer>())
            {
                using (IAccountTools client = new AccountToolsClient())
                {
                    DomainUpdateRequest actutualResult = client.CheckAndSetCredentials(new DomainCredentials(user, password, domain));

                    Assert.AreEqual(expectedResult, actutualResult.Domain);
                }
            }

            Mocks.VerifyAll();
        }
    }
}
