using Microsoft.VisualStudio.TestTools.UnitTesting;
using LeakBlocker.Libraries.Common.Resources;
using LeakBlocker.Libraries.Common;
using LeakBlocker.ServerShared.AgentCommunication;


namespace LeakBlocker.ServerShared.AgentCommunication.Tests
{

    [TestClass]
    public sealed class AgentInstallerStatusTest
    {
        [TestMethod]
        public void AgentInstallerStatus_CheckResources()
        {
            foreach (AgentInstallerStatus item in EnumHelper<AgentInstallerStatus>.Values)
            {
                string description = item.GetValueDescription();

                Assert.IsNotNull(description);
            }
        }
    }

}
