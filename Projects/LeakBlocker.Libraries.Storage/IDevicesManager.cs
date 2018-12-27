using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;

namespace LeakBlocker.Libraries.Storage
{
    /// <summary>
    /// Работа с устройствами
    /// </summary>
    public interface IDevicesManager
    {
        /// <summary>
        /// Выдает все устройства, когда-либо участвовавшие в аудите или в конфигурации
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]//Выполняются сложные операции
        ReadOnlySet<DeviceDescription> GetAllDevices();
    }
}