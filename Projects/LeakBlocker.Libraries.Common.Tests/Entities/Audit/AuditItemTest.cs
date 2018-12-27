using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Audit;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;

namespace LeakBlocker.Libraries.Common.Tests.Entities.Audit
{
    partial class AuditItemTest
    {
        private static IEnumerable<AuditItem> GetInstances()
        {
            BaseComputerAccount computer = BaseComputerAccountTest.objects.First();
            BaseUserAccount user = BaseUserAccountTest.objects.First();
            var dateTime = TimeTest.objects.First();
            DeviceDescription device = DeviceDescriptionTest.objects.First();
            const string textData = "1234321";
            const string additionalTextData = "qwerty123";
            const int configurationVersion = 1;

            foreach (AuditItemType itemType in EnumHelper<AuditItemType>.Values)
            {
                yield return new AuditItem(itemType, dateTime, computer, user, textData, additionalTextData, device, configurationVersion);
            }
        }
    }
}
