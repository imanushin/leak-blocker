using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.AdminView.Desktop.Controls.Standard.SettingsChangeActions;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Tests.Entities;

namespace LeakBlocker.AdminView.Desktop.Tests.Controls.Standard.SettingsChangeActions
{
    partial class AddDeviceToWhiteListTest
    {
        private static IEnumerable<AddDeviceToWhiteList> GetInstances()
        {
            foreach (DeviceDescription device in DeviceDescriptionTest.objects)
            {
                yield return new AddDeviceToWhiteList(device);
            }
        }

        protected override bool SkipSerializationTest()
        {
            return true;
        }
    }
}
