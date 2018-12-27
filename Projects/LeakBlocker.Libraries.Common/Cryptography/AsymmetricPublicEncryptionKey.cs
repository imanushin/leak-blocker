using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;

namespace LeakBlocker.Libraries.Common.Cryptography
{
    /// <summary>
    /// Public key for asymmetric encrypton. Can be used for encryption only.
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    public sealed class AsymmetricPublicEncryptionKey : EncryptionKey
    {
        /// <summary>
        /// Creates an instance from the binary form.
        /// </summary>
        /// <param name="key">Binary data.</param>
        public AsymmetricPublicEncryptionKey(ReadOnlyList<byte> key)
            : base(key)
        {
        }
        
        /// <summary>
        /// Создает ключ из base64 строки.
        /// </summary>
        public AsymmetricPublicEncryptionKey(string key)
            : base(key)
        {
        }
    }
}
