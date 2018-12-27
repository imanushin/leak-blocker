using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities;

namespace LeakBlocker.ServerShared.AdminViewCommunication
{
    /// <summary>
    /// Данные о компьютере, который работает с сервисом
    /// </summary>
    [DataContract(IsReference = true)]
    public sealed class ManagedComputerData : BaseReadOnlyObject
    {
        /// <summary>
        /// Инициализирует Read-Only объект
        /// </summary>
        public ManagedComputerData(ManagedComputerStatus status, DeviceAccessMap lockMap)
        {
            Check.ObjectIsNotNull(lockMap, "lockMap");
            Check.EnumerationValueIsDefined(status, "status");

            Status = status;
            LastLockMap = lockMap;
        }

        /// <summary>
        /// Статус компьютера
        /// </summary>
        [DataMember]
        public ManagedComputerStatus Status
        {
            get;
            private set;
        }

        /// <summary>
        /// Последние данные о блокировки (если комп выключен, то структура необязательно будет пустой)
        /// </summary>
        [DataMember]
        public DeviceAccessMap LastLockMap
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает все вложенные объекты, которые имеют смысл для вычисления хеш-кода и метода Equals
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return LastLockMap;
            yield return Status;
        }

        /// <summary>
        /// Выводит структуру в текстовое представление для отладки
        /// </summary>
        protected override string GetString()
        {
            return "Status: {0}; Map:{1}".Combine(Status, LastLockMap);
        }

        /// <summary>
        /// Создает такой же объект, но с другим статусом
        /// </summary>
        public ManagedComputerData CreateFromCurrent(ManagedComputerStatus newStatus)
        {
            Check.EnumerationValueIsDefined(newStatus);

            return new ManagedComputerData(newStatus, LastLockMap);
        }
    }
}
