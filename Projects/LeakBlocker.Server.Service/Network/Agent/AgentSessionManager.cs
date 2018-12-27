using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Cryptography;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Network;
using LeakBlocker.Server.Service.InternalTools;

namespace LeakBlocker.Server.Service.Network.Agent
{
    internal sealed class AgentSessionManager : Disposable, ISecuritySessionManager, IAgentNetworkSession
    {
        private readonly ConcurrentDictionary<int, BaseComputerAccount> threadMap = new ConcurrentDictionary<int, BaseComputerAccount>();
        private readonly ConcurrentDictionary<BaseComputerAccount, SymmetricEncryptionProvider> providers = new ConcurrentDictionary<BaseComputerAccount, SymmetricEncryptionProvider>();

        public SymmetricEncryptionProvider InitSession(byte[] sidData)
        {
            var identifier = new AccountSecurityIdentifier(sidData);

            ReadOnlySet<BaseComputerAccount> computersInCache = InternalObjects.SecurityObjectCache.Data.Computers;

            BaseComputerAccount computer = computersInCache.FirstOrDefault(currentComputer => currentComputer.Identifier == identifier);

            if (computer == null)
                throw new InvalidOperationException("Unable to find computer with SID {0} in cache. Total computer count in cache: {1}".Combine(identifier, computersInCache.Count));

            SymmetricEncryptionProvider provider = GetProvider(computer);

            threadMap.AddOrUpdate(Thread.CurrentThread.ManagedThreadId, computer, (k, c) => computer);

            return provider;
        }

        private SymmetricEncryptionProvider GetProvider(BaseComputerAccount computer)
        {
            SymmetricEncryptionKey key = InternalObjects.AgentKeyManager.GetAgentKey(computer);

            return providers.AddOrUpdate(computer, comp => new SymmetricEncryptionProvider(key),
                delegate(BaseComputerAccount currentComputer, SymmetricEncryptionProvider existingProvider)
                {
                    if (existingProvider.Key == key)
                        return existingProvider;
                    existingProvider.Dispose();
                    return new SymmetricEncryptionProvider(key);
                });
        }

        public void CloseSession()
        {
            if (!threadMap.TryRemove(Thread.CurrentThread.ManagedThreadId))
                Log.Add("Unable to remove item from storage");
        }

        protected override void DisposeManaged()
        {
            providers.Values.ForEach(item => item.Dispose());

            threadMap.Clear();
            providers.Clear();

            base.DisposeManaged();
        }

        public BaseComputerAccount Agent
        {
            get
            {
                int id = Thread.CurrentThread.ManagedThreadId;

                BaseComputerAccount result = threadMap.TryGetValue(id);

                Check.ObjectIsNotNull(result);

                return result;
            }
        }
    }
}
