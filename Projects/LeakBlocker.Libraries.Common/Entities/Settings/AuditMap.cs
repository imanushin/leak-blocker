using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules;

namespace LeakBlocker.Libraries.Common.Entities.Settings
{
    /// <summary>
    /// Настройки аудита для каждого устройства и пользователя
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix"), DataContract(IsReference = true)]
    public sealed class AuditMap : ReadOnlyMatrix<BaseUserAccount, DeviceDescription, AuditActionType>
    {
        /// <summary>
        /// Конструктор. Создает полностью инициализированный объект
        /// </summary>
        public AuditMap(
            IReadOnlyCollection<BaseUserAccount> users,
            IReadOnlyCollection<DeviceDescription> devices,
            Func<BaseUserAccount, DeviceDescription, AuditActionType> constructor)
            : base(users, devices, constructor)
        {
        }
    }
}
