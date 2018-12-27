using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Tests;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;
using LeakBlocker.Libraries.Common.Tests.Entities.Settings;
using LeakBlocker.Libraries.Storage;
using LeakBlocker.Libraries.SystemTools;
using LeakBlocker.Libraries.SystemTools.Entities;
using LeakBlocker.Server.Service.InternalTools;
using LeakBlocker.Server.Service.InternalTools.LocalStorages;
using LeakBlocker.Server.Service.InternalTools.SingletonImplementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace LeakBlocker.Server.Service.Tests.InternalTools
{
    [TestClass]
    public sealed class AuditItemHelperTest : BaseTest
    {
        private static readonly BaseComputerAccount computer = BaseComputerAccountTest.objects.First();
        private static readonly BaseUserAccount serviceUser = BaseUserAccountTest.objects.First();

        private AuditItemHelper target;

        [TestInitialize]
        public void Initialize()
        {
            var systemAccountTools = Mocks.StrictMock<ISystemAccountTools>();
            var auditItemsManager = Mocks.StrictMock<IAuditItemsManager>();
            var configurationStorage = Mocks.StrictMock<IConfigurationStorage>();

            systemAccountTools.Stub(x => x.LocalComputer).Return(computer);
            configurationStorage.Stub(x => x.CurrentFullConfiguration).Return(ProgramConfigurationTest.First);

            InternalObjects.Singletons.ConfigurationStorage.SetTestImplementation(configurationStorage);
            SystemObjects.Singletons.SystemAccountTools.SetTestImplementation(systemAccountTools);
            StorageObjects.Singletons.AuditItemsManager.SetTestImplementation(auditItemsManager);

            auditItemsManager.Expect(x => x.AddItem(null)).IgnoreArguments();

            Mocks.ReplayAll();

            target = new AuditItemHelper();
        }

        [TestMethod]
        public void NotifyInstallationStarted()
        {
            target.NotifyInstallationStarted(computer);
        }

        [TestMethod]
        public void AgentDoesNotResponded()
        {
            target.AgentIsNotResponding(computer);
        }

        [TestMethod]
        public void AgentInstallationSucceded()
        {
            target.AgentInstallationSucceded(computer);
        }

        [TestMethod]
        public void AgentInstallationFailed()
        {
            target.AgentInstallationFailed(computer, "test error");
        }

        [TestMethod]
        public void ConfigurationChanged()
        {
            target.ConfigurationChanged(serviceUser, 123);
        }

        [TestMethod]
        public void ReportsDoesNotConfigured()
        {
            target.ReportsDoesNotConfigured();
        }

        [TestMethod]
        public void SendingReportFailed()
        {
            target.SendingReportFailed("error text");
        }

        [TestMethod]
        public void DomainMemberIsNotAccessible_OU()
        {
            target.DomainMemberIsNotAccessible(OrganizationalUnitTest.First);
        }

        [TestMethod]
        public void DomainMemberIsNotAccessible_Group()
        {
            target.DomainMemberIsNotAccessible(DomainGroupAccountTest.First);
        }

        [TestMethod]
        public void DomainConnectionFailed()
        {
            target.DomainConnectionFailed(DomainAccountTest.First, "asd\\admin", new Exception("qqq"));
        }
    }
}
