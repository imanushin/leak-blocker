using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules.Conditions;

namespace LeakBlocker.Libraries.Storage.Entities
{
    partial class DbDeviceListRuleCondition
    {
        private DeviceListRuleCondition ForceGetDeviceListRuleCondition()
        {
            return new DeviceListRuleCondition(Not, Devices.Select(device => device.GetDeviceDescription()).ToReadOnlySet());
        }
    }
}
