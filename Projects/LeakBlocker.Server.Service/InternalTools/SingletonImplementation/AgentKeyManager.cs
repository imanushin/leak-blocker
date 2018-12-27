using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Cryptography;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Storage;
using LeakBlocker.ServerShared.AgentCommunication;

namespace LeakBlocker.Server.Service.InternalTools.SingletonImplementation
{
    internal sealed class AgentKeyManager : IAgentKeyManager
    {
        private readonly ConcurrentDictionary<BaseComputerAccount, SymmetricEncryptionKey> keys = CreateInitialData();

        private static ConcurrentDictionary<BaseComputerAccount, SymmetricEncryptionKey> CreateInitialData()
        {
            Dictionary<BaseComputerAccount, SymmetricEncryptionKey> dictionary = StorageObjects.AgentEncryptionDataManager.GetAllData().ToDictionary(item => item.Computer, item => new SymmetricEncryptionKey(item.Key));

            return new ConcurrentDictionary<BaseComputerAccount, SymmetricEncryptionKey>(dictionary);
        }

        public void AddAgentKey(BaseComputerAccount agent, SymmetricEncryptionKey key)
        {
            Check.ObjectIsNotNull(key, "key");
            Check.ObjectIsNotNull(agent, "agent");

            keys.AddOrUpdate(agent, key, (c, k) => key);

            StorageObjects.AgentEncryptionDataManager.SaveAgent(new AgentEncryptionData(agent, Convert.ToBase64String(key.Key.ToArray())));
        }

        public SymmetricEncryptionKey GetAgentKey(BaseComputerAccount agent)
        {
            Check.ObjectIsNotNull(agent, "agent");

            SymmetricEncryptionKey result = keys.TryGetValue(agent);

            Check.ObjectIsNotNull(result);
            
            return result;
        }
    }
}
