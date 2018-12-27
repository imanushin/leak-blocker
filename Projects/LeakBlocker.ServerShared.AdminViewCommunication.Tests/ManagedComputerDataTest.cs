using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Tests.Entities;

namespace LeakBlocker.ServerShared.AdminViewCommunication.Tests
{
    partial class ManagedComputerDataTest
    {
        private static IEnumerable<ManagedComputerData> GetInstances()
        {
            foreach (DeviceAccessMap lockMap in DeviceAccessMapTest.objects)
            {
                foreach (ManagedComputerStatus status in EnumHelper<ManagedComputerStatus>.Values)
                {
                    yield return new ManagedComputerData(status, lockMap);
                }
            }
        }
    }
}
