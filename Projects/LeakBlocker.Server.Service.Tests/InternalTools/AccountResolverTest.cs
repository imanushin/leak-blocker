using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Implementations;
using LeakBlocker.Libraries.Common.Tests;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;
using LeakBlocker.Libraries.SystemTools;
using LeakBlocker.Libraries.SystemTools.Entities;
using LeakBlocker.Server.Service.InternalTools;
using LeakBlocker.Server.Service.InternalTools.AdminUsersStorage;
using LeakBlocker.Server.Service.InternalTools.SingletonImplementation;
using LeakBlocker.ServerShared.AdminViewCommunication;
using LeakBlocker.ServerShared.AdminViewCommunication.KeysAgreement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using LeakBlocker.Libraries.Common.Collections;

namespace LeakBlocker.Server.Service.Tests.InternalTools
{
    [TestClass]
    public sealed class AccountResolverTest : BaseTest
    {
        private static readonly BaseUserAccount user = BaseUserAccountTest.First;
        private static readonly AsyncInvoker asyncInvoker = new AsyncInvoker();

        private ILocalKeysAgreementHelper localKeysAgreementHelper;
        private ISecurityObjectCache securityObjectCache;
        private IAdminKeysStorage adminKeyStorage;
        private IDomainSnapshot securityObjectContainer;
        private ISystemAccountTools accountTools;

        [TestInitialize]
        public void Init()
        {
            localKeysAgreementHelper = Mocks.StrictMock<ILocalKeysAgreementHelper>();
            adminKeyStorage = Mocks.StrictMock<IAdminKeysStorage>();
            securityObjectCache = Mocks.StrictMock<ISecurityObjectCache>();
            securityObjectContainer = Mocks.StrictMock<IDomainSnapshot>();
            accountTools = Mocks.StrictMock<ISystemAccountTools>();

            SharedObjects.Singletons.AsyncInvoker.SetTestImplementation(asyncInvoker);
            AdminViewCommunicationObjects.Singletons.LocalKeysAgreementHelper.SetTestImplementation(localKeysAgreementHelper);
            InternalObjects.Singletons.AdminKeysStorage.SetTestImplementation(adminKeyStorage);
            InternalObjects.Singletons.SecurityObjectCache.SetTestImplementation(securityObjectCache);
            SystemObjects.Singletons.SystemAccountTools.SetTestImplementation(accountTools);

            securityObjectCache.Stub(x => x.Data).Return(securityObjectContainer).Repeat.Any();
        }

        [TestMethod]
        public void ResolveUser_Cache()
        {
            securityObjectContainer.Stub(x => x.Users).Return(new[] { user }.ToReadOnlySet());

            Mocks.ReplayAll();

            var target = new AccountResolver();

            BaseUserAccount resultUser = target.ResolveUser(user.Identifier);

            Assert.AreEqual(user, resultUser);
        }

        [TestMethod]
        public void ResolveUser_Resolve()
        {
            securityObjectContainer.Stub(x => x.Users).Return(ReadOnlySet<BaseUserAccount>.Empty);
            accountTools.Stub(x => x.GetUserByIdentifier(user.Identifier)).Return(user).Repeat.Once();//Повторяем вызов ровно один раз

            Mocks.ReplayAll();

            var target = new AccountResolver();

            BaseUserAccount resultUser = target.ResolveUser(user.Identifier);

            Assert.AreEqual(user, resultUser);

            Mocks.BackToRecordAll();

            Mocks.ReplayAll();

            resultUser = target.ResolveUser(user.Identifier);//Тут уже делаем вызов из локального кеша: никаких внешних вызовов быть не должно

            Assert.AreEqual(user, resultUser);
        }
    }
}
