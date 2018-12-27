using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common.Entities.Security
{
    /// <summary>
    /// Base class for all security principals. 
    /// Class is immutable, guaranteed to be properly initialized.
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    [KnownType(typeof(BaseDomainAccount))]
    [KnownType(typeof(BaseUserAccount))]
    [KnownType(typeof(BaseGroupAccount))]
    public abstract class Account : BaseEntity, IScopeObject
    {
        /// <summary>
        /// Полное имя аккаунта
        /// </summary>
        [DataMember]
        [Required]
        [NotNull]
        public string FullName
        {
            get;
            private set;
        }

        /// <summary>
        /// Сокращенное имя аккаунта (например, без доменных приставок)
        /// </summary>
        [DataMember]
        [Required]
        [NotNull]
        public string ShortName
        {
            get;
            private set;
        }

        /// <summary>
        /// Account name.
        /// </summary>
        [DataMember]
        [Required]
        [NotNull]
        public string CanonicalName
        {
            get;
            private set;
        }

        /// <summary>
        /// Account security identifier.
        /// </summary>
        [DataMember]
        [Required]
        [NotNull]
        public AccountSecurityIdentifier Identifier
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes properties of the current instance.
        /// </summary>
        /// <param name="shortName"></param>
        /// <param name="fullName"></param>
        /// <param name="identifier">Account security identifier.</param>
        /// <param name="canonicalName"> </param>
        internal Account(string shortName, string fullName, AccountSecurityIdentifier identifier, string canonicalName)
        {
            Check.StringIsMeaningful(shortName, "shortName");
            Check.StringIsMeaningful(fullName, "fullName");
            Check.ObjectIsNotNull(identifier, "identifier");
            Check.ObjectIsNotNull(canonicalName, "canonicalName");

            FullName = fullName;
            ShortName = shortName;
            Identifier = identifier;
            CanonicalName = canonicalName;
        }

        /// <summary>
        /// Выдает полное имя аккаунта
        /// </summary>
        protected sealed override string GetString()
        {
            return FullName;
        }

        #region Equality

        /// <summary>
        /// Возвращает все вложенные объекты, которые имеют смысл для вычисления хеш-кода и метода Equals
        /// </summary>
        protected sealed override IEnumerable<object> GetInnerObjects()
        {
            yield return Identifier;
        }

        #endregion

        /// <summary>
        /// Formats the full name.
        /// </summary>
        /// <param name="shortName">Short name.</param>
        /// <param name="parent">Parent domain.</param>
        /// <returns>Full name.</returns>
        protected static string CombineFullName(string shortName, BaseDomainAccount parent)
        {
            Check.StringIsMeaningful(shortName, "shortName");
            Check.ObjectIsNotNull(parent, "parent");

            const string format = "{0}\\{1}";

            return format.Combine(parent.FullName, shortName);
        }
    }
}
