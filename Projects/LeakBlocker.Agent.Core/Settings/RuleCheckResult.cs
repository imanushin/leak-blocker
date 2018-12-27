using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Entities.Settings;

namespace LeakBlocker.Agent.Core.Settings
{
    /// <summary>
    /// Результат проверки правил
    /// </summary>
    internal sealed class RuleCheckResult : ReadOnlyMatrix<BaseUserAccount, DeviceDescription, CommonActionData>
    {
        /// <summary>
        /// Настройки аудита
        /// </summary>
        public DeviceAccessMap AccessMap
        {
            get;
            private set;
        }

        /// <summary>
        /// Настройки блокировки
        /// </summary>
        public AuditMap AuditMap
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Конструктор. Пробегает по коллекциям и создает полностью инициализированный объект
        /// </summary>
        public RuleCheckResult(
            IReadOnlyCollection<BaseUserAccount> users,
            IReadOnlyCollection<DeviceDescription> devices,
            Func<BaseUserAccount, DeviceDescription, CommonActionData> creator)
            : base(users, devices, creator)
        {
            Check.ObjectIsNotNull(users, "users");
            Check.ObjectIsNotNull(devices, "devices");
            Check.ObjectIsNotNull(creator, "creator");

            AccessMap = new DeviceAccessMap(users, devices, (user,device)=>this[user,device].Access);
            AuditMap = new AuditMap(users, devices, (user, device) => this[user, device].Audit);
        }
    }
}
