using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.Libraries.Common.Entities.Audit
{
    /// <summary>
    /// Event description.
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    [KnownType(typeof(LocalUserAccount))]
    [KnownType(typeof(DomainUserAccount))]
    public sealed class AuditItem : BaseEntity
    {
        /// <summary>
        /// Event type.
        /// </summary>
        [DataMember]
        [Required]
        public AuditItemType EventType
        {
            get;
            private set;
        }

        /// <summary>
        /// Time when the event occurred.
        /// </summary>
        [DataMember]
        [Required]
        [NotNull]
        public Time Time
        {
            get;
            private set;
        }

        /// <summary>
        /// Computer on which the event occurred.
        /// </summary>
        [DataMember]
        [NotNull]
        [Required]
        public BaseComputerAccount Computer
        {
            get;
            private set;
        }

        /// <summary>
        /// User that caused the event.
        /// </summary>
        [DataMember]
        [CanBeNull]
        public BaseUserAccount User
        {
            get;
            private set;
        }

        /// <summary>
        /// Text data associated with the event. Can be null.
        /// </summary>
        [DataMember]
        [CanBeNull]
        public string TextData
        {
            get;
            private set;
        }

        /// <summary>
        /// Text data associated with the event. Can be null.
        /// </summary>
        [DataMember]
        [CanBeNull]
        public string AdditionalTextData
        {
            get;
            private set;
        }

        /// <summary>
        /// Device that is associated with the event. Can be null.
        /// </summary>
        [DataMember]
        [CanBeNull]
        public DeviceDescription Device
        {
            get;
            private set;
        }

        /// <summary>
        /// Current configuration version. Can be 0.
        /// </summary>
        [DataMember]
        public int Configuration
        {
            get;
            private set;
        }

        /// <summary>
        /// Any numeric value associated with the event.
        /// </summary>
        [DataMember]
        public int Number
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates an instance of an AuditItem class.
        /// </summary>
        public AuditItem(
            AuditItemType type,
            Time time,
            BaseComputerAccount computer,
            [CanBeNull] BaseUserAccount user = null,
            [CanBeNull] string textData = null,
            [CanBeNull] string additionalTextData = null,
            [CanBeNull] DeviceDescription device = null,
            int configuration = 0,
            int number = 0)
        {
            Check.EnumerationValueIsDefined(type, "type");
            Check.ObjectIsNotNull(computer, "computer");
            Check.ObjectIsNotNull(time, "time");

            EventType = type;
            TextData = textData;
            AdditionalTextData = additionalTextData;
            Computer = computer;
            Time = time;
            User = user;
            Device = device;
            Configuration = configuration;
            Number = number;

            Check.ObjectIsNotNull(ToString());//Тестируем, что шаблон может быть создан по заданному типу и аргументам
        }

        #region Equality

        /// <summary>
        /// Возвращает все вложенные объекты, которые имеют смысл для вычисления хеш-кода и метода Equals
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return EventType;
            yield return Time;
            yield return Computer;
            yield return User;
            yield return TextData;
            yield return Device;
            yield return Configuration;
            yield return Number;
        }

        #endregion

        /// <summary>
        /// Сравнивает два объекта <see cref="AuditItem"/>
        /// </summary>
        protected override int CustomCompare(object target)
        {
            return Time.CompareTo(((AuditItem)target).Time);
        }

        /// <summary>
        /// Составляет User Friendly строку, описывающую данный <see cref="AuditItem"/>.
        /// Lazy-инициализация: строка составляется каждый раз в конструкторе (чтобы проверить параметры), в ToString происходит её выдача. После передачи через WCF строка пересоздается в методе ToString
        /// </summary>
        /// <returns></returns>
        protected override string GetString()
        {
            string template = EventType.GetValueDescription();

            return StringNamedFormatter.ApplyTemplate(template, new Dictionary<string, GetString>
            {
// ReSharper disable PossibleNullReferenceException
                { "Computer", ()=> Computer.ToString() },
                { "User", ()=> User.ToString() },
                { "Device", ()=> Device.ToString() },
                { "TextData", ()=> TextData },
                { "AdditionalTextData", ()=> AdditionalTextData },
                { "Number", ()=> Number.ToString(CultureInfo.InvariantCulture) },
// ReSharper restore PossibleNullReferenceException
            }.ToReadOnlyDictionary());
        }
    }
}
