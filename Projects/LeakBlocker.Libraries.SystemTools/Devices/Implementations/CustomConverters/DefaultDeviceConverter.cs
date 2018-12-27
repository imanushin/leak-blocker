using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.SystemTools.Devices.Implementations.CustomConverters
{
    internal sealed class DefaultDeviceConverter : DeviceConverter
    {
        protected override bool CheckDevice(ISystemDevice systemDevice)
        {
            return true;
        }
    }
}
