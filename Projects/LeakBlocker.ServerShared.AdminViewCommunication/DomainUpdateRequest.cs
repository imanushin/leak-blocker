using System.Linq;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.ServerShared.AdminViewCommunication
{
    /// <summary>
    /// Description of the domain update request that was started after updating the credentials.
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    public sealed class DomainUpdateRequest : BaseReadOnlyObject
    {
        /// <summary>
        /// Target domain.
        /// </summary>
        [DataMember]
        public BaseDomainAccount Domain
        {
            get;
            private set;
        }
         
        /// <summary>
        /// Update Request identifier.
        /// </summary>
        [DataMember]
        public Guid RequestIdentifier
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates an instance.
        /// </summary>
        public DomainUpdateRequest(BaseDomainAccount domain, Guid requestIdentifier)
        {
            Check.ObjectIsNotNull(domain, "domain");

            Domain = domain;
            RequestIdentifier = requestIdentifier;
        }

        /// <summary>
        /// Returns all object that should be involved in hash code calculation and equalirty checks.
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return Domain;
            yield return RequestIdentifier;
        }
    }
}