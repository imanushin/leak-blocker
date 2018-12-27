using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Settings.TemporaryAccess;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;

namespace LeakBlocker.Libraries.Common.Tests.Entities.Settings.TemporaryAccess
{
    partial class ComputerTemporaryAccessConditionTest
    {
        private static IEnumerable<ComputerTemporaryAccessCondition> GetInstances()
        {
            foreach (var computer in BaseComputerAccountTest.objects)
            {
                foreach (bool allowWrite in new[] { true, false })
                {
                    foreach (Time testDate in TimeTest.objects)
                    {
                        yield return new ComputerTemporaryAccessCondition(computer, testDate, allowWrite);
                    }
                }
            }
        }
    }
}

