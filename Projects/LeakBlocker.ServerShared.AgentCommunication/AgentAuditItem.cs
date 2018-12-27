using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Audit;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.ServerShared.AgentCommunication
{
    /// <summary>
    /// Event description.
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    public sealed class AgentAuditItem : BaseReadOnlyObject
    {
        /// <summary>
        /// Event type.
        /// </summary>
        [DataMember]
        public AuditItemType EventType
        {
            get;
            private set;
        }

        /// <summary>
        /// Time when the event occurred.
        /// </summary>
        [DataMember]
        [NotNull]
        public Time Time
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
        /// Any number that is associated with the event.
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
        public AgentAuditItem(AuditItemType type, Time time, BaseUserAccount user = null, string textData = null, string additionalTextData = null, DeviceDescription device = null, int configuration = 0, int number = 0)
        {
            Check.EnumerationValueIsDefined(type, "type");
            Check.ObjectIsNotNull(time, "time");

            EventType = type;
            TextData = textData;
            Time = time;
            User = user;
            Device = device;
            Configuration = configuration;
            AdditionalTextData = additionalTextData;
            Number = number;
        }

        #region Equality

        /// <summary>
        /// Возвращает все вложенные объекты, которые имеют смысл для вычисления хеш-кода и метода Equals
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return EventType;
            yield return Time;
            yield return User;
            yield return TextData;
            yield return Device;
            yield return Configuration;
            yield return AdditionalTextData;
            yield return Number;
        }

        #endregion
    }
}
