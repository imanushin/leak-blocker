using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Tests;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;
using LeakBlocker.ServerShared.AdminViewCommunication.KeysAgreement;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.ServerShared.AdminViewCommunication.Tests.KeysAgreement
{
    [TestClass]
    public sealed class LocalKeysAgreementHelperTest : BaseTest
    {
        [TestMethod]
        public void SaveLoadTest()
        {
            Mocks.ReplayAll();

            WindowsIdentity loggedOnUser = WindowsIdentity.GetCurrent();

            var user = new LocalUserAccount(
                loggedOnUser.Name,
                new AccountSecurityIdentifier(loggedOnUser.User),
                LocalComputerAccountTest.First);

            var time = Time.Now;

            var target = new LocalKeysAgreementHelper();

            target.SetRegistryValue(user.Identifier, time, target.DefaultKey);

            Assert.AreEqual(target.DefaultKey, target.GetRegistryValue(user.Identifier, time));

            target.RemoveRegistryValue(user.Identifier, time);
        }
    }
}
