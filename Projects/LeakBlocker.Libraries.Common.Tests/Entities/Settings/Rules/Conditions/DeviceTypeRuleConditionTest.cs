using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules.Conditions;

namespace LeakBlocker.Libraries.Common.Tests.Entities.Settings.Rules.Conditions
{
    partial class DeviceTypeRuleConditionTest
    {
        private static IEnumerable<DeviceTypeRuleCondition> GetInstances()
        {
            foreach (bool not in new[]{true,false})
            {
                foreach (DeviceCategory deviceCategory in EnumHelper<DeviceCategory>.Values)
                {
                    yield return new DeviceTypeRuleCondition(deviceCategory,not);
                }
            }
        }
    }
}
