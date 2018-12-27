using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Cryptography;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Network;
using LeakBlocker.Server.Service.InternalTools;
using LeakBlocker.Server.Service.InternalTools.AdminUsersStorage;

namespace LeakBlocker.Server.Service.Network.AdminView
{
    internal sealed class AdminViewSecuritySessionManager : Disposable, ISecuritySessionManager, IAdminViewWcfSession
    {
        private readonly ConcurrentDictionary<int, AccountSecurityIdentifier> threadMap = new ConcurrentDictionary<int, AccountSecurityIdentifier>();
        private readonly Dictionary<int, SymmetricEncryptionProvider> providers = new Dictionary<int, SymmetricEncryptionProvider>();

        private readonly object syncRoot = new object();

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]//Структура в конце-таки вычищается
        public SymmetricEncryptionProvider InitSession(byte[] token)
        {
            int userId = BitConverter.ToInt32(token, 0);

            AdminUserData user = InternalObjects.AdminKeysStorage.Current.GetUser(userId);

            threadMap.AddOrUpdate(Thread.CurrentThread.ManagedThreadId, user.UserIdentifier, (t, u) => user.UserIdentifier);

            lock (syncRoot)
            {
                if (providers.ContainsKey(userId))
                    return providers[userId];

                var result = new SymmetricEncryptionProvider(user.Key);

                providers[userId] = result;

                return result;
            }
        }

        public void CloseSession()
        {
            if (!threadMap.TryRemove(Thread.CurrentThread.ManagedThreadId))
                Log.Add("Unable to remove item from storage");
        }

        protected override void DisposeManaged()
        {
            DisposeSafe(providers.Values);
        }

        public AccountSecurityIdentifier CurrentUserIdentifier
        {
            get
            {
                int id = Thread.CurrentThread.ManagedThreadId;

                AccountSecurityIdentifier result = threadMap.TryGetValue(id);

                Check.ObjectIsNotNull(result);

                return result;
            }
        }
    }
}
