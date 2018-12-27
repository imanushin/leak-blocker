using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Settings;
using LeakBlocker.Libraries.Common.Tests.Entities.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Libraries.Storage.Tests
{
    [TestClass]
    public sealed class ConfigurationManagerTest : BaseDatabaseObjectsTests
    {
        [TestMethod]
        public void SaveLoadTest()
        {
            Mocks.ReplayAll();

            var first = ProgramConfigurationTest.objects.First();
            var second = ProgramConfigurationTest.objects.Skip(1).First();

            Assert.AreNotEqual(first, second, "Internal test error: test configurations should be different");

            var target = new ConfigurationManager();

            Assert.IsNull(target.GetLastConfiguration());

            target.SaveConfiguration(first);
            
            ProgramConfiguration firstResult = target.GetLastConfiguration();

            Assert.AreEqual(first, firstResult);

            target.SaveConfiguration(second);

            ProgramConfiguration secondResult = target.GetLastConfiguration();

            Assert.AreEqual(second, secondResult);
        }
    }
}
