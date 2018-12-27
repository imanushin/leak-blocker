using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Cryptography;
using LeakBlocker.Libraries.Common.Entities.Security;
using Microsoft.Win32;

namespace LeakBlocker.ServerShared.AdminViewCommunication.KeysAgreement
{
    internal sealed class LocalKeysAgreementHelper : ILocalKeysAgreementHelper
    {
        private const string registryKeyTemplate = @"HKEY_USERS\{0}\Software\Delta Corvi\Leak Blocker\{1}";

        private const string registryValue = "adminViewUserKey";

        public SymmetricEncryptionKey DefaultKey
        {
            get
            {
                return SymmetricEncryptionKey.Empty;
            }
        }

        private static string GetRegistryKey(AccountSecurityIdentifier userSid, Time time)
        {
            var result = registryKeyTemplate.Combine(userSid.Value, time.Ticks);

            return result;
        }

        public SymmetricEncryptionKey GetRegistryValue(AccountSecurityIdentifier userSid, Time time)
        {
            var serializedKey = (byte[])Registry.GetValue(GetRegistryKey(userSid, time), registryValue, null);

            Check.ObjectIsNotNull(serializedKey);

            return BaseObjectSerializer.DeserializeFromXml<SymmetricEncryptionKey>(serializedKey);
        }

        public void SetRegistryValue(AccountSecurityIdentifier userSid, Time time, SymmetricEncryptionKey key)
        {
            byte[] serializedKey = key.SerializeToXml();

            Registry.SetValue(GetRegistryKey(userSid, time), registryValue, serializedKey);
        }

        public void RemoveRegistryValue(AccountSecurityIdentifier userSid, Time time)
        {
            Registry.SetValue(GetRegistryKey(userSid, time), registryValue, string.Empty);
        }
    }
}
