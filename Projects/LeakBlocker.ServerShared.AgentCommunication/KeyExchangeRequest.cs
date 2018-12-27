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
    /// Class for secure key exchange. Public data of the generated request is sent to the client and then private data is 
    /// used to decrypt the reply. Public data should be passed to client using spoofing-protected ways.
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    public sealed class KeyExchangeRequest : BaseReadOnlyObject
    {
        [DataMember]
        private readonly AsymmetricPublicEncryptionKey publicKey;
        private readonly AsymmetricPrivateEncryptionKey privateKey;

        /// <summary>
        /// Creates a new instance for key exchange. Key pair is randomly generated.
        /// </summary>
        public KeyExchangeRequest()
        {
            KeyValuePair<AsymmetricPublicEncryptionKey, AsymmetricPrivateEncryptionKey> keyPair = AsymmetricEncryptionProvider.GenerateKeys();

            publicKey = keyPair.Key;
            privateKey = keyPair.Value;
        }

        private KeyExchangeRequest(AsymmetricPublicEncryptionKey publicKey)
        {
            this.publicKey = publicKey;
        }

        /// <summary>
        /// Returns all object that should be involved in hash code calculation and equalirty checks.
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return publicKey;
        }

        /// <summary>
        /// Decrypts the specified reply. Warning! Only original instance can be used for decryption.
        /// </summary>
        /// <param name="data">Encrrypted reply.</param>
        /// <returns>Secret data.</returns>
        public SymmetricEncryptionKey Decrypt(KeyExchangeReply data)
        {
            if (privateKey == null)
                throw new InvalidOperationException("Deserialized instance does not contain private key.");

            using (var provider = new AsymmetricEncryptionProvider(privateKey))
            {
                return new SymmetricEncryptionKey(provider.Decrypt(data).ToReadOnlyList());
            }
        }

        /// <summary>
        /// Converts the current instance to the string form. String form contains only public key.
        /// </summary>
        /// <param name="value">Instanccee that is being converted.</param>
        /// <returns>String form of the specified instance.</returns>
        public static implicit operator string(KeyExchangeRequest value)
        {
            Check.ObjectIsNotNull(value, "value");

            return value.publicKey.ToBase64String();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        protected override string GetString()
        {
            return this;
        }

        /// <summary>
        /// Converts string form of the KeyExchangeRequest to the class instance.
        /// </summary>
        /// <param name="value">String form.</param>
        /// <returns>Class instance.</returns>
        public static implicit operator KeyExchangeRequest(string value)
        {
            Check.StringIsMeaningful(value, "value");

            return new KeyExchangeRequest(new AsymmetricPublicEncryptionKey(value));
        }

        /// <summary>
        /// Converts the currrent instance to an AsymmetricPublicEncryptionKey instance.
        /// </summary>
        /// <returns>Public key.</returns>
        public static implicit operator AsymmetricPublicEncryptionKey(KeyExchangeRequest value)
        {
            Check.ObjectIsNotNull(value, "value");

            return value.publicKey;
        }

        /// <summary>
        /// Converts public key to the KeyExchangeRequest instance. Of cource result instance contains only public key.
        /// </summary>
        /// <param name="value">Public key.</param>
        /// <returns>KeyExchangeRequest instance.</returns>
        public static implicit operator KeyExchangeRequest(AsymmetricPublicEncryptionKey value)
        {
            Check.ObjectIsNotNull(value, "value");

            return new KeyExchangeRequest(value);
        }

        /// <summary>
        /// Converts public key to the KeyExchangeRequest instance. Of cource result instance contains only public key.
        /// </summary>
        /// <param name="value">Public key.</param>
        /// <returns>KeyExchangeRequest instance.</returns>
        public static KeyExchangeRequest FromAsymmetricPublicEncryptionKey(AsymmetricPublicEncryptionKey value)
        {
            return value;
        }

        /// <summary>
        /// Converts the currrent instance to an AsymmetricPublicEncryptionKey instance.
        /// </summary>
        /// <returns>Public key.</returns>
        public AsymmetricPublicEncryptionKey ToAsymmetricPublicEncryptionKey()
        {
            return this;
        }

        /// <summary>
        /// Converts string form of the KeyExchangeRequest to the class instance.
        /// </summary>
        /// <param name="value">String form.</param>
        /// <returns>Class instance.</returns>
        public static KeyExchangeRequest FromString(string value)
        {
            return value;
        }
    }
}
