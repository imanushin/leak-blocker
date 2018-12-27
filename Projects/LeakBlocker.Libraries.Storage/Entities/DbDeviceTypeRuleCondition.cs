using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules.Conditions;

namespace LeakBlocker.Libraries.Storage.Entities
{
    partial class DbDeviceTypeRuleCondition
    {
        private DeviceTypeRuleCondition ForceGetDeviceTypeRuleCondition()
        {
            return new DeviceTypeRuleCondition(DeviceType, Not);
        }
    }
}
