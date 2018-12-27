using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Audit;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Tests;
using LeakBlocker.Libraries.Common.Tests.Entities;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;

namespace LeakBlocker.ServerShared.AgentCommunication.Tests
{
    partial class AgentAuditItemTest
    {
        private static IEnumerable<AgentAuditItem> GetInstances()
        {
            foreach (AuditItemType auditItemType in EnumHelper<AuditItemType>.Values)
            {
                foreach (Time time in TimeTest.objects)
                {
                    foreach (BaseUserAccount baseUserAccount in BaseUserAccountTest.objects)
                    {
                        foreach (string textData in new[] { "123", null })
                        {
                            foreach (string additionalTextData in new[] { "qwe", null })
                            {
                                foreach (DeviceDescription device in new[] { DeviceDescriptionTest.First, null })
                                {
                                    yield return new AgentAuditItem(auditItemType, time, baseUserAccount, textData, additionalTextData, device);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
