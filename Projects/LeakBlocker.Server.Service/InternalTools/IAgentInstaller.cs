using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.ServerShared.AgentCommunication;

namespace LeakBlocker.Server.Service.InternalTools
{
    internal interface IAgentInstaller
    {
        AgentInstallerStatus Install(BaseComputerAccount computer, Credentials credentials);
    }
}
