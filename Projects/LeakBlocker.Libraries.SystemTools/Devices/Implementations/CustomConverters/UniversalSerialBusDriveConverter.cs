using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;

namespace LeakBlocker.Libraries.SystemTools.Devices.Implementations.CustomConverters
{
    internal sealed class UniversalSerialBusHubConverter : DeviceConverter
    {
        protected override bool CheckDevice(ISystemDevice systemDevice)
        {
            return (systemDevice.DeviceType == SystemDeviceType.UniversalSerialBus) && (systemDevice.Service.Equals("USBSTOR", StringComparison.OrdinalIgnoreCase));
        }

        protected override ReadOnlyDictionary<ISystemDevice, DeviceDescription> Convert(ISystemDevice systemDevice)
        {
            return base.Convert(systemDevice).Where(item => item.Value.Category == DeviceCategory.Storage).ToReadOnlyDictionary(item => item.Key, item => item.Value);
        }
    }
}
