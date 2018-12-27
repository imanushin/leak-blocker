using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.Libraries.Common.Entities
{
    /// <summary>
    /// Таблица: для каждого пользователя и устройста их отношение блокировки.
    /// Т. е. каждый пользователь имеет данные о каждом устройстве
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix"), DataContract(IsReference = true)]
    public sealed class DeviceAccessMap : ReadOnlyMatrix<BaseUserAccount, DeviceDescription, DeviceAccessType>
    {
        private readonly static DeviceAccessMap emptyMap = new DeviceAccessMap(
            ReadOnlySet<BaseUserAccount>.Empty,
            ReadOnlySet<DeviceDescription>.Empty,
            (user,device)=>default(DeviceAccessType));

        /// <summary>
        /// Пустой список пользователей и устройств
        /// </summary>
        public static DeviceAccessMap Empty
        {
            get
            {
                return emptyMap;
            }
        }

        /// <summary>
        /// Создает полностью инициализированный объект
        /// </summary>
        public DeviceAccessMap(
            IReadOnlyCollection<BaseUserAccount> keys1,
            IReadOnlyCollection<DeviceDescription> keys2, 
            Func<BaseUserAccount, DeviceDescription, DeviceAccessType> constructor)
            : base(keys1, keys2, constructor)
        {
        }

    }
}
