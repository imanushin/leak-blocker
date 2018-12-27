using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Principal;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common.Entities.Security
{
    /// <summary>
    /// Security identifier.
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    public sealed class AccountSecurityIdentifier : BaseEntity
    {
        #region Null identifier

        private static readonly AccountSecurityIdentifier nullIdentifier = new AccountSecurityIdentifier(new SecurityIdentifier(WellKnownSidType.NullSid, null));

        /// <summary>
        /// Returns null identifier (S-1-0-0).
        /// </summary>
        public static AccountSecurityIdentifier NullIdentifier
        {
            get
            {
                return nullIdentifier;
            }
        }

        #endregion

        /// <summary>
        /// Security identifier string value.
        /// </summary>
        [DataMember]
        [NotNull]
        [Required]
        public string Value
        {
            get;
            private set;
        }

        /// <summary>
        /// Выдает бинарное представление SID'а
        /// </summary>
        /// <returns></returns>
        public byte[] ToBinaryForm()
        {
            var identifier = new SecurityIdentifier(Value);

            var result = new byte[identifier.BinaryLength];

            identifier.GetBinaryForm(result,0);

            return result;
        }

        /// <summary>
        /// Creates a new instance of AccountSecurityIdentifier from binary data.
        /// </summary>
        /// <param name="identifier">Binary data.</param>
        public AccountSecurityIdentifier(IList<byte> identifier)
        {
            Check.CollectionIsNotNullOrEmpty(identifier, "identifier");

            Value = new SecurityIdentifier(identifier.ToArray(), 0).Value;
        }

        /// <summary>
        /// Creates a new instance of AccountSecurityIdentifier from string data.
        /// </summary>
        /// <param name="identifier">String data.</param>
        public AccountSecurityIdentifier(string identifier)
        {
            Check.StringIsMeaningful(identifier, "identifier");

            Value = new SecurityIdentifier(identifier).Value;
        }

        /// <summary>
        /// Creates a new instance of AccountSecurityIdentifier from binary data in unmanaged memory.
        /// </summary>
        /// <param name="identifier">Binary data pointer.</param>
        public AccountSecurityIdentifier(IntPtr identifier)
        {
            Check.PointerIsNotNull(identifier, "identifier");

            Value = new SecurityIdentifier(identifier).Value;
        }

        /// <summary>
        /// Creates a new instance of AccountSecurityIdentifier from default SecurityIdentifier.
        /// </summary>
        /// <param name="identifier">Security identifier.</param>
        public AccountSecurityIdentifier(SecurityIdentifier identifier)
        {
            Check.ObjectIsNotNull(identifier, "identifier");

            Value = identifier.Value;
        }

        #region Equality

        /// <summary>
        /// Возвращает все вложенные объекты, которые имеют смысл для вычисления хеш-кода и метода Equals
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return Value.ToUpperInvariant();
        }
        
        #endregion

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        protected override string GetString()
        {
            return Value;
        }
    }
}
