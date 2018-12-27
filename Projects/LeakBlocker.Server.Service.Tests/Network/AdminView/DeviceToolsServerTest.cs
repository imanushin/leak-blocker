using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.AdminView.Desktop.Network;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Tests.Entities;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;
using LeakBlocker.Libraries.Storage;
using LeakBlocker.Libraries.SystemTools;
using LeakBlocker.Libraries.SystemTools.Devices;
using LeakBlocker.Libraries.SystemTools.Entities;
using LeakBlocker.Server.Service.Network.AdminView;
using LeakBlocker.ServerShared.AdminViewCommunication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace LeakBlocker.Server.Service.Tests.Network.AdminView
{
    [TestClass]
    public sealed class DeviceToolsServerTest : BaseAdminViewNetworkTest
    {
        private ISystemAccountTools systemAccountTools;

        [TestInitialize]
        public void Init()
        {
            Initialize();

            systemAccountTools = Mocks.StrictMock<ISystemAccountTools>();

            SystemObjects.Singletons.SystemAccountTools.SetTestImplementation(systemAccountTools);
        }

        [TestMethod]
        public void GetServerDevices()
        {
            var deviceProvider = Mocks.StrictMock<IDeviceProvider>();

            ReadOnlySet<DeviceDescription> serverDevices = DeviceDescriptionTest.objects.Take(2).ToReadOnlySet();

            SystemObjects.Singletons.DeviceProvider.SetTestImplementation(deviceProvider);

            deviceProvider.Stub(x => x.EnumerateDevices()).IgnoreArguments().Return(serverDevices);

            Mocks.ReplayAll();

            using (InitServer<DeviceToolsServer>())
            {
                using (IDeviceTools client = new DeviceToolsClient())
                {
                    var result = client.GetConnectedDevices();

                    Assert.AreEqual(serverDevices, result);
                }
            }

            Mocks.VerifyAll();
        }
    }
}
