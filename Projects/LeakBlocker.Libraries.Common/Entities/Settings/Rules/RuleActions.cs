using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common.Entities.Settings.Rules
{
    /// <summary>
    /// Device audit options.
    /// </summary>
    public enum AuditActionType
    {
        /// <summary>
        /// Incorrect value.
        /// </summary>
        [Obsolete("Don't use", true)]
        [ForbiddenToUse]
        None = 0,

        /// <summary>
        /// Пропустить действие (то есть взять значение у предыдущей Rule по приоритету)
        /// </summary>
        Skip = 1,

        /// <summary>
        /// Нужен только для Excluded Computers.
        /// </summary>
        DisableAudit = 2,

        /// <summary>
        /// Audit only basic device events.
        /// </summary>
        Device = 3,

        /// <summary>
        /// Audit basic device events and file events for storage devices.
        /// </summary>
        DeviceAndFiles = 4
    }

    /// <summary>
    /// Device blocking optiions.
    /// </summary>
    public enum BlockActionType
    {
        /// <summary>
        /// Incorrect value.
        /// </summary>
        [ForbiddenToUse]
        [Obsolete("Don't use", true)]
        None = 0,

        /// <summary>
        /// Пропустить действие
        /// </summary>
        Skip = 1,

        /// <summary>
        /// Не блокировать
        /// </summary>
        Unblock = 2,

        /// <summary>
        /// Blocks the entire device.
        /// </summary>
        Complete = 3,

        /// <summary>
        /// For storage devices block only write operations. Non-storage devices wil be blocked completely.
        /// </summary>
        ReadOnly = 4
    }
}
