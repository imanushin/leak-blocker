using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Resources;

namespace LeakBlocker.Libraries.Common.Entities.Settings.TemporaryAccess
{
    /// <summary>
    /// Врменный доступ на компьютер.
    /// Опционально можно добавить пользователей и устройства
    /// </summary>
    [DataContract(IsReference = true)]
    [Serializable]
    public sealed class ComputerTemporaryAccessCondition : BaseTemporaryAccessCondition
    {
        /// <summary>
        /// Конструктор Read Only объекта
        /// </summary>
        public ComputerTemporaryAccessCondition(
            BaseComputerAccount computer,
            Time endTime,
            bool readOnly)
            : base(readOnly, endTime)
        {
            Check.ObjectIsNotNull(computer, "computer");

            Computer = computer;
        }

        /// <summary>
        /// Компьютер, на котором будет предоставлен доступ.
        /// </summary>
        [DataMember]
        [Required]
        public BaseComputerAccount Computer
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает все вложенные объекты, которые имеют смысл для вычисления хеш-кода и метода Equals
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return Computer;

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
            string computerAccess = TemporaryAccessStrings.AccessToComputer.Combine(Computer);

            return string.Join(" ", computerAccess, CreateTimeAndWriteRestrictionsString());
        }
    }
}
