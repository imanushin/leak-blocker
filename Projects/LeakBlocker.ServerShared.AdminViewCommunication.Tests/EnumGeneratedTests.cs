using Microsoft.VisualStudio.TestTools.UnitTesting;
using LeakBlocker.Libraries.Common.Resources;
using LeakBlocker.Libraries.Common;
using LeakBlocker.ServerShared.AdminViewCommunication;


namespace LeakBlocker.ServerShared.AdminViewCommunication.Tests
{

    [TestClass]
    public sealed class ManagedComputerStatusTest
    {
        [TestMethod]
        public void ManagedComputerStatus_CheckResources()
        {
            foreach (ManagedComputerStatus item in EnumHelper<ManagedComputerStatus>.Values)
            {
                string description = item.GetValueDescription();

                Assert.IsNotNull(description);
            }
        }
    }

    [TestClass]
    public sealed class ScopeTypeTest
    {
        [TestMethod]
        public void ScopeType_CheckResources()
        {
            foreach (ScopeType item in EnumHelper<ScopeType>.Values)
            {
                string description = item.GetValueDescription();

                Assert.IsNotNull(description);
            }
        }
    }

}
