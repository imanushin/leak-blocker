using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.AdminView.Desktop.Common;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;

namespace LeakBlocker.AdminView.Desktop.Tests.Common
{
    partial class UiComputerTest
    {
        private static IEnumerable<UiComputer> GetInstances()
        {
            foreach (BaseComputerAccount computer in BaseComputerAccountTest.objects)
            {
                yield return new UiComputer(computer);
            }
        }

        protected override bool SkipSerializationTest()
        {
            return true;
        }

    }
}
