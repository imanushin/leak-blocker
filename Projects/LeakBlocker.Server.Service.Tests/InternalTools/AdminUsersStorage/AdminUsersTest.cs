using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Server.Service.InternalTools.AdminUsersStorage;

namespace LeakBlocker.Server.Service.Tests.InternalTools.AdminUsersStorage
{
    partial class AdminUsersTest
    {
        private static IEnumerable<AdminUsers> GetInstances()
        {
            int id = 0;

            yield return new AdminUsers(new Dictionary<int, AdminUserData>());
            yield return new AdminUsers(AdminUserDataTest.objects.Take(5).ToDictionary(item => id++, item => item));
            yield return new AdminUsers(AdminUserDataTest.objects.ToDictionary(item => id++, item => item));
        }
    }
}
