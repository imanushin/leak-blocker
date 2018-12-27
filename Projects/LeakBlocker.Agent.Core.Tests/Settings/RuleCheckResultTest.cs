using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Agent.Core.Settings;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules;
using LeakBlocker.Libraries.Common.Tests.Entities;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;

namespace LeakBlocker.Agent.Core.Tests.Settings
{
    partial class RuleCheckResultTest
    {
        private static IEnumerable<RuleCheckResult> GetInstances()
        {
            yield return new RuleCheckResult(ReadOnlySet<BaseUserAccount>.Empty, ReadOnlySet<DeviceDescription>.Empty, (u, d) => null);

            var access = EnumHelper<DeviceAccessType>.Values.ToArray();
            var audit = EnumHelper<AuditActionType>.Values.ToArray();

            int index = 0;

            yield return new RuleCheckResult
                (
                    BaseUserAccountTest.objects.Objects,
                    DeviceDescriptionTest.objects.Objects,
                    (user, device) => new CommonActionData(access[(index++) % access.Length], audit[(index++) % audit.Length]));
        }

        protected override bool SkipSerializationTest()
        {
            return true;
        }

        protected override void CheckArgumentExceptionParameter(string expectedParameterName, string actualParameterName)
        {
            return;
        }

        internal static RuleCheckResult CreateWithFilling(IEnumerable<BaseUserAccount> users, IEnumerable<DeviceDescription> devices, CommonActionData commonActionData)
        {
            return new RuleCheckResult(users.ToReadOnlySet(), devices.ToReadOnlySet(), (u, d) => commonActionData);
        }
    }
}
