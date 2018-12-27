using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.AdminView.Desktop.Network;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Tests;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;
using LeakBlocker.Libraries.SystemTools;
using LeakBlocker.Libraries.SystemTools.Entities;
using LeakBlocker.ServerShared.AdminViewCommunication;
using LeakBlocker.ServerShared.AdminViewCommunication.KeysAgreement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32;
using Rhino.Mocks;

namespace LeakBlocker.AdminView.Desktop.Tests.Network
{
    [TestClass]
    public sealed class AdminKeyStorageTest : BaseTest
    {
        [TestMethod]
        public void GenerateAndSaveTest()
        {
            var keyAgreementHelper = Mocks.StrictMock<ILocalKeysAgreementHelper>();
            var keyAgreement = Mocks.StrictMock<ILocalKeyAgreement>();
            var systemAccountTools = Mocks.StrictMock<ISystemAccountTools>();

            keyAgreementHelper.Expect(x => x.SetRegistryValue(null, null, null)).IgnoreArguments().Repeat.Twice();//До вызова обязательно должны заполнить
            keyAgreementHelper.Expect(x => x.RemoveRegistryValue(null, null)).IgnoreArguments().Repeat.Any();
            keyAgreement.Stub(x => x.RegisterUser(null, null, null)).Return(1).IgnoreArguments().Repeat.Any();
            keyAgreement.Stub(x => x.Dispose()).Repeat.Any();

            AdminViewCommunicationObjects.Singletons.LocalKeysAgreementHelper.SetTestImplementation(keyAgreementHelper);
            UiObjects.Factories.LocalKeyAgreementClient.EnqueueTestImplementation(keyAgreement);
            UiObjects.Factories.LocalKeyAgreementClient.EnqueueTestImplementation(keyAgreement);
            SystemObjects.Singletons.SystemAccountTools.SetTestImplementation(systemAccountTools);

            Mocks.ReplayAll();

            if (Registry.CurrentUser.OpenSubKey("Software\\Delta Corvi\\Leak Blocker\\AdminView") != null)
                Registry.CurrentUser.DeleteSubKeyTree("Software\\Delta Corvi\\Leak Blocker\\AdminView");

            var target = new AdminKeyStorage();

            Assert.IsNotNull(target.EncryptionKey);
            Assert.IsNotNull(target.CreateUserToken());

            target = new AdminKeyStorage();

            Assert.IsNotNull(target.CreateUserToken());
            Assert.IsNotNull(target.EncryptionKey);
        }
    }
}
