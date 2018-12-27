using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Audit;

namespace LeakBlocker.Libraries.Common.Entities
{
    /// <summary>
    /// Состояние блокировки устройства
    /// </summary>
    public enum DeviceAccessType
    {
        /// <summary>
        /// Не надо его использовать
        /// </summary>
        [ForbiddenToUse]
        [Obsolete("Default value", true)]
        [UsedImplicitly]
        None = 0,

        /// <summary>
        /// Полностью заблокировано
        /// </summary>
        [LinkedEnum(AuditItemType.DeviceAccessBlocked)]
        Blocked,

        /// <summary>
        /// Имеет смысл только для устройств с файловой системой: запись заблокирована, чтение разрешено
        /// </summary>
        [LinkedEnum(AuditItemType.DeviceAccessReadOnly)]
        ReadOnly,

        /// <summary>
        /// Доступ к устройству полностью разрешен
        /// </summary>
        [LinkedEnum(AuditItemType.DeviceAccessAllowed)]
        Allowed,

        /// <summary>
        /// У пользователя временно есть полный доступ к устройству
        /// </summary>
        [LinkedEnum(AuditItemType.DeviceAccessTemporarilyAllowed)]
        TemporarilyAllowed,

        /// <summary>
        /// У пользователя временно есть полный доступ к устройству
        /// </summary>
        [LinkedEnum(AuditItemType.DeviceAccessTemporarilyReadOnly)]
        TemporarilyAllowedReadOnly,
        
        /// <summary>
        /// Агент не лицензирован
        /// </summary>
        [LinkedEnum(AuditItemType.DeviceAccessAllowedNotLicensed)]
        AllowedNotLicensed
    }
}