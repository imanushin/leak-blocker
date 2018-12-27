using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.AdminView.Desktop;
using LeakBlocker.AdminView.Desktop.Network;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Cryptography;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Network;
using LeakBlocker.Libraries.Common.Tests.Cryptography;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;
using LeakBlocker.Server.Service.InternalTools;
using LeakBlocker.Server.Service.InternalTools.AdminUsersStorage;
using LeakBlocker.Server.Service.Network.AdminView;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace LeakBlocker.Server.Service.Tests.Network.AdminView
{
    [TestClass]
    public abstract class BaseAdminViewNetworkTest : BaseNetworkTest
    {
        private static readonly BaseUserAccount currentUser = BaseUserAccountTest.Third;
        private static readonly SymmetricEncryptionKey channelKey = SymmetricEncryptionKeyTest.Third;
        private static readonly byte[] userToken = new byte[4];

        private static readonly AdminUsers currentUsers = new AdminUsers(new Dictionary<int, AdminUserData>()
        {
            {0, new AdminUserData(currentUser.Identifier,channelKey)}
        });

        private static readonly SymmetricEncryptionProvider symmetricEncryptionProvider = new SymmetricEncryptionProvider(channelKey);

        protected static BaseUserAccount NetworkUser
        {
            get
            {
                return currentUser;
            }
        }

        protected new void Initialize()
        {
            var adminKeyStorage = Mocks.StrictMock<IAdminKeyStorage>();
            var adminKeysStorage = Mocks.StrictMock<IAdminKeysStorage>();
            var wcfSession = Mocks.StrictMultiMock<IAdminViewWcfSession>(typeof(ISecuritySessionManager));

            adminKeyStorage.Stub(x => x.EncryptionKey).Return(channelKey).Repeat.Any();
            adminKeyStorage.Stub(x => x.CreateUserToken()).Return(userToken).Repeat.Any();
            adminKeysStorage.Stub(x => x.Current).Return(currentUsers);

            wcfSession.Stub(x => x.CurrentUserIdentifier).Return(currentUser.Identifier).Repeat.Any();
            ((ISecuritySessionManager)wcfSession).Stub(x => x.InitSession(userToken)).Return(symmetricEncryptionProvider).Repeat.Any();
            ((ISecuritySessionManager)wcfSession).Stub(x => x.CloseSession()).Repeat.Any();

            InternalObjects.Singletons.AdminKeysStorage.SetTestImplementation(adminKeysStorage);
            InternalObjects.Singletons.AdminViewSecuritySessionManager.SetTestImplementation(wcfSession);
            UiObjects.Singletons.AdminKeyStorage.SetTestImplementation(adminKeyStorage);


            BaseNetworkTest.Initialize();
        }
    }
}
