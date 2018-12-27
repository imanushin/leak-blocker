using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Tests.Entities;

namespace LeakBlocker.ServerShared.AgentCommunication.Tests
{
    partial class AgentStateTest
    {
        private static IEnumerable<AgentState> GetInstances()
        {
            foreach (AuditItemPackage auditItemPackage in AuditItemPackageTest.objects)
            {
                foreach (DeviceAccessMap deviceAccessMap in DeviceAccessMapTest.objects)
                {
                    yield return new AgentState(auditItemPackage, deviceAccessMap);
                }
            }
        }
    }
}
