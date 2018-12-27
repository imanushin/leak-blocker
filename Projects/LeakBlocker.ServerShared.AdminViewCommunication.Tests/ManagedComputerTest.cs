using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;

namespace LeakBlocker.ServerShared.AdminViewCommunication.Tests
{
    partial class ManagedComputerTest
    {
        private static IEnumerable<ManagedComputer> GetInstances()
        {
            foreach (ManagedComputerData computerData in ManagedComputerDataTest.objects)
            {
                foreach (BaseComputerAccount baseComputerAccount in BaseComputerAccountTest.objects)
                {
                    yield return new ManagedComputer(baseComputerAccount, computerData);
                }
            }
        }
    }
}
