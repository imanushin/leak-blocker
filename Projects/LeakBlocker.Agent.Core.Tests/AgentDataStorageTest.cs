using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Agent.Core.Implementations;
using LeakBlocker.Agent.Core.Tests.External;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Implementations;
using LeakBlocker.Libraries.Common.Tests;
using LeakBlocker.Libraries.Common.Tests.Entities;
using LeakBlocker.Libraries.Common.Tests.Entities.Settings;
using LeakBlocker.Libraries.SystemTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Agent.Core.Tests
{
    [TestClass]
    public class AgentDataStorageTest : BaseTest
    {
        [TestMethod]
        public void AgentDataStorageTest1()
        {
            SharedObjects.Singletons.ExceptionSuppressor.SetTestImplementation(new ExceptionSuppressor());
            SystemObjects.Factories.LocalDataCache.EnqueueConstructor(() => new LocalDataCacheImplementation());
            SystemObjects.Singletons.SystemAccountTools.SetTestImplementation(new SystemAccountToolsImplementation());
            SystemObjects.Singletons.FileTools.SetTestImplementation(new FileToolsImplementation());

            IAgentDataStorage target1 = new AgentDataStorage("testfile");

            Assert.IsNotNull(target1.Computer);
            Assert.IsNotNull(target1.DeviceStates);
            //Assert.IsNotNull(target1.Settings);
            Assert.IsNotNull(target1.Users);

            target1.SaveDeviceState(DeviceDescriptionTest.First, DeviceAccessType.Blocked);

            target1.Update();

            Assert.IsNotNull(target1.Computer);
            Assert.IsNotNull(target1.DeviceStates);
            //Assert.IsNotNull(target1.Settings);
            Assert.IsNotNull(target1.Users);

            target1.Settings = ProgramConfigurationTest.First;
            
            object obj = target1.ConsoleUser;
        }

        [TestMethod]
        public void AgentDataStorageTest2()
        {
            SharedObjects.Singletons.ExceptionSuppressor.SetTestImplementation(new ExceptionSuppressor());
            SystemObjects.Factories.LocalDataCache.EnqueueConstructor(() => new LocalDataCacheImplementation());
            SystemObjects.Singletons.SystemAccountTools.SetTestImplementation(new SystemAccountToolsImplementation());
            SystemObjects.Singletons.FileTools.SetTestImplementation(new FileToolsImplementation() { Throw = true });

            IAgentDataStorage target1 = new AgentDataStorage("testfile");

            Assert.IsNotNull(target1.Computer);
            Assert.IsNotNull(target1.DeviceStates);
            //Assert.IsNotNull(target1.Settings);
            Assert.IsNotNull(target1.Users);

            target1.SaveDeviceState(DeviceDescriptionTest.First, DeviceAccessType.Blocked);

            target1.Update();

            Assert.IsNotNull(target1.Computer);
            Assert.IsNotNull(target1.DeviceStates);
            //Assert.IsNotNull(target1.Settings);
            Assert.IsNotNull(target1.Users);

            target1.Settings = ProgramConfigurationTest.First;

            object obj = target1.ConsoleUser;
        }

        [TestMethod]
        public void AgentDataStorageTest3()
        {
            var fileTools = new FileToolsImplementation();

            SharedObjects.Singletons.ExceptionSuppressor.SetTestImplementation(new ExceptionSuppressor());
            SystemObjects.Factories.LocalDataCache.EnqueueConstructor(() => new LocalDataCacheImplementation());
            SystemObjects.Singletons.SystemAccountTools.SetTestImplementation(new SystemAccountToolsImplementation());
            SystemObjects.Singletons.FileTools.SetTestImplementation(fileTools);

            IAgentDataStorage target1 = new AgentDataStorage("testfile");

            Assert.IsNotNull(target1.Computer);
            Assert.IsNotNull(target1.DeviceStates);
           // Assert.IsNotNull(target1.Settings);
            Assert.IsNotNull(target1.Users);

            target1.SaveDeviceState(DeviceDescriptionTest.First, DeviceAccessType.Blocked);

            target1.Update();

            Assert.IsNotNull(target1.Computer);
            Assert.IsNotNull(target1.DeviceStates);
            //Assert.IsNotNull(target1.Settings);
            Assert.IsNotNull(target1.Users);

            target1.Settings = ProgramConfigurationTest.First;

            object obj = target1.ConsoleUser;

            IAgentDataStorage target2 = new AgentDataStorage("testfile");

            Assert.AreEqual(target1.Computer, target2.Computer);
            Assert.AreEqual(target1.DeviceStates, target2.DeviceStates);
            Assert.AreEqual(target1.Settings, target2.Settings);
            Assert.AreEqual(target1.Users, target2.Users);
            Assert.AreEqual(target1.ConsoleUser, target2.ConsoleUser);
        }

        [TestMethod]
        public void AgentDataStorageTest4()
        {
            var fileTools = new FileToolsImplementation();

            SharedObjects.Singletons.ExceptionSuppressor.SetTestImplementation(new ExceptionSuppressor());
            SystemObjects.Factories.LocalDataCache.EnqueueConstructor(() => new LocalDataCacheImplementation());
            SystemObjects.Singletons.SystemAccountTools.SetTestImplementation(new SystemAccountToolsImplementation());
            SystemObjects.Singletons.FileTools.SetTestImplementation(fileTools);

            IAgentDataStorage target1 = new AgentDataStorage("testfile");

            Assert.IsNotNull(target1.Computer);
            Assert.IsNotNull(target1.DeviceStates);
           // Assert.IsNotNull(target1.Settings);
            Assert.IsNotNull(target1.Users);

            target1.SaveDeviceState(DeviceDescriptionTest.First, DeviceAccessType.Blocked);

            target1.Update();

            Assert.IsNotNull(target1.Computer);
            Assert.IsNotNull(target1.DeviceStates);
           // Assert.IsNotNull(target1.Settings);
            Assert.IsNotNull(target1.Users);

            target1.Settings = ProgramConfigurationTest.First;

            object obj = target1.ConsoleUser;

            fileTools.Throw = true;
            IAgentDataStorage target2 = new AgentDataStorage("testfile");
        }
    }
}
