using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Settings.TemporaryAccess;

namespace LeakBlocker.ServerShared.AdminViewCommunication
{
    /// <summary>
    /// Упрощенная конфигурация
    /// </summary>
    [DataContract(IsReference = true)]
    [KnownType(typeof(BaseTemporaryAccessCondition))]
    [Serializable]
    public sealed class SimpleConfiguration : BaseReadOnlyObject
    {
        /// <summary>
        /// Создает неизменяемый объект
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "WhiteList")]
        public SimpleConfiguration(
            bool isBlockEnabled,
            bool isReadOnlyAccessEnabled,
            bool areInputDevicesAllowed,
            bool isFileAuditEnabled,
            IEnumerable<Scope> blockedScopes,
            IEnumerable<Scope> excludedScopes,
            IEnumerable<DeviceDescription> excludedDevices,
            IEnumerable<Scope> usersWhiteList,
            IEnumerable<BaseTemporaryAccessCondition> temporaryAccess)
        {
            Check.ObjectIsNotNull(excludedDevices, "excludedDevices");
            Check.ObjectIsNotNull(usersWhiteList, "usersWhiteList");
            Check.ObjectIsNotNull(blockedScopes, "blockedScopes");
            Check.ObjectIsNotNull(excludedScopes, "excludedScopes");
            Check.ObjectIsNotNull(temporaryAccess, "temporaryAccess");

            ExcludedDevices = excludedDevices.ToReadOnlySet();
            UsersWhiteList = usersWhiteList.ToReadOnlySet();
            IsBlockEnabled = isBlockEnabled;
            IsFileAuditEnabled = isFileAuditEnabled;
            IsReadOnlyAccessEnabled = isReadOnlyAccessEnabled;
            AreInputDevicesAllowed = areInputDevicesAllowed;
            BlockedScopes = blockedScopes.ToReadOnlySet();
            ExcludedScopes = excludedScopes.ToReadOnlySet();
            TemporaryAccess = temporaryAccess.ToReadOnlySet();
        }

        /// <summary>
        /// Преобразовывает текущую конфигурацию в иную, используя небольшие модификации
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "WhiteList")]
        public SimpleConfiguration GetFromCurrent(
            IEnumerable<Scope> blockedScopes = null,
            IEnumerable<Scope> excludedScopes = null,
            IEnumerable<DeviceDescription> excludedDevices = null,
            IEnumerable<Scope> usersWhiteList = null,
            IEnumerable<BaseTemporaryAccessCondition> temporaryAccess = null
            )
        {
            return new SimpleConfiguration(
                IsBlockEnabled,
                IsReadOnlyAccessEnabled,
                AreInputDevicesAllowed,
                IsFileAuditEnabled,
                blockedScopes ?? BlockedScopes,
                excludedScopes ?? ExcludedScopes,
                excludedDevices ?? ExcludedDevices,
                usersWhiteList ?? UsersWhiteList,
                temporaryAccess ?? TemporaryAccess);
        }

        /// <summary>
        /// Включена ли блокировка
        /// </summary>
        [DataMember]
        public bool IsBlockEnabled
        {
            get;
            private set;
        }

        /// <summary>
        /// Включен ли аудит по файлам
        /// </summary>
        [DataMember]
        public bool IsFileAuditEnabled
        {
            get;
            private set;
        }

        /// <summary>
        /// Есть ли доступ на чтение (иначе заблокировано всё устройство)
        /// </summary>
        [DataMember]
        public bool IsReadOnlyAccessEnabled
        {
            get;
            private set;
        }

        /// <summary>
        /// Разрешены ли устройства ввода (клавиатура, мышь и пр.)
        /// </summary>
        [DataMember]
        public bool AreInputDevicesAllowed
        {
            get;
            private set;
        }

        /// <summary>
        /// Список заблокированный Scope
        /// </summary>
        [DataMember]
        public ReadOnlySet<Scope> BlockedScopes
        {
            get;
            private set;
        }

        /// <summary>
        /// Список Scope, исключенных из блокировки
        /// </summary>
        [DataMember]
        public ReadOnlySet<Scope> ExcludedScopes
        {
            get;
            private set;
        }

        /// <summary>
        /// Список устройств, исключенных из блокировки
        /// </summary>
        [DataMember]
        public ReadOnlySet<DeviceDescription> ExcludedDevices
        {
            get;
            private set;
        }

        /// <summary>
        /// Список из временного доступа
        /// </summary>
        [DataMember]
        public ReadOnlySet<BaseTemporaryAccessCondition> TemporaryAccess
        {
            get;
            private set;
        }

        /// <summary>
        /// Белый список пользователей
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "WhiteList"), DataMember]
        public ReadOnlySet<Scope> UsersWhiteList
        {
            get;
            private set;
        }

        /// <summary>
        /// Выдает все внутренние объекты для сравнения и вычисления хеш-кода
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return IsReadOnlyAccessEnabled;
            yield return IsBlockEnabled;
            yield return AreInputDevicesAllowed;
            yield return IsFileAuditEnabled;
            yield return TemporaryAccess;
            yield return ExcludedDevices;
            yield return UsersWhiteList;
            yield return BlockedScopes;
            yield return ExcludedScopes;
        }
    }
}
