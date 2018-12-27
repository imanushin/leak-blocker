using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities;

namespace LeakBlocker.Libraries.Common.License
{
    /// <summary>
    /// Запрос на лицензию
    /// </summary>
    [DataContract(IsReference = true)]
    public sealed class LicenseRequestData : BaseReadOnlyObject
    {
        /// <summary>
        /// Создает объект. Имя компании может быть пустым
        /// </summary>
        public LicenseRequestData(UserContactInformation userContact, int count)
        {
            Check.ObjectIsNotNull(userContact, "userContact");
            Check.IntegerIsGreaterThanZero(count, "count");

            UserContact = userContact;
            Count = count;
        }

        /// <summary>
        /// Имя компании. Может равняться <see cref="string.Empty"/>
        /// </summary>
        [DataMember]
        public UserContactInformation UserContact
        {
            get;
            private set;
        }

        /// <summary>
        /// Количество лицензий
        /// </summary>
        [DataMember]
        public int Count
        {
            get;
            private set;
        }

        /// <summary>
        /// Выдает все вложенные объекты
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return UserContact;
            yield return Count;
        }
    }
}
