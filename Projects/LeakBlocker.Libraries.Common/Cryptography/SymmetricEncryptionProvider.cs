using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common.Cryptography
{
    /// <summary>
    /// Упрощает работу с шифрованием
    /// </summary>
    public sealed class SymmetricEncryptionProvider : EncryptionProvider
    {
        private static readonly byte[] initializationVector = new byte[] { 180, 34, 30, 99, 137, 230, 39, 144, 37, 152, 239, 199, 40, 122, 247, 30 };

        private readonly AesCryptoServiceProvider cryptoProvider = new AesCryptoServiceProvider();
        private readonly byte[] secretKey;

        /// <summary>
        /// Key that is used for encryption.
        /// </summary>
        public SymmetricEncryptionKey Key
        {
            get;
            private set;
        }

        /// <summary>
        /// Создает объект, резервирует Unmanaged ресурсы
        /// </summary>
        public SymmetricEncryptionProvider(SymmetricEncryptionKey key)
        {
            Check.ObjectIsNotNull(key, "key");

            secretKey = key.Key.ToArray();
            Key = key;
        }

        /// <summary>
        /// Шифрует входные данные
        /// </summary>
        public override byte[] Encrypt(byte[] data)
        {
            Check.CollectionIsNotNullOrEmpty(data, "data");

            ThrowIfDisposed();

            using (var memoryStream = new MemoryStream())
            {
                using (ICryptoTransform encryptor = cryptoProvider.CreateEncryptor(secretKey, initializationVector))
                {
                    var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

                    cryptoStream.Write(data, 0, data.Length);
                    cryptoStream.Flush();
                    cryptoStream.FlushFinalBlock();

                    byte[] encryptedData = memoryStream.GetBuffer();

                    Array.Resize(ref encryptedData, (int) memoryStream.Length);

                    byte[] sourceSizeData = BitConverter.GetBytes(data.Length);

                    var result = new byte[encryptedData.Length + sourceSizeData.Length];
                    sourceSizeData.CopyTo(result, 0);

                    Array.Copy(encryptedData, 0, result, 4, encryptedData.Length);

                    return result;
                }
            }
        }

        /// <summary>
        /// Расшифровывает данные
        /// </summary>
        public override byte[] Decrypt(byte[] data)
        {
            Check.CollectionIsNotNullOrEmpty(data, "data");

            ThrowIfDisposed();

            int sourceDataSize = BitConverter.ToInt32(data, 0);
            int sizeDataLength = BitConverter.GetBytes(sourceDataSize).Length;

            using (var memoryStream = new MemoryStream(data, sizeDataLength, data.Length - sizeDataLength, false))
            {
                using (ICryptoTransform decryptor = cryptoProvider.CreateDecryptor(secretKey, initializationVector))
                {
                    var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

                    var result = new byte[sourceDataSize];

                    cryptoStream.Read(result, 0, result.Length);

                    return result;
                }
            }
        }

        /// <summary>
        /// Releases resources.
        /// </summary>
        protected override void DisposeManaged()
        {
            DisposeSafe(cryptoProvider);
        }
    }
}
