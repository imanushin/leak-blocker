using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.Libraries.Common.Tests.Entities.Security
{
    partial class CredentialsTest
    {
        private static IEnumerable<Credentials> GetInstances()
        {
            int number = 9645470;

            foreach (BaseDomainAccount account in BaseDomainAccountTest.objects)
            {
                yield return new Credentials(account, "User" + number++, "Password" + number++);
            }
        }
    }
}
