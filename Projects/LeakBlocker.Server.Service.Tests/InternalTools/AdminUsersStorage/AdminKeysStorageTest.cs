using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Server.Service.InternalTools.AdminUsersStorage;
using LeakBlocker.Server.Service.Tests.InternalTools.LocalStorages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Server.Service.Tests.InternalTools.AdminUsersStorage
{
    [TestClass]
    public sealed class AdminKeysStorageTest : BaseConfigurationManagerTest
    {
        [TestMethod]
        public void SaveLoadNewTest()
        {
            Mocks.ReplayAll();

            CleanupDirectory();

            var target = new AdminKeysStorage();

            Assert.IsNotNull(target.Current);

            int id;

            var newData = target.Current.AddUser(AdminUserDataTest.First, out id);

            Assert.IsNotNull(newData.GetUser(id));

            target.Save(newData);

            Assert.AreEqual(newData, target.Current);



            newData = target.Current.AddUser(AdminUserDataTest.Second, out id);

            Assert.IsNotNull(newData.GetUser(id));

            target.Save(newData);

            Assert.AreEqual(newData, target.Current);
        }

    }
}
