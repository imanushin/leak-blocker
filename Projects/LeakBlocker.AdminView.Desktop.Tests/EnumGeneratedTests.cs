using Microsoft.VisualStudio.TestTools.UnitTesting;
using LeakBlocker.Libraries.Common.Resources;
using LeakBlocker.Libraries.Common;
using LeakBlocker.AdminView.Desktop.Controls.Standard.Views;


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
