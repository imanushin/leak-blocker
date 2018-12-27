using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Cryptography;

namespace LeakBlocker.ServerShared.AgentCommunication
{
    /// <summary>
    /// Client-side class for key eexchange session. Encrypts secret data. Encrypted result can be returned to the server
    /// using even insecure ways.
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    public sealed class KeyExchangeReply : BaseReadOnlyObject
    {
        [DataMember]
        private readonly ReadOnlyList<byte> encryptedData;

        /// <summary>
        /// Creates an instance of the KeyExchangeReply class.
        /// </summary>
        /// <param name="request">Server request that contains public data.</param>
        /// <param name="key">Secret key that should be passed to the server.</param>
        public KeyExchangeReply(KeyExchangeRequest request, SymmetricEncryptionKey key)
        {
            Check.ObjectIsNotNull(request, "request");
            Check.ObjectIsNotNull(key, "key");

            using (var provider = new AsymmetricEncryptionProvider(request))
            {
                encryptedData = provider.Encrypt(key).ToReadOnlyList();
            }
        }

        /// <summary>
        /// Returns all object that should be involved in hash code calculation and equalirty checks.
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return encryptedData;
        }

        /// <summary>
        /// Converts the current instance to the binary form.
        /// </summary>
        /// <param name="value">Class instance.</param>
        /// <returns>Byte array.</returns>
        public static implicit operator byte[](KeyExchangeReply value)
        {
            Check.ObjectIsNotNull(value, "value");

            return value.encryptedData.ToArray();
        }

        /// <summary>
        /// Converts the current instance to the binary form.
        /// </summary>
        /// <returns>Byte array.</returns>
        public byte[] ToByteArray()
        {
            return this;
        }
    }
}
