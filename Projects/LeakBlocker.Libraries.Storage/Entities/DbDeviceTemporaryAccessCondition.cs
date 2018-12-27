using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Settings.TemporaryAccess;

namespace LeakBlocker.Libraries.Storage.Entities
{
    partial class DbDeviceTemporaryAccessCondition
    {
        private DeviceTemporaryAccessCondition ForceGetDeviceTemporaryAccessCondition()
        {
            return new DeviceTemporaryAccessCondition(
                Device.GetDeviceDescription(),
                EndTime,
                ReadOnlyAccess);
        }
    }
}
