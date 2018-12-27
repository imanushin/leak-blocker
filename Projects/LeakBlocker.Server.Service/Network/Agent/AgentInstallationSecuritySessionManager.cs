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
    internal sealed class AgentInstallationSecuritySessionManager : Disposable, ISecuritySessionManager, IAgentNetworkSession
    {
        private readonly Lazy<SymmetricEncryptionProvider> encryptionProvider = new Lazy<SymmetricEncryptionProvider>(CreateProvider);

        private readonly ConcurrentDictionary<int, BaseComputerAccount> threadMap = new ConcurrentDictionary<int, BaseComputerAccount>();
        
        private static SymmetricEncryptionProvider CreateProvider()
        {
            return new SymmetricEncryptionProvider(SymmetricEncryptionKey.Empty);
        }

        public SymmetricEncryptionProvider InitSession(byte[] token)
        {
            var identifier = new AccountSecurityIdentifier(token);

            BaseComputerAccount computer = InternalObjects.SecurityObjectCache.Data.Computers.First(currentComputer => currentComputer.Identifier == identifier);

            threadMap.AddOrUpdate(Thread.CurrentThread.ManagedThreadId, computer, (k, c) => computer);

            return encryptionProvider.Value;
        }

        public void CloseSession()
        {
            if (!threadMap.TryRemove(Thread.CurrentThread.ManagedThreadId))
                Log.Add("Unable to remove item from storage");
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

        protected override void DisposeManaged()
        {
            DisposeSafe(encryptionProvider.Value);
            threadMap.Clear();
            
            base.DisposeManaged();
        }
    }
}