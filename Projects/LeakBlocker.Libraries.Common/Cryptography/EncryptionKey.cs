using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;

namespace LeakBlocker.Libraries.Common.Cryptography
{
    /// <summary>
    /// Base class for encryption keys.
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    [KnownType(typeof(SymmetricEncryptionKey))]
    [KnownType(typeof(AsymmetricPrivateEncryptionKey))]
    [KnownType(typeof(AsymmetricPublicEncryptionKey))]
    public abstract class EncryptionKey : BaseReadOnlyObject
    {
        /// <summary>
        /// Ключ для шифрования.
        /// </summary>
        [DataMember]
        public ReadOnlyList<byte> Key
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates an instance from the binary form.
        /// </summary>
        /// <param name="key">Binary data.</param>
        protected EncryptionKey(ReadOnlyList<byte> key)
        {
            Check.CollectionIsNotNullOrEmpty(key, "key");

            Key = key;
        }

        /// <summary>
        /// Creates an instance from the string form.
        /// </summary>
        /// <param name="value">String data.</param>
        protected EncryptionKey(string value)
        {
            Check.StringIsMeaningful(value, "value");

            Key = Convert.FromBase64String(value).ToReadOnlyList();
        }

        /// <summary>
        /// Перечисляет все вложенные объекты
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return Key;
        }

        /// <summary>
        /// Преобразование в Base64 строку
        /// </summary>
        /// <returns></returns>
        public string ToBase64String()
        {
            return Convert.ToBase64String(Key.ToArray());
        }

        /// <summary>
        /// Converts the instance to the binary form.
        /// </summary>
        /// <param name="value">Class instance.</param>
        /// <returns>Binary form.</returns>
        public static implicit operator byte[](EncryptionKey value)
        {
            Check.ObjectIsNotNull(value, "value");

            return value.Key.ToArray();
        }
    }
}
