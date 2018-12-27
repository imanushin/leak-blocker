using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;

namespace LeakBlocker.ServerShared.AdminViewCommunication.Tests
{
    partial class ResultComputerTest
    {
        private static IEnumerable<ResultComputer> GetInstances()
        {
            foreach (BaseComputerAccount baseComputerAccount in BaseComputerAccountTest.objects)
            {
                yield return new ResultComputer(ScopeTest.objects.First(), baseComputerAccount);
            }
        }
    }
}
