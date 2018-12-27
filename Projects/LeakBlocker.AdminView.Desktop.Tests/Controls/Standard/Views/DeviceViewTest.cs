using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.AdminView.Desktop.Controls.Standard.Views;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Tests.Entities;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.AdminView.Desktop.Tests.Controls.Standard.Views
{
    [TestClass]
    public sealed class DeviceViewTest
    {
        [TestMethod]
        public void SimpleDeviceTest()
        {
            foreach (DeviceAccessType state in EnumHelper<DeviceAccessType>.Values)
            {
                DeviceAccessType localState = state;

                var map = new DeviceAccessMap(
                    BaseUserAccountTest.objects.Take(1).ToReadOnlySet(),
                    DeviceDescriptionTest.objects.Take(1).ToReadOnlySet(),
                    (user, device) => localState);

                new DeviceView(map, DeviceDescriptionTest.First);
            }
        }
    }
}
