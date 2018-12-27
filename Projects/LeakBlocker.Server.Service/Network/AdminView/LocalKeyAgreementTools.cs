using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Cryptography;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Network;
using LeakBlocker.Libraries.SystemTools;
using LeakBlocker.Server.Service.Generated;
using LeakBlocker.Server.Service.InternalTools;
using LeakBlocker.Server.Service.InternalTools.AdminUsersStorage;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.Server.Service.Network.AdminView
{
    internal sealed class LocalKeyAgreementTools : GeneratedLocalKeyAgreement
    {
        public LocalKeyAgreementTools()
            : base(new SecuritySessionManager())
        {
        }

        protected override int RegisterUser(string fullUserName, AccountSecurityIdentifier userSid, Time timeMark)
        {
            Check.StringIsMeaningful(fullUserName, "fullUserName");
            Check.ObjectIsNotNull(userSid, "userSid");

            if (timeMark.Add(TimeSpan.FromHours(1)) < Time.Now)
                throw new InvalidOperationException("Ticked for user {0} with time {1} was expired".Combine(fullUserName, timeMark));

            SymmetricEncryptionKey resultKey = AdminViewCommunicationObjects.LocalKeysAgreementHelper.GetRegistryValue(userSid, timeMark);

            int result;

            AdminUsers newStorage = InternalObjects.AdminKeysStorage.Current.AddUser(new AdminUserData(userSid, resultKey), out result);

            InternalObjects.AdminKeysStorage.Save(newStorage);

            return result;
        }

        private sealed class SecuritySessionManager : ISecuritySessionManager
        {
            private readonly Lazy<SymmetricEncryptionProvider> encryptionProvider = new Lazy<SymmetricEncryptionProvider>(CreateProvider);

            private static SymmetricEncryptionProvider CreateProvider()
            {
                return new SymmetricEncryptionProvider(AdminViewCommunicationObjects.LocalKeysAgreementHelper.DefaultKey);
            }

            public SymmetricEncryptionProvider InitSession(byte[] token)
            {
                return encryptionProvider.Value;
            }

            public void CloseSession()
            {
            }
        }
    }
}
