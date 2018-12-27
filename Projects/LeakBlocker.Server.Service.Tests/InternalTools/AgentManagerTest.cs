using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Tests;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;
using LeakBlocker.Libraries.Storage;
using LeakBlocker.Libraries.SystemTools;
using LeakBlocker.Libraries.SystemTools.ProcessTools;
using LeakBlocker.Server.Service.InternalTools;
using LeakBlocker.Server.Service.InternalTools.LocalStorages;
using LeakBlocker.Server.Service.InternalTools.SingletonImplementation;
using LeakBlocker.ServerShared.AdminViewCommunication;
using LeakBlocker.ServerShared.AdminViewCommunication.Tests;
using LeakBlocker.ServerShared.AgentCommunication;
using LeakBlocker.ServerShared.AgentCommunication.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using LeakBlocker.Libraries.Common;
using Rhino.Mocks.Exceptions;

namespace LeakBlocker.Server.Service.Tests.InternalTools
{
    [TestClass]
    public sealed class AgentManagerTest : BaseTest
    {
        private static readonly BaseComputerAccount agentComputerTurnedOn = BaseComputerAccountTest.First;
        private static readonly BaseComputerAccount agentComputerTurnedOff = BaseComputerAccountTest.Second;
        private static readonly BaseComputerAccount agentComputerFailure = BaseComputerAccountTest.Third;
        private static readonly ManagedComputerData managedComputerData = ManagedComputerDataTest.First;
        private static readonly Credentials credentials = CredentialsTest.First;

        private IPrerequisites prerequisites;
        private IAuditItemHelper auditItemHelper;
        private IAgentStatusStore agentStatusStore;
        private ICredentialsManager credentialsManager;
        private IAgentKeyManager agentKeyManager;
        private IAgentInstaller agentInstaller;

        [TestInitialize]
        public void Init()
        {
            prerequisites = Mocks.StrictMock<IPrerequisites>();
            auditItemHelper = Mocks.StrictMock<IAuditItemHelper>();
            agentStatusStore = Mocks.StrictMock<IAgentStatusStore>();
            credentialsManager = Mocks.StrictMock<ICredentialsManager>();
            agentKeyManager = Mocks.StrictMock<IAgentKeyManager>();
            agentInstaller = Mocks.StrictMock<IAgentInstaller>();

            InternalObjects.Singletons.AgentInstaller.SetTestImplementation(agentInstaller);
            InternalObjects.Singletons.AuditItemHelper.SetTestImplementation(auditItemHelper);
            InternalObjects.Singletons.AgentStatusStore.SetTestImplementation(agentStatusStore);
            InternalObjects.Singletons.AgentKeyManager.SetTestImplementation(agentKeyManager);
            StorageObjects.Singletons.CredentialsManager.SetTestImplementation(credentialsManager);
            SystemObjects.Singletons.Prerequisites.SetTestImplementation(prerequisites);

            prerequisites.Stub(x => x.GetRemoteSystemVersion(null)).IgnoreArguments().Return(new Version(1, 1, 1)).Repeat.Any();

            prerequisites.Stub(x => x.ComputerIsAccessible(agentComputerTurnedOn.FullName)).Return(true).Repeat.Any();
            prerequisites.Stub(x => x.ComputerIsAccessible(agentComputerTurnedOff.FullName)).Return(false).Repeat.Any();

            prerequisites.Stub(x => x.FirewallIsActive(agentComputerTurnedOn.FullName)).Return(false).Repeat.Any();
            prerequisites.Stub(x => x.FirewallIsActive(agentComputerTurnedOff.FullName)).Return(false).Repeat.Any();

            agentInstaller.Stub(x => x.Install(agentComputerTurnedOn, credentials)).Return(AgentInstallerStatus.Success);
            agentInstaller.Stub(x => x.Install(agentComputerFailure, credentials)).Throw(new Exception());
        }

        [TestCleanup]
        public void Cleanup() 
        {
            Mocks.VerifyAll();
        }

        [TestMethod]
        public void IsComputerTurnedOn_Success()
        {
            Mocks.ReplayAll();

            IAgentManager target = new AgentManager();

            Assert.IsTrue(target.IsComputerTurnedOn(agentComputerTurnedOn));
            Assert.IsFalse(target.IsComputerTurnedOn(agentComputerTurnedOff));
        }

        [TestMethod]
        public void IsComputerTurnedOn_Fail()
        {
            prerequisites = Mocks.StrictMock<IPrerequisites>();

            SystemObjects.Singletons.Prerequisites.SetTestImplementation(prerequisites);

            prerequisites.Stub(x => x.ComputerIsAccessible(agentComputerTurnedOn.FullName)).Throw(new ApplicationException());
            auditItemHelper.Expect(x => x.ErrorInAgentCommunications(null, null)).IgnoreArguments();

            Mocks.ReplayAll();

            IAgentManager target = new AgentManager();

            Assert.IsFalse(target.IsComputerTurnedOn(agentComputerTurnedOn));
        }

        [TestMethod]
        public void InstallAgent_Error()
        {
            auditItemHelper.Expect(x => x.NotifyInstallationStarted(agentComputerFailure));
            auditItemHelper.Expect(x => x.AgentInstallationFailed(agentComputerFailure, null)).IgnoreArguments();

            agentStatusStore.Stub(x => x.GetComputerData(agentComputerFailure)).Return(managedComputerData);
            agentStatusStore.Stub(x => x.SetComputerData(null, null)).IgnoreArguments();
            credentialsManager.Stub(x => x.TryGetCredentials(agentComputerFailure)).Return(credentials);
            agentStatusStore.Stub(x => x.UpdateLastCommunicationTimeAsCurrent(agentComputerFailure));

            prerequisites.Stub(x => x.FirewallIsActive(null)).IgnoreArguments().Return(false);

            Mocks.ReplayAll();

            IAgentManager target = new AgentManager();

            target.InstallAgent(agentComputerFailure);
        }

        [TestMethod]
        public void InstallAgent_Success()
        {
            auditItemHelper.Expect(x => x.NotifyInstallationStarted(agentComputerTurnedOn));
            auditItemHelper.Expect(x => x.AgentInstallationSucceded(agentComputerTurnedOn)).IgnoreArguments();
            agentStatusStore.Stub(x => x.GetComputerData(agentComputerTurnedOn)).Return(managedComputerData);
            agentStatusStore.Stub(x => x.SetComputerData(null, null)).IgnoreArguments();
            credentialsManager.Stub(x => x.TryGetCredentials(agentComputerTurnedOn)).Return(credentials);
            agentStatusStore.Stub(x => x.UpdateLastCommunicationTimeAsCurrent(agentComputerTurnedOn));

            Mocks.ReplayAll();

            IAgentManager target = new AgentManager();

            target.InstallAgent(agentComputerTurnedOn);
        }
    }
}
