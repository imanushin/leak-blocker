using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Agent.Core.Implementations.AgentDataStorageObjects;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Tests.Entities;

namespace LeakBlocker.Agent.Core.Tests.Implementations.AgentDataStorageObjects
{
    partial class DeviceStateTest
    {
        static IEnumerable<DeviceState> GetInstances()
        {
            foreach (DeviceDescription device in DeviceDescriptionTest.objects)
            {
                foreach (DeviceAccessType state in EnumHelper<DeviceAccessType>.Values)
                {
                    yield return new DeviceState(device, state);
                }
            }
        }

    }
}
