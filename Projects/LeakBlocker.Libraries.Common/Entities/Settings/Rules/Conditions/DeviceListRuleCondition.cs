using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;

namespace LeakBlocker.Libraries.Common.Entities.Settings.Rules.Conditions
{
    /// <summary>
    /// Condition for checking devices.
    /// Class is immutable, guaranteed to be properly initialized.
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    public sealed class DeviceListRuleCondition : BaseRuleCondition
    {
        /// <summary>
        /// List of devices.
        /// </summary>
        [DataMember]
        [Required]
        [NotNull]
        public ReadOnlySet<DeviceDescription> Devices
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates an instance of DeviceListRuleCondition class.
        /// </summary>
        /// <param name="not">Logical inversion. Specify true if result of checking the condition should be inverted.</param>
        /// <param name="devices">List of devices.</param>
        public DeviceListRuleCondition(bool not, IReadOnlyCollection<DeviceDescription> devices)
            : base(not)
        {
            Check.CollectionHasOnlyMeaningfulData(devices, "devices");

            Devices = devices.ToReadOnlySet();
        }

        #region Equality

        /// <summary>
        /// Возвращает все вложенные объекты, которые имеют смысл для вычисления хеш-кода и метода Equals
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return Devices;

            foreach (object innerObject in base.GetInnerObjects())
            {
                yield return innerObject;
            }

        }
        
        #endregion
    }
}
