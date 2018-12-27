using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.AdminView.Desktop.Controls.Standard.SettingsChangeActions;
using LeakBlocker.Libraries.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.AdminView.Desktop.Tests.Controls.Standard.SettingsChangeActions
{
    partial class BaseChangeActionTest
    {
        protected override bool SkipSerializationTest()
        {
            return true;
        }

        [TestMethod]
        public void ShortTextTest()
        {
            foreach (BaseChangeAction action in objects)
            {
                string result = action.ShortText;

                Assert.IsNotNull(result);
            }
        }
    }
}
