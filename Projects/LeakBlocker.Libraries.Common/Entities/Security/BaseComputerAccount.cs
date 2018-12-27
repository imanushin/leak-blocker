using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common.Entities.Security
{
    /// <summary>
    /// Base class for computers.
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    [KnownType(typeof(LocalComputerAccount))]
    [KnownType(typeof(DomainComputerAccount))]
    public abstract class BaseComputerAccount : BaseDomainAccount
    {
        /// <summary>
        /// Initializes properties of the current instance.
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="identifier">Security identifier</param>
        /// <param name="canonicalName">Canonical name.</param>
        /// <param name="shortName"></param>
        internal BaseComputerAccount(string shortName, string fullName, AccountSecurityIdentifier identifier, string canonicalName)
            : base(shortName, fullName, identifier, canonicalName)
        {
        }
    }
}
