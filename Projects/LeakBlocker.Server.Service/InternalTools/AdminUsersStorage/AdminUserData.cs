using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Cryptography;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.Server.Service.InternalTools.AdminUsersStorage
{
    [DataContract(IsReference = true)]
    internal sealed class AdminUserData : BaseReadOnlyObject
    {
        public AdminUserData(AccountSecurityIdentifier userIdentifier, SymmetricEncryptionKey key)
        {
            Check.ObjectIsNotNull(userIdentifier, "userIdentifier");
            Check.ObjectIsNotNull(key, "key");

            UserIdentifier = userIdentifier;
            Key = key;
        }

        [DataMember]
        public AccountSecurityIdentifier UserIdentifier
        {
            get;
            private set;
        }

        [DataMember]
        public SymmetricEncryptionKey Key
        {
            get;
            private set;
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return UserIdentifier;
            yield return Key;
        }
    }
}
