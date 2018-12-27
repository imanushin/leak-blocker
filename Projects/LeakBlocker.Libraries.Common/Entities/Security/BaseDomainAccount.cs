using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common.Entities.Security
{
    /// <summary>
    /// Base class for domains. Domains can contain other accounts.
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    [KnownType(typeof(DomainAccount))]
    [KnownType(typeof(BaseComputerAccount))]
    public abstract class BaseDomainAccount : Account
    {
        /// <summary>
        /// Initializes properties of the current instance.
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="identifier">Security identifier</param>
        /// <param name="canonicalName">Canonical name.</param>
        /// <param name="shortName"></param>
        internal BaseDomainAccount(string shortName, string fullName, AccountSecurityIdentifier identifier, string canonicalName)
            : base(shortName, fullName, identifier, canonicalName)
        {
        }
    }
}
