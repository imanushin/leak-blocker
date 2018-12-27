using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Libraries.Storage.Tests
{
    [TestClass]
    public sealed class AgentEncryptionDataManagerTest : BaseStorageTest
    {
        [TestMethod]
        public void SaveLoadTest()
        {
            Mocks.ReplayAll();
            
            var first = new AgentEncryptionData(BaseComputerAccountTest.First, "1");
            var second = new AgentEncryptionData(BaseComputerAccountTest.Second, "1");
            var third = new AgentEncryptionData(BaseComputerAccountTest.First, "2");

            var target = new AgentEncryptionDataManager();

            Assert.AreEqual(0, target.GetAllData().Count);

            target.SaveAgent(first);

            Assert.AreEqual(1, target.GetAllData().Count);

            target.SaveAgent(second);

            Assert.AreEqual(2, target.GetAllData().Count);

            target.SaveAgent(third);

            Assert.AreEqual(new[] { second, third }.ToReadOnlySet(), target.GetAllData());



        }
    }
}
