using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Resources;

namespace LeakBlocker.Libraries.Common.Entities.Audit
{
    /// <summary>
    /// Важность события
    /// </summary>
    [EnumerationDescriptionProvider(typeof(AuditItemSeverityTypeStrings))]
    public enum AuditItemSeverityType
    {
        /// <summary>
        /// Incorrect value.
        /// </summary>
        [ForbiddenToUse]
        [Obsolete("Incorrect value", true)]
        [UsedImplicitly]
        None = 0,

        /// <summary>
        /// Информация
        /// </summary>
        Information,

        /// <summary>
        /// Предупреждение
        /// </summary>
        Warning,

        /// <summary>
        /// Ошибка
        /// </summary>
        Error
    }
}
