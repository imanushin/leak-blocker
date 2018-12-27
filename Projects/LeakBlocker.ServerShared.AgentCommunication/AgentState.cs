using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities;

namespace LeakBlocker.ServerShared.AgentCommunication
{
    /// <summary>
    /// Текущая информация об агенте: набор его Audit Item'ов и набор устройств
    /// </summary>
    [DataContract(IsReference = true)]
    public sealed class AgentState : BaseReadOnlyObject
    {
        /// <summary>
        /// Сжатая версия аудита
        /// </summary>
        [DataMember]
        public AuditItemPackage Audit
        {
            get;
            private set;
        }

        /// <summary>
        /// Информация о подключенных устройствах
        /// </summary>
        [DataMember]
        public DeviceAccessMap DeviceAccess
        {
            get;
            private set;
        }

        /// <summary>
        /// Конструктор ReadOnly объекта
        /// </summary>
        public AgentState(AuditItemPackage audit, DeviceAccessMap deviceAccess)
        {
            Check.ObjectIsNotNull(audit, "audit");
            Check.ObjectIsNotNull(deviceAccess, "deviceAccess");

            Audit = audit;
            DeviceAccess = deviceAccess;
        }

        /// <summary>
        /// Все вложенные объекты (используется для сравнения)
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return Audit;
            yield return DeviceAccess;
        }
    }
}
