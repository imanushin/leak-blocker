using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Storage.InternalTools;

namespace LeakBlocker.Libraries.Storage
{
    internal sealed class DevicesManager : IDevicesManager
    {
        public ReadOnlySet<DeviceDescription> GetAllDevices()
        {
            using (IDatabaseModel model = StorageObjects.CreateDatabaseModel())
            {
                return model.DeviceDescriptionSet.ToList().Select(device=>device.GetDeviceDescription()).ToReadOnlySet();
            }
        }
    }
}
