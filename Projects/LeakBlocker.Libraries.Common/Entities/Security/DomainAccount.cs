using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common.Entities.Security
{
    /// <summary>
    /// Represents an active directory domain.
    /// Class is immutable, guaranteed to be properly initialized.
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    public sealed class DomainAccount : BaseDomainAccount
    {
        /// <summary>
        /// Creates a DomainAccount instance.
        /// </summary>
        /// <param name="identifier">Domain security identifier.</param>
        /// <param name="canonicalName">Canonical name.</param>
        /// <param name="netBiosName"></param>
        public DomainAccount(string netBiosName, AccountSecurityIdentifier identifier, string canonicalName)
            : base(netBiosName, GetDnsName(canonicalName), identifier, canonicalName)
        {
            Check.StringIsMeaningful(canonicalName, "canonicalName");
        }

        private static string GetDnsName(string canonicalName)
        {
            Check.ObjectIsNotNull(canonicalName, "canonicalName");

            return canonicalName.EndsWith("/", StringComparison.Ordinal) ? canonicalName.Substring(0, canonicalName.Length - 1) : canonicalName;
        }
    }
}
