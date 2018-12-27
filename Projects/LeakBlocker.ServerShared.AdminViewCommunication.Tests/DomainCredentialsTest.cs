using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;

namespace LeakBlocker.ServerShared.AdminViewCommunication.Tests
{
    partial class DomainCredentialsTest
    {
        private static IEnumerable<DomainCredentials> GetInstances()
        {
            foreach (DomainUserAccount userAccount in DomainUserAccountTest.objects)
            {
                yield return new DomainCredentials(userAccount.FullName, "123", userAccount.Parent.FullName);
            }
        }
    }
}
