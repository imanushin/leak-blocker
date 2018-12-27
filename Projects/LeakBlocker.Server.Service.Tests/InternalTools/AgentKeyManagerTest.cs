using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Cryptography;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Tests;
using LeakBlocker.Libraries.Common.Tests.Cryptography;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;
using LeakBlocker.Libraries.Storage;
using LeakBlocker.Server.Service.InternalTools.SingletonImplementation;
using LeakBlocker.ServerShared.AgentCommunication;
using LeakBlocker.ServerShared.AgentCommunication.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace LeakBlocker.Server.Service.Tests.InternalTools
{
    [TestClass]
    public sealed class AgentKeyManagerTest : BaseTest
    {
        private IAgentEncryptionDataManager encryptionManager;

        [TestInitialize]
        public void Init()
        {
            encryptionManager = Mocks.StrictMock<IAgentEncryptionDataManager>();

            StorageObjects.Singletons.AgentEncryptionDataManager.SetTestImplementation(encryptionManager);
        }

        [TestMethod]
        public void AddGetTest()
        {
            KeyExchangeRequest firstRequest = KeyExchangeRequestTest.First;
            KeyExchangeRequest secondRequest = KeyExchangeRequestTest.Second;

            BaseComputerAccount firstComputer = BaseComputerAccountTest.First;
            BaseComputerAccount secondComputer = BaseComputerAccountTest.Second;

            SymmetricEncryptionKey firstKey1 = SymmetricEncryptionKeyTest.First;
            SymmetricEncryptionKey firstKey2 = SymmetricEncryptionKeyTest.Second;
            SymmetricEncryptionKey secondKey = SymmetricEncryptionKeyTest.Third;

            encryptionManager.Stub(x => x.GetAllData()).Return(new[] { new AgentEncryptionData(firstComputer, firstKey1.ToBase64String()) }.ToReadOnlySet());
            encryptionManager.Expect(x => x.SaveAgent(null)).IgnoreArguments().Repeat.Twice();

            Mocks.ReplayAll();

            var target = new AgentKeyManager();

            Assert.AreEqual(firstKey1, target.GetAgentKey(firstComputer));//Проверяем, что можем правильно читать из базы

            target.AddAgentKey(firstComputer, firstKey2);//Заменяем ключ

            Assert.AreEqual(firstKey2, target.GetAgentKey(firstComputer));

            target.AddAgentKey(secondComputer, secondKey);//Сохраняем другого агента

            Assert.AreEqual(secondKey, target.GetAgentKey(secondComputer));

            Mocks.VerifyAll();
        }

        //[TestMethod]
        //[ExpectedException(typeof(InvalidOperationException))]
        //public void ExceptionIfNoRequests()
        //{
        //    KeyExchangeRequest firstRequest = KeyExchangeRequestTest.First;

        //    BaseComputerAccount firstComputer = BaseComputerAccountTest.First;

        //    SymmetricEncryptionKey firstKey1 = SymmetricEncryptionKeyTest.First;

        //    encryptionManager.Stub(x => x.GetAllData()).Return(ReadOnlySet<AgentEncryptionData>.Empty);

        //    Mocks.ReplayAll();

        //    var target = new AgentKeyManager();

        //    target.AddAgentKey(firstComputer, firstKey1);
        //}
    }
}
