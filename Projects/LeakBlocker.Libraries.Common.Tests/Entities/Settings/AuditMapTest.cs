using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Settings;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;

namespace LeakBlocker.Libraries.Common.Tests.Entities.Settings
{
    partial class AuditMapTest
    {
        private static IEnumerable<AuditMap> GetInstances()
        {
            yield return new AuditMap(
                    BaseUserAccountTest.objects.Take(1).ToReadOnlySet(),
                    DeviceDescriptionTest.objects.Take(1).ToReadOnlySet(),
                    (user, device) => AuditActionType.Device);

            int item = 0;

            yield return new AuditMap(
                BaseUserAccountTest.objects.Objects,
                DeviceDescriptionTest.objects.Objects,
                (user, device) => EnumHelper<AuditActionType>.Values.ToList()[(item++) % EnumHelper<AuditActionType>.Values.Count]);

        }

        protected override void CheckArgumentExceptionParameter(string expectedParameterName, string actualParameterName)
        {
        }
    }
}
