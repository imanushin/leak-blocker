using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;

namespace LeakBlocker.Libraries.Common.Cryptography
{
    /// <summary>
    /// Provides methods for encrryption and decryption using asymmetric algorithm.
    /// </summary>
    public sealed class AsymmetricEncryptionProvider : EncryptionProvider
    {
        private const int keyLength = 2048;

        private readonly RSACryptoServiceProvider provider = new RSACryptoServiceProvider(keyLength);
        private readonly bool onlyPublicKey;

        private AsymmetricEncryptionProvider()
        {
        }

        /// <summary>
        /// Creates an instance using data that includes a private key. Such instance can be used for both encryption and decryption.
        /// </summary>
        /// <param name="key">Initialization data.</param>
        public AsymmetricEncryptionProvider(AsymmetricPrivateEncryptionKey key)
        {
            Check.ObjectIsNotNull(key, "key");

            provider.ImportCspBlob(key.Key.ToArray());
        }

        /// <summary>
        /// Creates an instance using data that includes only public key. Such instance can be used for encryption only.
        /// </summary>
        /// <param name="key">Initialization data.</param>
        public AsymmetricEncryptionProvider(AsymmetricPublicEncryptionKey key)
        {
            Check.ObjectIsNotNull(key, "key");

            onlyPublicKey = true;
            provider.ImportCspBlob(key.Key.ToArray());
        }
        
        /// <summary>
        /// Encrypts the specified data block.
        /// </summary>
        /// <param name="data">Raw data.</param>
        /// <returns>Encrypted data.</returns>
        public override byte[] Encrypt(byte[] data)
        {
            Check.CollectionIsNotNullOrEmpty(data, "data");

            ThrowIfDisposed();

            if (data.Length > 117)
                Exceptions.Throw(ErrorMessage.ObjectTooLarge);

            return provider.Encrypt(data, false);
        }

        /// <summary>
        ///  Dencrypts the specified data block.
        /// </summary>
        /// <param name="data">Encrypted data.</param>
        /// <returns>Source data.</returns>
        public override byte[] Decrypt(byte[] data)
        {
            Check.CollectionIsNotNullOrEmpty(data, "data");

            ThrowIfDisposed();

            if (onlyPublicKey)
                Exceptions.Throw(ErrorMessage.InvalidKeyUsage, "Cannot decrypt without private key.");

            return provider.Decrypt(data, false);
        }

        /// <summary>
        /// Generate a new pair of encryption keys.
        /// </summary>
        public static KeyValuePair<AsymmetricPublicEncryptionKey, AsymmetricPrivateEncryptionKey> GenerateKeys()
        {
            using (var provider = new AsymmetricEncryptionProvider())
            {
                return new KeyValuePair<AsymmetricPublicEncryptionKey, AsymmetricPrivateEncryptionKey>(
                    new AsymmetricPublicEncryptionKey(provider.provider.ExportCspBlob(false).ToReadOnlyList()),
                    new AsymmetricPrivateEncryptionKey(provider.provider.ExportCspBlob(true).ToReadOnlyList()));
            }
        }
    }
}
