using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;

namespace LeakBlocker.Libraries.Common.Cryptography
{
    /// <summary>
    /// Private key for asymmetric encrypton. Can be used for botn encryption and decryption.
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    public sealed class AsymmetricPrivateEncryptionKey : EncryptionKey
    {
        /// <summary>
        /// Creates an instance from the binary form.
        /// </summary>
        /// <param name="key">Binary data.</param>
        public AsymmetricPrivateEncryptionKey(ReadOnlyList<byte> key)
            : base(key)
        {
        }
        
        /// <summary>
        /// Создает ключ из base64 строки.
        /// </summary>
        public AsymmetricPrivateEncryptionKey(string key)
            : base(key)
        {
        }
    }
}
