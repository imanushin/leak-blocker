using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.Libraries.Common.Tests.Entities.Security
{
// ReSharper disable InconsistentNaming
    public sealed class IScopeObjectTest : ReadOnlyObjectTest
// ReSharper restore InconsistentNaming
    {
        internal static readonly ObjectsCache<IScopeObject> objects = new ObjectsCache<IScopeObject>(GetInstances);

        private static IEnumerable<IScopeObject> GetInstances()
        {
            return OrganizationalUnitTest.objects.Union(AccountTest.objects.OfType<IScopeObject>());
        }
    }
}
