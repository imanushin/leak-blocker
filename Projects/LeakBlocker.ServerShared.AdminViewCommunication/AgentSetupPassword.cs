using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.ServerShared.AdminViewCommunication
{
    /// <summary>
    /// Password for uninstalling agents without server.
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    public sealed class AgentSetupPassword : BaseReadOnlyObject
    {
        /// <summary>
        /// Password.
        /// </summary>
        [DataMember]
        public string Value
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="value">Password.</param>
        public AgentSetupPassword(string value)
        {
            Check.StringIsMeaningful(value, "value");

            Value = value;
        }

        /// <summary>
        /// Returns all object that should be involved in hash code calculation and equalirty checks.
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return Value;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        protected override string GetString()
        {
            return Value;
        }

        /// <summary>
        /// Generates a new random password.
        /// </summary>
        /// <returns>New password.</returns>
        public static AgentSetupPassword Generate()
        {
            using (var provider = new RNGCryptoServiceProvider())
            {
                var data = new byte[64];
                provider.GetBytes(data);

                return new AgentSetupPassword(Convert.ToBase64String(data));
            }
        }
    }
}
