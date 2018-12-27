using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Cryptography;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.ServerShared.AdminViewCommunication.KeysAgreement
{
    /// <summary>
    /// Общий интерфейс для упрощенного согласования ключей пользователей
    /// </summary>
    public interface ILocalKeysAgreementHelper
    {
        /// <summary>
        /// Ключ шифрования по-умолчанию (пустой ключ, используется в качестве "открытой" аутентификации)
        /// </summary>
        SymmetricEncryptionKey DefaultKey
        {
            get;
        }

        /// <summary>
        /// Выдает сохраненный ключ шифрования
        /// </summary>
        SymmetricEncryptionKey GetRegistryValue(AccountSecurityIdentifier user, Time time);

        /// <summary>
        /// Выдает сохраненный ключ шифрования
        /// </summary>
        void SetRegistryValue(AccountSecurityIdentifier user, Time time, SymmetricEncryptionKey key);

        /// <summary>
        /// Удаляет
        /// </summary>
        void RemoveRegistryValue(AccountSecurityIdentifier user, Time time);
    }
}
