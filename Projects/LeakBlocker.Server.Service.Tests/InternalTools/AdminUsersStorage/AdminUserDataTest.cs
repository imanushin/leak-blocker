using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Cryptography;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Tests.Cryptography;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;
using LeakBlocker.Server.Service.InternalTools.AdminUsersStorage;

namespace LeakBlocker.Server.Service.Tests.InternalTools.AdminUsersStorage
{
    partial class AdminUserDataTest
    {
        private static IEnumerable<AdminUserData> GetInstances()
        {
            foreach (AccountSecurityIdentifier userIdentifier in AccountSecurityIdentifierTest.objects)
            {
                foreach (SymmetricEncryptionKey key in SymmetricEncryptionKeyTest.objects)
                {
                    yield return new AdminUserData(userIdentifier, key);
                }
            }
        }
    }
}
