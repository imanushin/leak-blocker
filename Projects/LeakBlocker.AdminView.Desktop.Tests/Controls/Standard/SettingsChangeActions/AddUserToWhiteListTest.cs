using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.AdminView.Desktop.Controls.Standard.SettingsChangeActions;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;

namespace LeakBlocker.AdminView.Desktop.Tests.Controls.Standard.SettingsChangeActions
{
    partial class AddUserToWhiteListTest
    {
        private static IEnumerable<AddUserToWhiteList> GetInstances()
        {
            foreach (BaseUserAccount user in BaseUserAccountTest.objects)
            {
                yield return new AddUserToWhiteList(user);
            }
        }

        protected override bool SkipSerializationTest()
        {
            return true;
        }
    }
}
