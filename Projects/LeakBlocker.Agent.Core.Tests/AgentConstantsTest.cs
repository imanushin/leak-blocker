using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Agent.Core.Implementations;
using LeakBlocker.Agent.Core.Tests.External;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Agent.Core.Tests
{
    [TestClass]
    public class AgentConstantsTest : BaseTest
    {
        [TestMethod]
        public void AgentConstantsTest1()
        {
            IAgentConstants item = new AgentConstants();
            Assert.IsFalse(string.IsNullOrWhiteSpace(item.ServiceCommandLineMode));
            Assert.IsFalse(string.IsNullOrWhiteSpace(item.InstallerCommandLineMode));
            Assert.IsFalse(string.IsNullOrWhiteSpace(item.StandaloneServerAddress));
            Assert.IsFalse(string.IsNullOrWhiteSpace(item.ServiceName));
            Assert.IsFalse(string.IsNullOrWhiteSpace(item.ServiceDisplayedName));
            Assert.IsFalse(string.IsNullOrWhiteSpace(item.ServiceDescription));
            Assert.IsFalse(string.IsNullOrWhiteSpace(item.ServiceModuleFolder));
            Assert.IsFalse(string.IsNullOrWhiteSpace(item.ServiceModulePath));
            Assert.IsFalse(string.IsNullOrWhiteSpace(item.DatabaseFile));
            Assert.IsFalse(string.IsNullOrWhiteSpace(item.AgentDataFile));
            Assert.AreNotEqual(item.NetworkTaskInterval, default(TimeSpan));
            Assert.AreNotEqual(item.InstallerTimeout, default(TimeSpan));
        }
    }
}
