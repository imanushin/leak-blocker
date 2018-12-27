using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Agent.Core.Implementations;
using LeakBlocker.Agent.Core.Tests.External;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Cryptography;
using LeakBlocker.Libraries.Common.Tests;
using LeakBlocker.Libraries.SystemTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Agent.Core.Tests
{
    [TestClass]
    public class VersionIndependentPrivateStorageTest : BaseTest
    {
        [TestMethod]
        public void VersionIndependentPrivateStorageTest1()
        {
            SharedObjects.Singletons.CommandLine.SetTestImplementation(new CommandLineImplementation("notepad.exe"));
            SystemObjects.Factories.PrivateRegistryStorage.EnqueueConstructor(id => new PrivateRegistryStorageImplementation());
            AgentObjects.Singletons.AgentConstants.SetTestImplementation(new AgentConstants());

            IVersionIndependentPrivateStorage target = new VersionIndependentPrivateStorage();

            Assert.IsTrue(target.Empty);
            Assert.IsNotNull(target.UninstallString);
            Assert.IsNotNull(target.PasswordHash);
            Assert.IsNotNull(target.Version);

            target.SaveData("qwerty", SymmetricEncryptionKey.Empty.ToBase64String());

            Assert.IsFalse(target.Empty);
            Assert.IsFalse(string.IsNullOrWhiteSpace(target.UninstallString));
            Assert.IsFalse(string.IsNullOrWhiteSpace(target.PasswordHash));
            Assert.IsFalse(string.IsNullOrWhiteSpace(target.Version));
        }
    }
}
