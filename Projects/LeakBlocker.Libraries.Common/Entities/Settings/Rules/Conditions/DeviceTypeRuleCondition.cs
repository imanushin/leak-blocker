using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common.Entities.Settings.Rules.Conditions
{
    /// <summary>
    /// Позволяет блокировать или разблокировать устройства определенного типа
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    public sealed class DeviceTypeRuleCondition : BaseRuleCondition
    {
        /// <summary>
        /// Тип устройста, который следует блокировать
        /// </summary>
        [DataMember]
        public DeviceCategory DeviceType
        {
            get;
            private set;
        }

        /// <summary>
        /// Создает условие для блокировки.
        /// </summary>
        public DeviceTypeRuleCondition(DeviceCategory deviceType, bool not)
            : base(not)
        {
            Check.EnumerationValueIsDefined(deviceType, "deviceType");

            DeviceType = deviceType;
        }

        /// <summary>
        /// Выдает все зависимые объекты
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return DeviceType;

            foreach (object innerObject in base.GetInnerObjects())
            {
                yield return innerObject;
            }
        }
    }
}
