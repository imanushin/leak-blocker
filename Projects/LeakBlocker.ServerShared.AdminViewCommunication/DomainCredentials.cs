using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.ServerShared.AdminViewCommunication
{
    /// <summary>
    /// Класс для передачи логина/пароля пользователя
    /// </summary>
    [DataContract(IsReference = true)]
    [Serializable]
    public sealed class DomainCredentials : BaseReadOnlyObject
    {
        /// <summary>
        /// Конструктор: копирует данные так, чтобы нельзя было изменить извне
        /// </summary>
        public DomainCredentials(string fullUserName, string password, string domain)
        {
            Check.StringIsMeaningful(fullUserName, "fullUserName");
            Check.StringIsMeaningful(password, "password");
            Check.StringIsMeaningful(domain, "domain");

            FullUserName = fullUserName;
            Password = password;
            Domain = domain;
        }

        /// <summary>
        /// Домен, для которого требуется проверить Credentials
        /// </summary>
        [DataMember]
        public string Domain
        {
            get;
            private set;
        }

        /// <summary>
        /// Полное имя пользователя: виде domain\userName, userName@domain и пр.
        /// </summary>
        [DataMember]
        public string FullUserName
        {
            get;
            private set;
        }

        /// <summary>
        /// Пароль
        /// </summary>
        [DataMember]
        public string Password
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает все вложенные объекты, которые имеют смысл для вычисления хеш-кода и метода Equals
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return Domain.ToUpperInvariant();
            yield return FullUserName.ToUpperInvariant();
            yield return Password;
        }
    }
}
