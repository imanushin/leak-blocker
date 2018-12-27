using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;
using LeakBlocker.Libraries.SystemTools.Entities;

namespace LeakBlocker.Libraries.SystemTools.Tests.Entities
{
    partial class CachedUserDataTest 
    {
        private static IEnumerable<CachedUserData> GetInstances()
        {
            foreach (BaseUserAccount user in BaseUserAccountTest.objects)
            {
                yield return new CachedUserData(user, AccountSecurityIdentifierTest.objects.ToReadOnlySet());
            }
        }

        protected override bool SkipSerializationTest()
        {
            return true;
        }
    }
}
