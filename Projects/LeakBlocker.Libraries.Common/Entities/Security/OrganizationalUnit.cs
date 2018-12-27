using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common.Entities.Security
{
    /// <summary>
    /// Represents an active directory organizational unit. 
    /// Class is immutable, guaranteed to be properly initialized.
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    public sealed class OrganizationalUnit : BaseEntity, IDomainMember, IScopeObject
    {
        /// <summary>
        /// Full canonical name of the organizational unit.
        /// </summary>
        [DataMember]
        [Required]
        public string CanonicalName
        {
            get;
            private set;
        }

        /// <summary>
        /// Active directory domain that contains this organizational unit.
        /// </summary>
        [DataMember]
        [Required]
        public DomainAccount Parent
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates an OrganizationalUnit instance.
        /// </summary>
        /// <param name="name">Displayed name of the organizational unit.</param>
        /// <param name="canonicalName">Full canonical name of the organizational unit.</param>
        /// <param name="parent">Active directory domain that contains the organizational unit.</param>
        public OrganizationalUnit(string name, string canonicalName, DomainAccount parent)
        {
            Check.StringIsMeaningful(name, "name");
            Check.StringIsMeaningful(canonicalName, "canonicalName");
            Check.ObjectIsNotNull(parent, "parent");

            CanonicalName = canonicalName;
            ShortName = name;
            Parent = parent;
        }

        #region Equality

        /// <summary>
        /// Возвращает все вложенные объекты, которые имеют смысл для вычисления хеш-кода и метода Equals
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return CanonicalName.ToUpperInvariant();
            yield return Parent;
        }
        
        #endregion

        #region IBaseDomainMember

        BaseDomainAccount IBaseDomainMember.Parent
        {
            get
            {
                return Parent;
            }
        }

        #endregion

        /// <summary>
        /// Для метода ToString. Объект сам не занимается кешированием
        /// </summary>
        protected override string GetString()
        {
            return FullName;
        }

        /// <summary>
        /// Full canonical name.
        /// </summary>
        public string FullName
        {
            get
            {
                return CanonicalName;
            }
        }

        /// <summary>
        /// Short name.
        /// </summary>
        [DataMember]
        [Required]
        public string ShortName
        {
            get;
            private set;
        }
    }
}
