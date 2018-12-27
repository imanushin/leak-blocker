using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Resources;

namespace LeakBlocker.Libraries.Common.Entities.Audit
{
    /// <summary>
    /// Обобщенный тип для <see cref="AuditItem"/>
    /// </summary>
    [EnumerationDescriptionProvider(typeof(AuditItemGroupTypeStrings))]
    public enum AuditItemGroupType
    {
        /// <summary>
        /// Не использовать
        /// </summary>
        [Obsolete("Default value", true)]
        [ForbiddenToUse]
        [UsedImplicitly]
        None,

        /// <summary>
        /// Установка/удаление агента или сервера
        /// </summary>
        Installation,

        /// <summary>
        /// Операции с устройстом: блокировка и т. д. Здесь же: операции с файлами
        /// </summary>
        Devices,

        /// <summary>
        /// Низкориоритетные действия
        /// </summary>
        Other,

        /// <summary>
        /// Операции с устройствами, но при условии действующего временного доступа
        /// </summary>
        TemporaryAccess,

        /// <summary>
        /// Системные операции: например, перезагрузка компьютера
        /// </summary>
        System
    }
}
