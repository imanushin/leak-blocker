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
    public sealed class CredentialsManagerTest : BaseStorageTest
    {
        [TestMethod]
        public void NullArgsCheck()
        {
            var target = new CredentialsManager();

            CheckFirstForNull<Credentials>(target.UpdateCredentials);
            CheckFirstForNull<BaseDomainAccount>((domain) => target.TryGetCredentials(domain));
        }

        [TestMethod]
        public void NoDomainNoCredentials()
        {
            Mocks.ReplayAll();

            BaseDomainAccount testAccount = BaseDomainAccountTest.objects.First();

            var target = new CredentialsManager();

            Credentials result = target.TryGetCredentials(testAccount);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void NoCredentialsForExistingDomain()
        {
            Mocks.ReplayAll();

            BaseDomainAccount testAccount = BaseDomainAccountTest.objects.First();

            using (IDatabaseModel model = StorageObjects.CreateDatabaseModel())
            {
                DbBaseDomainAccount.ConvertFromBaseDomainAccount(testAccount, model);

                model.SaveChanges();
            }

            var target = new CredentialsManager();

            Credentials result = target.TryGetCredentials(testAccount);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void SetUpdateAndGet()
        {
            Mocks.ReplayAll();

            Credentials testCredentials = CredentialsTest.objects.First();

            var target = new CredentialsManager();

            target.UpdateCredentials(testCredentials);

            var testCredentials2 = new Credentials(
                testCredentials.Domain,
                testCredentials.User + "12314234",
                testCredentials.Password + "2133123123");

            target.UpdateCredentials(testCredentials2);

            Credentials result = target.TryGetCredentials(testCredentials.Domain);

            Assert.AreEqual(testCredentials2, result);

            ReadOnlySet<Credentials> all = target.GetAllCredentials();

            Assert.AreEqual(1, all.Count);
            Assert.AreEqual(testCredentials2, all.First());
        }
    }
}
