using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;
using LeakBlocker.Libraries.Storage.Entities;
using LeakBlocker.Libraries.Storage.InternalTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Libraries.Storage.Tests
{
    [TestClass]
    public sealed class AccountManagerTest : BaseStorageTest
    {
        [TestMethod]
        public void GetSavedComputers()
        {
            Mocks.ReplayAll();

            ReadOnlySet<BaseComputerAccount> testComputers = BaseComputerAccountTest.objects.ToReadOnlySet();

            using (IDatabaseModel model = StorageObjects.CreateDatabaseModel())
            {
                testComputers.ForEach(computer => DbBaseComputerAccount.ConvertFromBaseComputerAccount(computer, model));

                model.SaveChanges();
            }

            var target = new AccountManager();

            ReadOnlySet<BaseComputerAccount> result = target.GetSavedComputers();

            Assert.AreEqual(testComputers, result);
        }

        [TestMethod]
        public void GetSavedUsers()
        {
            Mocks.ReplayAll();

            ReadOnlySet<BaseUserAccount> testComputers = BaseUserAccountTest.objects.ToReadOnlySet();

            using (IDatabaseModel model = StorageObjects.CreateDatabaseModel())
            {
                testComputers.ForEach(computer => DbBaseUserAccount.ConvertFromBaseUserAccount(computer, model));

                model.SaveChanges();
            }

            var target = new AccountManager();

            ReadOnlySet<BaseUserAccount> result = target.GetSavedUsers();

            Assert.AreEqual(testComputers, result);
        }
    }
}
