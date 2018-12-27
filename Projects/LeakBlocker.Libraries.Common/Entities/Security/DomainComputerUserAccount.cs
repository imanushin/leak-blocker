using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common.Entities.Security
{
    /// <summary>
    /// Represents a local user on the domain computer. 
    /// Class is immutable, guaranteed to be properly initialized.
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    public sealed class DomainComputerUserAccount : BaseUserAccount
    {
        /// <summary>
        /// Computer to which this account belongs.
        /// </summary>
        [IgnoreDataMember]
        [NotNull]
        public new DomainComputerAccount Parent
        {
            get
            {
                return (DomainComputerAccount)base.Parent;
            }
        }

        /// <summary>
        /// Creates an UserAccount instance.
        /// </summary>
        /// <param name="shortName">User name.</param>
        /// <param name="identifier">User security identifier.</param>
        /// <param name="parent">Domain to which this group belongs.</param>
        public DomainComputerUserAccount(string shortName, AccountSecurityIdentifier identifier, DomainComputerAccount parent)
            : base(shortName, identifier, parent, string.Empty)
        {
        }
    }
}
