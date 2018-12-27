using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Cryptography;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.ServerShared.AdminViewCommunication;
using LeakBlocker.ServerShared.AdminViewCommunication.KeysAgreement;
using Microsoft.Win32;

namespace LeakBlocker.AdminView.Desktop.Network
{
    internal sealed class AdminKeyStorage : IAdminKeyStorage
    {
        private int? userId;
        private SymmetricEncryptionKey key;

        private readonly object syncRoot = new object();

        public byte[] CreateUserToken()
        {
            if (key == null)
            {
                lock (syncRoot)
                {
                    if (key == null)
                    {
                        UpdateFromLocalStorage();
                    }
                }
            }

            Check.ObjectIsNotNull(userId);

            return BitConverter.GetBytes(userId.Value);
        }

        public SymmetricEncryptionKey EncryptionKey
        {
            get
            {
                if (key == null)
                {
                    lock (syncRoot)
                    {
                        if (key == null)
                        {
                            UpdateFromLocalStorage();
                        }
                    }
                }

                return key;
            }
        }

        private void UpdateFromLocalStorage()
        {
            ILocalKeysAgreementHelper keysAgreementHelper = AdminViewCommunicationObjects.LocalKeysAgreementHelper;

            Time requestTime = Time.Now;

            WindowsIdentity currentUser = WindowsIdentity.GetCurrent();

            Check.ObjectIsNotNull(currentUser);

            var sid = new AccountSecurityIdentifier(currentUser.User);

            SymmetricEncryptionKey newKey = SymmetricEncryptionKey.GenerateRandomKey();

            keysAgreementHelper.SetRegistryValue(sid, requestTime, newKey);

            using (ILocalKeyAgreement client = UiObjects.CreateLocalKeyAgreementClient())
            {
                userId = client.RegisterUser(currentUser.Name, sid, requestTime);
                
                key = newKey;
            }

            keysAgreementHelper.RemoveRegistryValue(sid, requestTime);
        }
    }
}
