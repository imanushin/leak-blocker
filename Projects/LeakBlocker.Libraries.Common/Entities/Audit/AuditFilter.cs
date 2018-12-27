using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.Libraries.Common.Entities.Audit
{
    /// <summary>
    /// Фильтр для отображения части аудита
    /// </summary>
    [DataContract(IsReference = true)]
    [Serializable]
    public sealed class AuditFilter : BaseReadOnlyObject
    {
        /// <summary>
        /// Создает Read-Only объект
        /// </summary>
        public AuditFilter(
            string name,
            IReadOnlyCollection<BaseComputerAccount> computers,
            IReadOnlyCollection<BaseUserAccount> users,
            IReadOnlyCollection<DeviceDescription> devices,
            Time startTime,
            Time endTime,
            bool onlyError,
            IReadOnlyCollection<AuditItemGroupType> types)
        {
            Check.StringIsMeaningful(name, "name");
            Check.ObjectIsNotNull(computers, "computers");
            Check.ObjectIsNotNull(users, "users");
            Check.ObjectIsNotNull(devices, "devices");
            Check.ObjectIsNotNull(startTime, "startTime");
            Check.ObjectIsNotNull(endTime, "endTime");
            Check.ObjectIsNotNull(onlyError, "onlyError");
            Check.ObjectIsNotNull(types, "types");

            Name = name;
            Computers = computers.ToReadOnlySet();
            Users = users.ToReadOnlySet();
            Devices = devices.ToReadOnlySet();
            StartTime = startTime;
            EndTime = endTime;
            OnlyError = onlyError;
            Types = types.ToReadOnlySet();
        }

        /// <summary>
        /// Имя фильтра
        /// </summary>
        [DataMember]
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Компьютеры, которые могут быть использованы в фильтре. Если нет ни одного, то подходит любой объект
        /// </summary>
        [DataMember]
        public ReadOnlySet<BaseComputerAccount> Computers
        {
            get;
            private set;
        }


        /// <summary>
        /// Пользователи, которые могут быть использованы в фильтре. Если нет ни одного, то подходит любой объект
        /// </summary>
        [DataMember]
        public ReadOnlySet<BaseUserAccount> Users
        {
            get;
            private set;
        }

        /// <summary>
        /// Устройства, которые могут быть использованы в фильтре. Если нет ни одного, то подходит любой объект
        /// </summary>
        [DataMember]
        public ReadOnlySet<DeviceDescription> Devices
        {
            get;
            private set;
        }

        /// <summary>
        /// Если не равно <see cref="Time.Unknown"/>, то задает минимально время для элементов в выборке
        /// </summary>
        [DataMember]
        public Time StartTime
        {
            get;
            private set;
        }


        /// <summary>
        /// Если не равно <see cref="Time.Unknown"/>, то задает максимальное время для элементов в выборке
        /// </summary>
        [DataMember]
        public Time EndTime
        {
            get;
            private set;
        }

        /// <summary>
        /// Если true, то в фильтр пропускаются только сообещения с ошибками
        /// </summary>
        [DataMember]
        public bool OnlyError
        {
            get;
            private set;
        }

        /// <summary>
        /// Фильтр по типам событий
        /// </summary>
        [DataMember]
        public ReadOnlySet<AuditItemGroupType> Types
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает все вложенные объекты, которые имеют смысл для вычисления хеш-кода и метода Equals
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return Name;
            yield return Computers;
            yield return Users;
            yield return Devices;
            yield return StartTime;
            yield return EndTime;
            yield return OnlyError;
            yield return Types;
        }

        /// <summary>
        /// Для метода ToString. Объект сам не занимается кешированием
        /// </summary>
        protected override string GetString()
        {
            return "Audit filter {0}, Computers: {1}, Users: {2}, Devices: {3}, StartTime: {4}, EndTime: {5}, OnlyError: {6}, Types: {7}".Combine(
                Name,
                Computers,
                Users,
                Devices,
                StartTime,
                EndTime,
                OnlyError,
                Types);
        }
    }
}
