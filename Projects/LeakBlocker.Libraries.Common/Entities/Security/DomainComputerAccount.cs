using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common.Entities.Security
{
    /// <summary>
    /// Represents a computer. Computer has two security identifiers: local and domain. Local identifier uniquely identifies
    /// the computer, while domain identifier does not.
    /// Class is immutable, guaranteed to be properly initialized.
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    public sealed class DomainComputerAccount : BaseComputerAccount, IDomainMember, IGroupMember
    {
        /// <summary>
        /// Domain to which this account belongs.
        /// </summary>
        [DataMember]
        [NotNull]
        public DomainAccount Parent
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates a DomainComputerAccount instance.
        /// </summary>
        /// <param name="name">Computer name.</param>
        /// <param name="identifier">Security identifier</param>
        /// <param name="parent">Domain. Specify null if computer is not joined to a domain.</param>
        /// <param name="canonicalName">Canonical name.</param>
        public DomainComputerAccount(string name, AccountSecurityIdentifier identifier, DomainAccount parent, string canonicalName)
            : base(name, CreateFullName(name, parent), identifier, canonicalName)
        {
            Check.ObjectIsNotNull(parent, "parent");
            Check.StringIsMeaningful(canonicalName, "canonicalName");

            Parent = parent;
        }

        private static string CreateFullName(string shortName, BaseDomainAccount parent)
        {
            Check.StringIsMeaningful(shortName, "shortName");
            Check.ObjectIsNotNull(parent, "parent");

            const string format = "{1}.{0}";

            return format.Combine(parent.FullName, shortName);
        }

        #region IBaseDomainMember

        BaseDomainAccount IBaseDomainMember.Parent
        {
            get
            {
                return Parent;
            }
        }

        #endregion
    }
}
