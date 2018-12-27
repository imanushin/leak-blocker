using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Network;

namespace LeakBlocker.ServerShared.AdminViewCommunication
{
    /// <summary>
    /// Интерфейс для более легкой работы с устройствами: поиск устройств и т. д.
    /// </summary>
    [NetworkObject]
    public interface IDeviceTools : IDisposable
    {
        /// <summary>
        /// Выдает список всех подключенных устройств к заданному компьютеру.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        ReadOnlySet<DeviceDescription> GetConnectedDevices();
    }
}
