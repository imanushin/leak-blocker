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
    public sealed class LocalGroupAccount : BaseGroupAccount
    {
        /// <summary>
        /// Computer to which this account belongs.
        /// </summary>
        [NotNull]
        [IgnoreDataMember]
        public new LocalComputerAccount Parent
        {
            get
            {
                return (LocalComputerAccount)base.Parent;
            }
        }

        /// <summary>
        /// Creates a GroupAccount instance.
        /// </summary>
        /// <param name="name">Group name.</param>
        /// <param name="identifier">Group security identifier.</param>
        /// <param name="parent">Computer to which this group belongs.</param>
        public LocalGroupAccount(string name, AccountSecurityIdentifier identifier, LocalComputerAccount parent)
            : base(name, identifier, parent, string.Empty)
        {
        }
    }
}
