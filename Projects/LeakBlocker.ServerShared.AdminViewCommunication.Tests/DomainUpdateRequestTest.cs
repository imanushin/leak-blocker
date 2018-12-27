using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;

namespace LeakBlocker.ServerShared.AdminViewCommunication.Tests
{
    partial class DomainUpdateRequestTest
    {
        private static IEnumerable<DomainUpdateRequest> GetInstances()
        {
            foreach (BaseDomainAccount domain in BaseDomainAccountTest.objects)
            {
                yield return new DomainUpdateRequest(domain, new Guid(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11));
            }
        }
    }
}
