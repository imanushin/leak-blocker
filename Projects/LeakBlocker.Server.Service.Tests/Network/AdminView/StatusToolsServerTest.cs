using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.AdminView.Desktop.Network;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;
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
    public sealed class StatusToolsServerTest : BaseAdminViewNetworkTest
    {
        [TestInitialize]
        public void Init()
        {
            Initialize();
        }


        [TestMethod]
        public void GetStatuses()
        {
            var agentStatusManager = Mocks.StrictMock<IAgentStatusStore>();

            InternalObjects.Singletons.AgentStatusStore.SetTestImplementation(agentStatusManager);

            BaseComputerAccount computer = BaseComputerAccountTest.objects.First();
            ManagedComputerData data = ManagedComputerDataTest.objects.First();

            var dictionary = new[] { data }.ToDictionary(item => computer).ToReadOnlyDictionary();

            agentStatusManager.Stub(x => x.GetManagedScope()).Return(dictionary);

            Mocks.ReplayAll();

            using (InitServer<StatusToolsServer>())
            {
                using (IStatusTools client = new StatusToolsClient())
                {
                    var statuses = client.GetStatuses();

                    Assert.IsNotNull(statuses);
                    Assert.IsNotNull(statuses.FirstOrDefault());
                    Assert.AreEqual(computer, statuses.First().TargetComputer);
                    Assert.AreEqual(data, statuses.First().Data);
                }
            }

            Mocks.VerifyAll();
        }
    }
}
