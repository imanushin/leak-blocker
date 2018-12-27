using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.AdminView.Desktop.Network;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Settings;
using LeakBlocker.Libraries.Common.Tests.Entities.Settings;
using LeakBlocker.Server.Service.InternalTools;
using LeakBlocker.Server.Service.InternalTools.LocalStorages;
using LeakBlocker.Server.Service.Network.AdminView;
using LeakBlocker.ServerShared.AdminViewCommunication;
using LeakBlocker.ServerShared.AdminViewCommunication.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace LeakBlocker.Server.Service.Tests.Network.AdminView
{
    [TestClass]
    public sealed class ConfigurationToolsServerTest : BaseAdminViewNetworkTest
    {
        [TestInitialize]
        public void Init()
        {
            Initialize();
        }

        [TestMethod]
        public void GetLastConfiguration()
        {
            var configurationStorage = Mocks.StrictMock<IConfigurationStorage>();

            InternalObjects.Singletons.ConfigurationStorage.SetTestImplementation(configurationStorage);

            SimpleConfiguration expectedConfig = SimpleConfigurationTest.objects.First();

            configurationStorage.Stub(x => x.CurrentOrDefault).Return(expectedConfig);

            Mocks.ReplayAll();

            using (InitServer<ConfigurationToolsServer>())
            {
                using (IConfigurationTools client = new ConfigurationToolsClient())
                {
                    var configuration = client.LastConfiguration();

                    Assert.AreEqual(expectedConfig, configuration);
                }
            }

            Mocks.VerifyAll();
        }

        [TestMethod]
        public void SetConfiguration()
        {
            var configurationStorage = Mocks.StrictMock<IConfigurationStorage>();
            var scopeManager = Mocks.StrictMock<IScopeManager>();
            var agentStatusObserver = Mocks.StrictMock<IAgentStatusObserver>();
            var auditItemHelper = Mocks.StrictMock<IAuditItemHelper>();
            var accountResolver = Mocks.StrictMock<IAccountResolver>();

            InternalObjects.Singletons.ConfigurationStorage.SetTestImplementation(configurationStorage);
            InternalObjects.Singletons.ScopeManager.SetTestImplementation(scopeManager);
            InternalObjects.Singletons.AgentStatusObserver.SetTestImplementation(agentStatusObserver);
            InternalObjects.Singletons.AuditItemHelper.SetTestImplementation(auditItemHelper);
            InternalObjects.Singletons.AccountResolver.SetTestImplementation(accountResolver);

            SimpleConfiguration config = SimpleConfigurationTest.First;
            ProgramConfiguration fullConfig = ProgramConfigurationTest.objects.First();

            auditItemHelper.Expect(x => x.ConfigurationChanged(NetworkUser, fullConfig.ConfigurationVersion));
            configurationStorage.Stub(x => x.CurrentFullConfiguration).Return(fullConfig);
            configurationStorage.Expect(x => x.Save(config));
            scopeManager.Expect(x => x.ForceUpdateScope());
            agentStatusObserver.Expect(x => x.EnqueueObserving());
            accountResolver.Stub(x => x.ResolveUser(NetworkUser.Identifier)).Return(NetworkUser);

            Mocks.ReplayAll();

            using (InitServer<ConfigurationToolsServer>())
            {
                using (IConfigurationTools client = new ConfigurationToolsClient())
                {
                    client.SaveConfiguration(config);
                }
            }

            Mocks.VerifyAll();//Проверяем, что конфигурация таки сохранится в базу
        }

        [TestMethod]
        public void SetConfiguration_True()
        {
            var configurationStorage = Mocks.StrictMock<IConfigurationStorage>();

            InternalObjects.Singletons.ConfigurationStorage.SetTestImplementation(configurationStorage);

            configurationStorage.Stub(x => x.Current).Return(SimpleConfigurationTest.First);

            Mocks.ReplayAll();

            using (InitServer<ConfigurationToolsServer>())
            {
                using (IConfigurationTools client = new ConfigurationToolsClient())
                {
                    Assert.IsTrue(client.HasConfiguration());
                }
            }
        }

        [TestMethod]
        public void SetConfiguration_False()
        {
            var configurationStorage = Mocks.StrictMock<IConfigurationStorage>();

            InternalObjects.Singletons.ConfigurationStorage.SetTestImplementation(configurationStorage);

            configurationStorage.Stub(x => x.Current).Return(null);

            Mocks.ReplayAll();

            using (InitServer<ConfigurationToolsServer>())
            {
                using (IConfigurationTools client = new ConfigurationToolsClient())
                {
                    Assert.IsFalse(client.HasConfiguration());
                }
            }
        }
    }
}
