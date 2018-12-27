using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common.Entities.Security
{
    /// <summary>
    /// Computer that is not joined to a domain. 
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    public sealed class LocalComputerAccount : BaseComputerAccount
    {

        /// <summary>
        /// Creates a LocalComputerAccount instance.
        /// </summary>
        /// <param name="name">Computer name.</param>
        /// <param name="identifier">Security identifier</param>
        public LocalComputerAccount(string name, AccountSecurityIdentifier identifier)
            : base(name, name, identifier, name)
        {
        }
    }
}
