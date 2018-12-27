using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common.Entities.Security
{
    /// <summary>
    /// Represents local a group on tha domain computer. 
    /// Class is immutable, guaranteed to be properly initialized.
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    public sealed class DomainComputerGroupAccount : BaseGroupAccount
    {
        /// <summary>
        /// Domain to which this account belongs.
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
        /// Creates a GroupAccount instance.
        /// </summary>
        /// <param name="name">Group name.</param>
        /// <param name="identifier">Group security identifier.</param>
        /// <param name="parent">Domain to which this group belongs.</param>
        public DomainComputerGroupAccount(string name, AccountSecurityIdentifier identifier, DomainComputerAccount parent)
            : base(name, identifier, parent, string.Empty)
        {
        }
    }
}
