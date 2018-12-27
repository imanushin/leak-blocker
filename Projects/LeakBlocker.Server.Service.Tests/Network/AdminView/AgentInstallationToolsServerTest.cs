using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeakBlocker.AdminView.Desktop.Network;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;
using LeakBlocker.Server.Service.InternalTools;
using LeakBlocker.Server.Service.Network.AdminView;
using LeakBlocker.ServerShared.AdminViewCommunication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace LeakBlocker.Server.Service.Tests.Network.AdminView
{
    [TestClass]
    public sealed class AgentInstallationToolsServerTest : BaseAdminViewNetworkTest
    {
        private static readonly ReadOnlySet<BaseComputerAccount> computers = BaseComputerAccountTest.objects.Take(5).ToReadOnlySet();

        private IAgentManager agentManager;

        [TestInitialize]
        public void Init()
        {
            Initialize();

            agentManager = Mocks.StrictMock<IAgentManager>();
            InternalObjects.Singletons.AgentManager.SetTestImplementation(agentManager);
        }

        [TestMethod]
        public void ForceInstallation()
        {
            foreach (BaseComputerAccount computer in computers)
            {
                BaseComputerAccount localComputer = computer;

                agentManager.Expect(x => x.InstallAgent(localComputer));
            }

            Mocks.ReplayAll();

            using (InitServer<AgentInstallationToolsServer>())
            {
                using (IAgentInstallationTools client = new AgentInstallationToolsClient())
                {
                    client.ForceInstallation(computers);
                }
            }

            Mocks.VerifyAll();
        }
    }
}
