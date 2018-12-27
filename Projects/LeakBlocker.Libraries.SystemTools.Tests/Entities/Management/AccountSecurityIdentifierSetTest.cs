using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;
using LeakBlocker.Libraries.SystemTools.Entities.Management;

namespace LeakBlocker.Libraries.SystemTools.Tests.Entities.Management
{
    partial class AccountSecurityIdentifierSetTest
    {
        private static IEnumerable<AccountSecurityIdentifierSet> GetInstances()
        {
            yield return new AccountSecurityIdentifierSet(AccountSecurityIdentifierTest.objects.ToReadOnlySet());
            yield return new AccountSecurityIdentifierSet(AccountSecurityIdentifierTest.objects.Skip(1).ToReadOnlySet());
        }
    }
}
