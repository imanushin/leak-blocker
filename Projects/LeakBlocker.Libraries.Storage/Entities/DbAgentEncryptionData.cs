using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.Libraries.Storage.Entities
{
    partial class DbAgentEncryptionData
    {
        private AgentEncryptionData ForceGetAgentEncryptionData()
        {
            return new AgentEncryptionData(Computer.GetBaseComputerAccount(), Key);
        }
    }
}
