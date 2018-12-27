using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common.Entities.Security
{
    /// <summary>
    /// Represents a group. Local groups can contain only users. 
    /// Active directory groups can contain users, computers and other groups.
    /// Class is immutable, guaranteed to be properly initialized.
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    public sealed class DomainGroupAccount : BaseGroupAccount, IDomainMember
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
        /// Creates a GroupAccount instance.
        /// </summary>
        /// <param name="name">Group name.</param>
        /// <param name="identifier">Group security identifier.</param>
        /// <param name="parent">Domain to which this group belongs.</param>
        /// <param name="canonicalName">Canonical name.</param>
        public DomainGroupAccount(string name, AccountSecurityIdentifier identifier, DomainAccount parent, string canonicalName)
            : base(name, identifier, parent, canonicalName)
        {
            Check.StringIsMeaningful(canonicalName, "canonicalName");
        }
    }
}
