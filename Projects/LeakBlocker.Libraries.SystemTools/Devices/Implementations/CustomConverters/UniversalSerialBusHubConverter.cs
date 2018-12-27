using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;

namespace LeakBlocker.Libraries.SystemTools.Devices.Implementations.CustomConverters
{
    internal sealed class UniversalSerialBusDriveConverter : DeviceConverter
    {
        protected override bool CheckDevice(ISystemDevice systemDevice)
        {
            return (systemDevice.DeviceType == SystemDeviceType.UniversalSerialBus) && (systemDevice.Service.StartsWith("USBHUB", StringComparison.OrdinalIgnoreCase));
        }
        
        protected override ReadOnlyDictionary<ISystemDevice, DeviceDescription> Convert(ISystemDevice systemDevice)
        {
            ReadOnlyDictionary<ISystemDevice, DeviceDescription> converted = base.Convert(systemDevice);
            var result = new Dictionary<ISystemDevice, DeviceDescription>();

            // use DeviceCategory.Controller instead ignoring and show such devices to user.
            foreach (KeyValuePair<ISystemDevice, DeviceDescription> currentItem in converted)
            {
                if (CheckDevice(currentItem.Key))
                    continue;
                result.Add(currentItem.Key, currentItem.Value);
            }

            return result.ToReadOnlyDictionary();
        }
    }
}
