using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;

namespace LeakBlocker.Libraries.SystemTools.Devices
{
    /// <summary>
    /// Interface for device provider.
    /// </summary>
    public interface IDeviceProvider
    {
        /// <summary>
        /// Returns list of currently present devices.
        /// </summary>
        /// <param name="includeOffline">Include devices that were previously connected to the computer.</param>
        /// <returns>List of devices.</returns>
        ReadOnlySet<ISystemDevice> EnumerateSystemDevices(bool includeOffline = false);

        /// <summary>
        /// Returns list of currently present devices.
        /// </summary>
        /// <returns>List of devices.</returns>
        ReadOnlySet<DeviceDescription> EnumerateDevices();
    }
}
