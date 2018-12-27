using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;
using LeakBlocker.Server.Service.InternalTools.SingletonImplementation.LocalStorages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Server.Service.Tests.InternalTools.LocalStorages
{
    [TestClass]
    public sealed class AgentsSetupListStorageTest : BaseConfigurationManagerTest
    {
        [TestMethod]
        public void GetSetTest()
        {
            Mocks.ReplayAll();

            CleanupDirectory();

            var target = new AgentsSetupListStorage();

            Assert.IsNotNull(target.Current);

            ReadOnlySet<BaseComputerAccount> computers = BaseComputerAccountTest.objects.Objects;

            target.Save(computers);

            Assert.AreEqual(computers, target.Current);
        }
    }
}
