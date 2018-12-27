using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Agent.Core.Implementations.AgentDataStorageObjects;
using LeakBlocker.Agent.Core.Tests.External;
using LeakBlocker.Agent.Core.Tests.Implementations.AgentDataStorageObjects;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.SystemTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Agent.Core.Tests
{
    [TestClass]
    public class DataCacheTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DataCacheTest1()
        {
            SystemObjects.Singletons.SystemAccountTools.SetTestImplementation(new SystemAccountToolsImplementation());

            new DataCache(null);
        }

        [TestMethod]
        public void DataCacheTest2()
        {
            SystemObjects.Singletons.SystemAccountTools.SetTestImplementation(new SystemAccountToolsImplementation());

            var target = new DataCache();
            Assert.IsNotNull(target.Users);
           // Assert.IsNotNull(target.Settings);
            Assert.IsNotNull(target.DeviceStates);
           // Assert.IsNotNull(target.Computer);
        }

        [TestMethod]
        public void DataCacheTest3()
        {
            SystemObjects.Singletons.SystemAccountTools.SetTestImplementation(new SystemAccountToolsImplementation());

            var target = new DataCache(DataDiskCacheTest.First);
            Assert.IsNotNull(target.Users);
            Assert.IsNotNull(target.Settings);
            Assert.IsNotNull(target.DeviceStates);
           // Assert.IsNotNull(target.Computer);
        }
    }
}
