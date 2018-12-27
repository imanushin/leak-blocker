using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Resources;

namespace LeakBlocker.Libraries.Common.Entities.Settings.TemporaryAccess
{
    /// <summary>
    /// Временная разблокировка устройства.
    /// Опциональный параметр: пользователи, для которых разблокируется устройство
    /// </summary>
    [DataContract(IsReference = true)]
    [Serializable]
    public sealed class DeviceTemporaryAccessCondition : BaseTemporaryAccessCondition
    {
        /// <summary>
        /// Конструктор. 
        /// </summary>
        public DeviceTemporaryAccessCondition(DeviceDescription device, Time endTime, bool readOnly)
            : base(readOnly, endTime)
        {
            Check.ObjectIsNotNull(device, "device");

            Device = device;
        }

        /// <summary>
        /// Устройство, которое временно помещается в белый список
        /// </summary>
        [DataMember]
        [Required]
        public DeviceDescription Device
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает все вложенные объекты, которые имеют смысл для вычисления хеш-кода и метода Equals
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return Device;

            foreach (object innerObject in base.GetInnerObjects())
            {
                yield return innerObject;
            }

        }

        /// <summary>
        /// Возвращает строку с User-Friendly описание Condition`а.
        /// Результат может быть использован в UI
        /// </summary>
        protected override string GetString()
        {
            string deviceAccess = TemporaryAccessStrings.AccessToDevice.Combine(Device);

            return string.Join(" ", deviceAccess, CreateTimeAndWriteRestrictionsString());
        }
    }
}
