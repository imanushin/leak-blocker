using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.AdminView.Desktop.Controls.Standard.SettingsChangeActions;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Settings.TemporaryAccess;
using LeakBlocker.Libraries.Common.Tests.Entities.Settings.TemporaryAccess;

namespace LeakBlocker.AdminView.Desktop.Tests.Controls.Standard.SettingsChangeActions
{
    partial class CancelTemporaryAccessActionTest
    {
        private static IEnumerable<CancelTemporaryAccessAction> GetInstances()
        {
            foreach (BaseTemporaryAccessCondition baseTemporaryAccessCondition in BaseTemporaryAccessConditionTest.objects)
            {
                yield return new CancelTemporaryAccessAction(baseTemporaryAccessCondition);
            }
        }

        protected override bool SkipSerializationTest()
        {
            return true;
        }
    }
}
