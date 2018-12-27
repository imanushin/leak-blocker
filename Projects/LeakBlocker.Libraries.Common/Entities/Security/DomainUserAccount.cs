using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common.Entities.Security
{
    /// <summary>
    /// Represents an user. 
    /// Class is immutable, guaranteed to be properly initialized.
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    public sealed class DomainUserAccount : BaseUserAccount, IDomainMember
    {
        /// <summary>
        /// Domain to which this account belongs.
        /// </summary>
        [IgnoreDataMember]
        [NotNull]
        public new DomainAccount Parent
        {
            get
            {
                return (DomainAccount)base.Parent;
            }
        }

        /// <summary>
        /// Creates an UserAccount instance.
        /// </summary>
        /// <param name="shortName">User name.</param>
        /// <param name="identifier">User security identifier.</param>
        /// <param name="parent">Domain to which this group belongs.</param>
        /// <param name="canonicalName">Canonical name.</param>
        public DomainUserAccount(string shortName, AccountSecurityIdentifier identifier, DomainAccount parent, string canonicalName)
            : base(shortName, identifier, parent, canonicalName)
        {
            Check.StringIsMeaningful(canonicalName, "canonicalName");
        }
    }
}
