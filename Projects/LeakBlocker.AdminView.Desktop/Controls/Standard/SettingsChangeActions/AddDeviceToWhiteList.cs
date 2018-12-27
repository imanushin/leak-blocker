using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.AdminView.Desktop.Resources;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Resources;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.AdminView.Desktop.Controls.Standard.SettingsChangeActions
{
    internal sealed class AddDeviceToWhiteList : BaseChangeAction
    {
        private readonly DeviceDescription device;

        public AddDeviceToWhiteList(DeviceDescription device)
        {
            Check.ObjectIsNotNull(device, "device");

            this.device = device;
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return device;
        }

        public override string ShortText
        {
            get
            {
                return AdminViewResources.AddDeviceToTheWhiteListTemplate.Combine(device);
            }
        }

        public override SimpleConfiguration AddSettings(SimpleConfiguration originalConfiguration)
        {
            return originalConfiguration.GetFromCurrent(excludedDevices: originalConfiguration.ExcludedDevices.UnionWith(device));
        }

        protected override string GetString()
        {
            return ShortText;
        }
    }
}
