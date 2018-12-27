using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common.Entities.Security
{
    /// <summary>
    /// Логин и пароль для определенного аккаунта
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    [KnownType(typeof(DomainAccount))]
    [KnownType(typeof(DomainComputerAccount))]
    public sealed class Credentials : BaseEntity
    {
        /// <summary>
        /// Список доменов, для доступа к которым требуется отдельно заданный логин и пароль
        /// </summary>
        [DataMember]
        [Required]
        [NotNull]
        public BaseDomainAccount Domain
        {
            get;
            private set;
        }

        /// <summary>
        /// Пользователь (логин). Должен быть администратором в на компьютерах, где будет оперировать
        /// </summary>
        [DataMember]
        [Required]
        [NotNull]
        public string User
        {
            get;
            private set;
        }

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        [DataMember]
        [Required]
        [NotNull]
        public string Password
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates an instance of the Credentials class.
        /// </summary>
        /// <param name="domain">Target domain.</param>
        /// <param name="user">Username</param>
        /// <param name="password">Password.</param>
        public Credentials(BaseDomainAccount domain, string user, string password)
        {
            Check.ObjectIsNotNull(domain, "domain");
            Check.StringIsMeaningful(user, "user");
            Check.StringIsMeaningful(password, "password");

            User = user;
            Password = password;
            Domain = domain;
        }

        #region Equality

        /// <summary>
        /// Возвращает все вложенные объекты, которые имеют смысл для вычисления хеш-кода и метода Equals
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return User;
            yield return Domain;
            yield return Password;
        }

        #endregion

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        protected override string GetString()
        {
            return "Credentials: domain - {0}; user - {1}".Combine(Domain, User);
        }
    }
}
