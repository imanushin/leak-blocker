using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using LeakBlocker.AdminView.Desktop.Network;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Cryptography;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Implementations;
using LeakBlocker.Libraries.Common.Network;
using LeakBlocker.Libraries.Common.SystemTools;
using LeakBlocker.Libraries.Common.Tests;
using LeakBlocker.Libraries.Common.Tests.Cryptography;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;
using LeakBlocker.Libraries.SystemTools;
using LeakBlocker.Libraries.SystemTools.Entities;
using LeakBlocker.Libraries.SystemTools.Entities.Implementations;
using LeakBlocker.Server.Service.InternalTools;
using LeakBlocker.Server.Service.InternalTools.AdminUsersStorage;
using LeakBlocker.Server.Service.Network.AdminView;
using LeakBlocker.Server.Service.Tests.InternalTools.AdminUsersStorage;
using LeakBlocker.ServerShared.AdminViewCommunication;
using LeakBlocker.ServerShared.AdminViewCommunication.KeysAgreement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace LeakBlocker.Server.Service.Tests.Network.AdminView
{
    [TestClass]
    public sealed class LocalKeyAgreementToolsTest : BaseTest
    {
        private static readonly BaseUserAccount user = BaseUserAccountTest.First;
        private static readonly SymmetricEncryptionKey defaultKey = SymmetricEncryptionKeyTest.First;

        private static readonly Time validTime = Time.Now;
        private static readonly Time invalidTime = Time.Unknown;

        private static readonly AsyncInvoker asyncInvoker = new AsyncInvoker();

        private ILocalKeysAgreementHelper localKeysAgreementHelper;
        private IAdminKeysStorage adminKeyStorage;
        private IThreadPool threadPool;

        [TestInitialize]
        public void Initialize()
        {
            localKeysAgreementHelper = Mocks.StrictMock<ILocalKeysAgreementHelper>();
            adminKeyStorage = Mocks.StrictMock<IAdminKeysStorage>();
            threadPool = new NativeThreadPool(1);

            SharedObjects.Singletons.AsyncInvoker.SetTestImplementation(asyncInvoker);
            AdminViewCommunicationObjects.Singletons.LocalKeysAgreementHelper.SetTestImplementation(localKeysAgreementHelper);
            InternalObjects.Singletons.AdminKeysStorage.SetTestImplementation(adminKeyStorage);
            SharedObjects.Factories.ThreadPool.EnqueueTestImplementation(threadPool);
        }

        [TestCleanup]
        public void Cleanup()
        {
            threadPool.Dispose();
        }

        [TestMethod]
        public void RegisterUser_Ok_Cache()
        {
            localKeysAgreementHelper.Stub(x => x.DefaultKey).Return(defaultKey);
            localKeysAgreementHelper.Stub(x => x.GetRegistryValue(user.Identifier, validTime)).Return(SymmetricEncryptionKeyTest.Second);

            adminKeyStorage.Stub(x => x.Current).Return(AdminUsersTest.First);
            adminKeyStorage.Stub(x => x.Save(null)).IgnoreArguments();

            Mocks.ReplayAll();

            using (var serverHost = new BaseNetworkHost(SharedObjects.Constants.DefaultTcpPort, SharedObjects.Constants.DefaultTcpTimeout))
            {
                serverHost.RegisterServer(new LocalKeyAgreementTools());

                Thread.Sleep(1000);//Ждем, пока стартует сервер

                using (ILocalKeyAgreement client = new LocalKeyAgreementClient())
                {
                    client.RegisterUser(user.FullName, user.Identifier, validTime);
                }
            }
        }

        [TestMethod]
        public void RegisterUser_Ok_DirectRequest()
        {
            localKeysAgreementHelper.Stub(x => x.DefaultKey).Return(defaultKey);
            localKeysAgreementHelper.Stub(x => x.GetRegistryValue(user.Identifier, validTime)).Return(SymmetricEncryptionKeyTest.Second);

            adminKeyStorage.Stub(x => x.Current).Return(AdminUsersTest.First);
            adminKeyStorage.Stub(x => x.Save(null)).IgnoreArguments();

            Mocks.ReplayAll();

            using (var serverHost = new BaseNetworkHost(SharedObjects.Constants.DefaultTcpPort, SharedObjects.Constants.DefaultTcpTimeout))
            {
                serverHost.RegisterServer(new LocalKeyAgreementTools());

                Thread.Sleep(1000);//Ждем, пока стартует сервер

                using (ILocalKeyAgreement client = new LocalKeyAgreementClient())
                {
                    client.RegisterUser(user.FullName, user.Identifier, validTime);
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]//Ибо запрос устарел
        public void RegisterUser_Fail()
        {
            localKeysAgreementHelper.Stub(x => x.DefaultKey).Return(defaultKey);

            Mocks.ReplayAll();

            using (var serverHost = new BaseNetworkHost(SharedObjects.Constants.DefaultTcpPort, SharedObjects.Constants.DefaultTcpTimeout))
            {
                serverHost.RegisterServer(new LocalKeyAgreementTools());

                Thread.Sleep(1000);//Ждем, пока стартует сервер

                using (ILocalKeyAgreement client = new LocalKeyAgreementClient())
                {
                    client.RegisterUser(user.FullName, user.Identifier, invalidTime);
                }
            }
        }
    }
}
