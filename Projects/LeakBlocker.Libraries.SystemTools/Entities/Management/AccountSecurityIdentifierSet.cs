using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.Libraries.SystemTools.Entities.Management
{
    /// <summary>
    /// Collection of security identifiers.
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    public sealed class AccountSecurityIdentifierSet : BaseReadOnlyObject
    {
        /// <summary>
        /// Collection of identifiers.
        /// </summary>
        [DataMember]
        public ReadOnlySet<AccountSecurityIdentifier> Identifiers
        {
            get;
            private set;
        }

        internal AccountSecurityIdentifierSet(IReadOnlyCollection<AccountSecurityIdentifier> identifiers)
        {
            Check.CollectionHasNoDefaultItems(identifiers, "identifiers");

            Identifiers = identifiers.ToReadOnlySet();
        }

        /// <summary>
        /// Returns all object that should be involved in hash code calculation and equalirty checks.
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return Identifiers;
        }
    }
}
