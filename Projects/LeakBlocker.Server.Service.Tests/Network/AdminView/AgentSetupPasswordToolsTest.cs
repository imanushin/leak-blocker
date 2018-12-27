using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeakBlocker.AdminView.Desktop.Network;
using LeakBlocker.Server.Service.InternalTools;
using LeakBlocker.Server.Service.Network.AdminView;
using LeakBlocker.ServerShared.AdminViewCommunication;
using LeakBlocker.ServerShared.AdminViewCommunication.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace LeakBlocker.Server.Service.Tests.Network.AdminView
{
    [TestClass]
    public sealed class AgentSetupPasswordToolsTest : BaseAdminViewNetworkTest
    {
        private static readonly AgentSetupPassword password = AgentSetupPasswordTest.First;

        private IAgentSetupPasswordManager agentSetupPasswordManager;
        private IMailer mailer;

        [TestInitialize]
        public void Init()
        {
            Initialize();

            mailer = Mocks.StrictMock<IMailer>();
            agentSetupPasswordManager = Mocks.StrictMock<IAgentSetupPasswordManager>();

            InternalObjects.Singletons.AgentSetupPasswordManager.SetTestImplementation(agentSetupPasswordManager);
            InternalObjects.Singletons.Mailer.SetTestImplementation(mailer);
        }

        [TestMethod]
        public void GetPassword()
        {
            agentSetupPasswordManager.Stub(x => x.Current).Return(password);

            Mocks.ReplayAll();

            using (InitServer<AgentSetupPasswordToolsServer>())
            {
                using (IAgentSetupPasswordTools client = new AgentSetupPasswordToolsClient())
                {
                    var result = client.GetPassword();

                    Assert.AreEqual(password, result);
                }
            }
        }

        [TestMethod]
        public void SendPassword()
        {
            agentSetupPasswordManager.Stub(x => x.Current).Return(password);
            mailer.Stub(x => x.SendMessage(null, null)).IgnoreArguments();

            Mocks.ReplayAll();

            using (InitServer<AgentSetupPasswordToolsServer>())
            {
                using (IAgentSetupPasswordTools client = new AgentSetupPasswordToolsClient())
                {
                    client.SendPassword(new EmailSettings("123@mail.com", "to@mail.com", "smtpHost", 234, true, true, "234234", "424234"));
                }
            }
        }
    }
}
