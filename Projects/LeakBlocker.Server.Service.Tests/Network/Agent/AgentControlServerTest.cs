using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Agent.Core;
using LeakBlocker.Agent.Core.Implementations;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Cryptography;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Entities.Settings;
using LeakBlocker.Libraries.Common.Tests.Cryptography;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;
using LeakBlocker.Libraries.Common.Tests.Entities.Settings;
using LeakBlocker.Libraries.Storage;
using LeakBlocker.Libraries.SystemTools;
using LeakBlocker.Libraries.SystemTools.Entities;
using LeakBlocker.Server.Service.InternalTools;
using LeakBlocker.Server.Service.InternalTools.LocalStorages;
using LeakBlocker.Server.Service.Network.Agent;
using LeakBlocker.ServerShared.AgentCommunication;
using LeakBlocker.ServerShared.AgentCommunication.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using LeakBlocker.Libraries.Common.Collections;

namespace LeakBlocker.Server.Service.Tests.Network.Agent
{
    [TestClass]
    public sealed class AgentControlServerTest : BaseNetworkTest
    {
        private static readonly BaseComputerAccount agentComputer = BaseComputerAccountTest.First;
        private static readonly SymmetricEncryptionKey key = SymmetricEncryptionKeyTest.First;

        private IAgentPrivateStorage agentPrivateStorage;
        private IAgentKeyManager agentKeyManager;
        private ISecurityObjectCache securityObjectsCache;
        private IDomainSnapshot securityObjectsContainer;
        private ISystemAccountTools systemAccountTools;

        private AgentSessionManager sessionManager;

        [TestInitialize]
        public void Init()
        {
            Initialize();

            agentPrivateStorage = Mocks.StrictMock<IAgentPrivateStorage>();
            agentKeyManager = Mocks.StrictMock<IAgentKeyManager>();
            securityObjectsCache = Mocks.StrictMock<ISecurityObjectCache>();
            securityObjectsContainer = Mocks.StrictMock<IDomainSnapshot>();
            systemAccountTools = Mocks.StrictMock<ISystemAccountTools>();

            sessionManager = new AgentSessionManager();

            agentPrivateStorage.Stub(x => x.ServerAddress).Return(Environment.MachineName);
            agentPrivateStorage.Stub(x => x.ServerPort).Return(SharedObjects.Constants.DefaultTcpPort);
            agentPrivateStorage.Stub(x => x.SecretKey).Return(key.ToBase64String());
            agentKeyManager.Stub(x => x.GetAgentKey(agentComputer)).Return(key);
            securityObjectsCache.Stub(x => x.Data).Return(securityObjectsContainer);
            securityObjectsContainer.Stub(x => x.Computers).Return(new []{agentComputer}.ToReadOnlySet());
            systemAccountTools.Stub(x => x.LocalComputer).Return(agentComputer);

            SystemObjects.Singletons.SystemAccountTools.SetTestImplementation(systemAccountTools);
            AgentObjects.Singletons.AgentPrivateStorage.SetTestImplementation(agentPrivateStorage);
            InternalObjects.Singletons.AgentKeyManager.SetTestImplementation(agentKeyManager);
            InternalObjects.Singletons.SecurityObjectCache.SetTestImplementation(securityObjectsCache);
            InternalObjects.Singletons.AgentSessionManager.SetTestImplementation(sessionManager);
        }

        [TestCleanup]
        public void Cleanup()
        {
            Disposable.DisposeSafe(sessionManager);

            sessionManager = null;
        }

        [TestMethod]
        public void SendShutdownNotification()
        {
            var agentStatusStore = Mocks.StrictMock<IAgentStatusStore>();

            InternalObjects.Singletons.AgentStatusStore.SetTestImplementation(agentStatusStore);

            agentStatusStore.Stub(x => x.SetComputerData(null, null)).IgnoreArguments();

            Mocks.ReplayAll();

            using (InitServer<AgentControlServer>())
            {
                using (IAgentControlService client = new AgentControlServiceClient())
                {
                    client.SendShutdownNotification();
                }
            }
        }
        [TestMethod]
        public void Synchronize()
        {
            ProgramConfiguration resultConfiguration = ProgramConfigurationTest.First;

            var agentStatusStore = Mocks.StrictMock<IAgentStatusStore>();
            var auditItemsManager = Mocks.StrictMock<IAuditItemsManager>();
            var configurationStorage = Mocks.StrictMock<IConfigurationStorage>();
            var scopeManager = Mocks.StrictMock<IScopeManager>();

            InternalObjects.Singletons.AgentStatusStore.SetTestImplementation(agentStatusStore);
            StorageObjects.Singletons.AuditItemsManager.SetTestImplementation(auditItemsManager);
            InternalObjects.Singletons.ConfigurationStorage.SetTestImplementation(configurationStorage);
            InternalObjects.Singletons.ScopeManager.SetTestImplementation(scopeManager);

            agentStatusStore.Stub(x => x.SetComputerData(null, null)).IgnoreArguments();
            auditItemsManager.Expect(x => x.AddItems(null)).IgnoreArguments();
            agentStatusStore.Stub(x => x.IsUnderLicense(agentComputer)).Return(true);
            agentStatusStore.Expect(x => x.UpdateLastCommunicationTimeAsCurrent(agentComputer));
            configurationStorage.Stub(x => x.CurrentFullConfiguration).Return(resultConfiguration);
            scopeManager.Stub(x => x.CurrentScope()).Return(BaseComputerAccountTest.objects.Objects);


            Mocks.ReplayAll();

            using (InitServer<AgentControlServer>())
            {
                using (IAgentControlService client = new AgentControlServiceClient())
                {
                    var result = client.Synchronize(AgentStateTest.First);

                    Assert.IsNotNull(result);
                    Assert.AreEqual(resultConfiguration, result.Settings);
                    Assert.IsTrue(result.Licensed);
                }
            }
        }
    }
}
