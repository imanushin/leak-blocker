using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.SystemTools;

namespace LeakBlocker.Server.Service.InternalTools.SingletonImplementation
{
    internal sealed class AccountResolver : IAccountResolver
    {
        private readonly ConcurrentDictionary<AccountSecurityIdentifier, BaseUserAccount> cachedUsers = new ConcurrentDictionary<AccountSecurityIdentifier, BaseUserAccount>();

        public BaseUserAccount ResolveUser(AccountSecurityIdentifier userIdentifier)
        {
            try
            {
                Log.Add("Starting resolving user {0}", userIdentifier);

                BaseUserAccount currentUser = cachedUsers.TryGetValue(userIdentifier);

                if (currentUser != null)
                    return currentUser;

                Log.Add("Trying to get user {0} from cache...", userIdentifier);

                currentUser = InternalObjects.SecurityObjectCache.Data.Users.FirstOrDefault(user => user.Identifier == userIdentifier);

                if (currentUser != null)
                {
                    cachedUsers.AddOrUpdate(userIdentifier, currentUser, (i, u) => currentUser);

                    return currentUser;
                }

                using (new TimeMeasurement("Retrieving user {0} from AD".Combine(userIdentifier), true))
                {
                    currentUser = SystemObjects.SystemAccountTools.GetUserByIdentifier(userIdentifier);

                    cachedUsers.AddOrUpdate(userIdentifier, currentUser, (i, u) => currentUser);

                    return currentUser;
                }
            }
            finally
            {
                Log.Add("Resolving user {0} finished", userIdentifier);
            }
        }
    }
}
