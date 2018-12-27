using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common.Entities.Security
{
    /// <summary>
    /// Base class for groups.
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    [KnownType(typeof(LocalGroupAccount))]
    [KnownType(typeof(DomainGroupAccount))]
    [KnownType(typeof(DomainComputerGroupAccount))]
    public abstract class BaseGroupAccount : Account, IGroupMember, IBaseDomainMember
    {
        /// <summary>
        /// Domain to which this account belongs. 
        /// </summary>
        [DataMember]
        [NotNull]
        public BaseDomainAccount Parent
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes properties of the current instance.
        /// </summary>
        /// <param name="shortName"></param>
        /// <param name="identifier">Group security identifier.</param>
        /// <param name="parent">Domain to which this group belongs.</param>
        /// <param name="canonicalName">Canonical name.</param>
        internal BaseGroupAccount(string shortName, AccountSecurityIdentifier identifier, BaseDomainAccount parent, string canonicalName)
            : base(shortName, CombineFullName(shortName, parent), identifier, canonicalName)
        {
            Check.ObjectIsNotNull(parent, "parent");

            Parent = parent;
        }
    }
}
