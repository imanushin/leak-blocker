using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities;

namespace LeakBlocker.Libraries.Storage.Entities
{
    partial class DbDeviceDescription
    {
        private DeviceDescription ForceGetDeviceDescription()
        {
            return new DeviceDescription(FriendlyName, Identifier, Category);
        }
    }
}
