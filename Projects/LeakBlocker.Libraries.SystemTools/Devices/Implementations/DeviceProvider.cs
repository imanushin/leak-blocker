using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;

namespace LeakBlocker.Libraries.SystemTools.Devices.Implementations
{
    internal sealed class DeviceProvider : IDeviceProvider
    {
        public ReadOnlySet<ISystemDevice> EnumerateSystemDevices(bool includeOffline = false)
        {
            return SystemDevice.EnumerateDevices(includeOffline);
        }

        public ReadOnlySet<DeviceDescription> EnumerateDevices()
        {
            return EnumerateSystemDevices().SelectMany(device => device.Convert().Values).ToReadOnlySet();
        }

    }
}

