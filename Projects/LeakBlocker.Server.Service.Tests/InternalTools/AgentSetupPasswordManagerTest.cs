using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Tests;
using LeakBlocker.Server.Service.InternalTools.SingletonImplementation;
using LeakBlocker.Server.Service.Tests.InternalTools.LocalStorages;
using LeakBlocker.ServerShared.AdminViewCommunication;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Server.Service.Tests.InternalTools
{
    [TestClass]
    public class AgentSetupPasswordManagerTest : BaseConfigurationManagerTest
    {
        [TestInitialize]
        public void Initialize()
        {
            CleanupDirectory();
        }

        [TestMethod]
        public void SaveLoadTest()
        {
            Mocks.ReplayAll();

            var target = new AgentSetupPasswordManager();
            Assert.IsNotNull(target.Current);

            var target2 = new AgentSetupPasswordManager();
            Assert.AreEqual(target.Current, target2.Current);

            CleanupDirectory();

            var target3 = new AgentSetupPasswordManager();
            Assert.AreNotEqual(target.Current, target3.Current);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SaveLoadTest2()
        {
            var target = new AgentSetupPasswordManager();

            target.Save(new AgentSetupPassword("qqq"));
        }
    }
}
