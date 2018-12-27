using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.Libraries.Common.Tests.Entities.Security
{
    partial class AccountSecurityIdentifierTest
    {
        private static IEnumerable<AccountSecurityIdentifier> GetInstances()
        {
            for (int i = 0; i < 3; i++)
            {
                yield return new AccountSecurityIdentifier("S-1-5-" + i);
            }
        }
    }
}
