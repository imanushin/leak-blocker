using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Agent.Core.Settings;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules;

namespace LeakBlocker.Agent.Core.Tests.Settings
{
    partial class CommonActionDataTest
    {
        private static IEnumerable<CommonActionData> GetInstances()
        {
            foreach (AuditActionType auditItemType in EnumHelper<AuditActionType>.Values)
            {
                foreach (DeviceAccessType deviceAccessType in EnumHelper<DeviceAccessType>.Values)
                {
                    yield return new CommonActionData(deviceAccessType, auditItemType);
                }
            }
        }

        protected override bool SkipSerializationTest()
        {
            return true;
        }
    }
}
