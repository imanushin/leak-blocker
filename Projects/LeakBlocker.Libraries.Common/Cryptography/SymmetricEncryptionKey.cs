using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;

namespace LeakBlocker.Libraries.Common.Cryptography
{
    /// <summary>
    /// Ключ для шифрования с помощью AES
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    public sealed class SymmetricEncryptionKey : EncryptionKey
    {
        private const int aesKeyLength = 32;//256 бит
        private static readonly SymmetricEncryptionKey empty = new SymmetricEncryptionKey(new byte[aesKeyLength].ToReadOnlyList());

        /// <summary>
        /// Empty key.
        /// </summary>
        public static SymmetricEncryptionKey Empty
        {
            get
            {
                return empty;
            }
        }
        
        /// <summary>
        /// Создает ключ.
        /// </summary>
        public SymmetricEncryptionKey(ReadOnlyList<byte> key)
            : base(key)
        {
            if (Key.Count != aesKeyLength)
                Exceptions.Throw(ErrorMessage.InvalidData, "Key length should be {0}".Combine(aesKeyLength));
        }

        /// <summary>
        /// Создает ключ из base64 строки.
        /// </summary>
        public SymmetricEncryptionKey(string key)
            : base(key)
        {
            if (Key.Count != aesKeyLength)
                Exceptions.Throw(ErrorMessage.InvalidData, "Key length should be {0}".Combine(aesKeyLength));
        }
        
        /// <summary>
        /// Generates a new key. 
        /// </summary>
        /// <returns>New key.</returns>
        public static SymmetricEncryptionKey GenerateRandomKey()
        {
            using (var provider = new RNGCryptoServiceProvider())
            {
                var data = new byte[aesKeyLength];
                provider.GetNonZeroBytes(data);
                return new SymmetricEncryptionKey(data.ToReadOnlyList());
            }
        }
    }
}
