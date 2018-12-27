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
    /// Временная разблокировка для пользователя
    /// </summary>
    [DataContract(IsReference = true)]
    [Serializable]
    public sealed class UserTemporaryAccessCondition : BaseTemporaryAccessCondition
    {
        /// <summary>
        /// Создает RO объект, проверяет все параметры
        /// </summary>
        public UserTemporaryAccessCondition(BaseUserAccount user, Time endTime, bool readOnly)
            : base(readOnly, endTime)
        {
            Check.ObjectIsNotNull(user, "user");

            User = user;
        }

        /// <summary>
        /// Пользователь, который временно добавляется в белый список
        /// </summary>
        [DataMember]
        [Required]
        public BaseUserAccount User
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает все вложенные объекты, которые имеют смысл для вычисления хеш-кода и метода Equals
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return User;

            foreach (object innerObject in base.GetInnerObjects())
                yield return innerObject;
        }

        /// <summary>
        /// Возвращает строку с User-Friendly описание Condition`а.
        /// Результат может быть использован в UI
        /// </summary>
        protected override string GetString()
        {
            string userAccess = TemporaryAccessStrings.AccessToUser.Combine(User);

            return string.Join(" ", userAccess, CreateTimeAndWriteRestrictionsString());
        }
    }
}
