using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;

namespace LeakBlocker.ServerShared.AgentCommunication.Tests
{
    partial class AuditItemPackageTest
    {
        private static IEnumerable<AuditItemPackage> GetInstances()
        {
            yield return new AuditItemPackage(AgentAuditItemTest.objects.Take(1).ToReadOnlySet());
            yield return new AuditItemPackage(ReadOnlySet<AgentAuditItem>.Empty);
            yield return new AuditItemPackage(AgentAuditItemTest.objects.ToReadOnlySet());
        }
    }
}
