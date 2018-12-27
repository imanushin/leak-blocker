using LeakBlocker.AdminView.Desktop.Controls.Standard.Views;
using LeakBlocker.Libraries.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.AdminView.Desktop.Tests.Controls.Standard.Views
{

    [TestClass]
    public sealed class DeviceDescriptionStateTest
    {
        [TestMethod]
        public void DeviceDescriptionState_CheckResources()
        {
            foreach (DeviceDescriptionState item in EnumHelper<DeviceDescriptionState>.Values)
            {
                string description = item.GetValueDescription();

                Assert.IsNotNull(description);
            }
        }
    }

}
