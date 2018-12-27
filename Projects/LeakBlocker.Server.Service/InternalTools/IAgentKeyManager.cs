using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Cryptography;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.ServerShared.AgentCommunication;

namespace LeakBlocker.Server.Service.InternalTools
{
    internal interface IAgentKeyManager
    {
        void AddAgentKey(BaseComputerAccount agent, SymmetricEncryptionKey key);

        SymmetricEncryptionKey GetAgentKey(BaseComputerAccount agent);
    }
}
