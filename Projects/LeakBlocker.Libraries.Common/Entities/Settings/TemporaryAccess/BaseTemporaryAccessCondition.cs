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
    /// Информация о временном доступе
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    [KnownType(typeof(ComputerTemporaryAccessCondition))]
    [KnownType(typeof(UserTemporaryAccessCondition))]
    [KnownType(typeof(DeviceTemporaryAccessCondition))]
    public abstract class BaseTemporaryAccessCondition : BaseEntity
    {
        /// <summary>
        /// Конструктор, реализует общие поля для объектов временного доступа
        /// </summary>
        internal BaseTemporaryAccessCondition(bool readOnly, Time endTime)
        {
            Check.ObjectIsNotNull(endTime, "endTime");

            EndTime = endTime;
            ReadOnlyAccess = readOnly;
        }

        /// <summary>
        /// Время, до которого предоставляется временный доступ
        /// </summary>
        [DataMember]
        [Required]
        public Time EndTime
        {
            get;
            private set;
        }

        /// <summary>
        /// Следует ли заблокировать запись. Значение true имеет смысл только при конфигурациях с запрещенной записью
        /// </summary>
        [Required]
        [DataMember]
        public bool ReadOnlyAccess
        {
            get;
            private set;
        }

        #region Equality

        /// <summary>
        /// Возвращает все вложенные объекты, которые имеют смысл для вычисления хеш-кода и метода Equals
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return ReadOnlyAccess;
            yield return EndTime;
        }

        #endregion

        #region String Generation

        /// <summary>
        /// Выдает текст с описанием того, насколько выдан временный доступ и есть ли возможность записи
        /// </summary>
        protected string CreateTimeAndWriteRestrictionsString()
        {
            string writeAcces = ReadOnlyAccess ? TemporaryAccessStrings.WriteAccessWillBeDenied : TemporaryAccessStrings.WriteAccessWillBeGranted;

            string timeRestrictions = IsForeverAccess ? 
                TemporaryAccessStrings.OperationsWillBeAllowedUntilCancelation : 
                TemporaryAccessStrings.OperationsWillBeAllowedUntilTime.Combine(EndTime);

            return string.Join(" ", writeAcces, timeRestrictions);
        }

        private bool IsForeverAccess
        {
            get
            {
                return DateTime.Now.AddYears(10) < EndTime.ToUtcDateTime();
            }
        }

        #endregion
    } 
}
