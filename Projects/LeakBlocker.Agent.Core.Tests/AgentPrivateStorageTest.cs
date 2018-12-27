using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Agent.Core.Implementations;
using LeakBlocker.Agent.Core.Tests.External;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Tests;
using LeakBlocker.Libraries.SystemTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Agent.Core.Tests
{
    [TestClass]
    public class AgentPrivateStorageTest : BaseTest
    {
        [TestMethod]
        public void AgentPrivateStorageTest1()
        {
            SystemObjects.Factories.PrivateRegistryStorage.EnqueueConstructor(id => new PrivateRegistryStorageImplementation());
            AgentObjects.Singletons.AgentConstants.SetTestImplementation(new AgentConstants());

            IAgentPrivateStorage target = new AgentPrivateStorage();
           
            Assert.IsNotNull(target.ServerAddress);
            Assert.IsNotNull(target.SecretKey);
            Assert.IsNotNull(target.PasswordHash);
            Assert.IsNotNull(target.FirstRun);

            target.ServerAddress = "test";
            Assert.AreEqual(target.ServerAddress, "test");

            target.SecretKey = "test";
            Assert.AreEqual(target.SecretKey, "test");
            
            target.PasswordHash = "test";
            Assert.AreEqual(target.PasswordHash, "test");
            
            target.FirstRun = true;
            Assert.AreEqual(target.FirstRun, true);
                        
            target.Licensed = true;
            Assert.AreEqual(target.Licensed, true);
            
            //target.Running = true;
            //Assert.AreEqual(target.Running, true);

            bool b1 = target.StandaloneMode;

            target.FirstRun = false;
            Assert.AreEqual(target.FirstRun, false);

            target.Licensed = false;
            Assert.AreEqual(target.Licensed, false);

            target.ServerPort = 123;
            Assert.AreEqual(target.ServerPort, 123);

            target.ServerPort = 0;
            Assert.AreEqual(target.ServerPort, 0);
        }
    }
}
