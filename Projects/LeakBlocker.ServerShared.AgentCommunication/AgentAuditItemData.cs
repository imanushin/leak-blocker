using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Audit;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.ServerShared.AgentCommunication
{
    [Serializable]
    [DataContract(IsReference = true)]
    public sealed class AgentAuditItemData : BaseReadOnlyObject
    {
        [DataMember]
        private readonly AuditItemType eventType;
        [DataMember]
        private readonly BaseUserAccount user;
        [DataMember]
        private string textData;
        [DataMember]
        private string additionalTextData;
        [DataMember]
        private DeviceDescription device;

        public AgentAuditItemData(AgentAuditItem auditItem)
        {
            Check.ObjectIsNotNull(auditItem, "auditItem");

            eventType = auditItem.EventType;
            if (auditItem.TextData != null)
                textData = auditItem.TextData.ToUpperInvariant();
            user = auditItem.User;
            device = auditItem.Device;
            if (auditItem.AdditionalTextData != null)
                additionalTextData = auditItem.AdditionalTextData.ToUpperInvariant();
        }

        #region Equality

        /// <summary>
        /// Возвращает все вложенные объекты, которые имеют смысл для вычисления хеш-кода и метода Equals
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return eventType;
            yield return textData;
            yield return user;
            yield return device;
            yield return additionalTextData;
        }

        #endregion
    }
}
