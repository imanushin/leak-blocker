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
    public sealed class LocalUserAccount : BaseUserAccount
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
        /// Creates an UserAccount instance.
        /// </summary>
        /// <param name="shortName">User name.</param>
        /// <param name="identifier">User security identifier.</param>
        /// <param name="parent">Computer to which this group belongs.</param>
        public LocalUserAccount(string shortName, AccountSecurityIdentifier identifier, LocalComputerAccount parent)
            : base(shortName, identifier, parent, string.Empty)
        {
        }
    }
}
