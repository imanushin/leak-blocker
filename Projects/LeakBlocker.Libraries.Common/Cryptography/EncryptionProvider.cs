using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common.Cryptography
{
    /// <summary>
    /// Base class for encryption providers.
    /// </summary>
    public abstract class EncryptionProvider : Disposable
    {
        /// <summary>
        /// Encrypts the specified data block.
        /// </summary>
        /// <param name="data">Raw data.</param>
        /// <returns>Encrypted data.</returns>
        public abstract byte[] Encrypt(byte[] data);

        /// <summary>
        ///  Dencrypts the specified data block.
        /// </summary>
        /// <param name="data">Encrypted data.</param>
        /// <returns>Source data.</returns>
        public abstract byte[] Decrypt(byte[] data);
    }
}
