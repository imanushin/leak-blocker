using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Network;
using LeakBlocker.Server.Service.Generated;
using LeakBlocker.Server.Service.InternalTools;

namespace LeakBlocker.Server.Service.Network.AdminView
{
    internal sealed class AgentInstallationToolsServer : GeneratedAgentInstallationTools
    {
        public AgentInstallationToolsServer()
            : base((ISecuritySessionManager)InternalObjects.AdminViewSecuritySessionManager)
        {
        }

        protected override void ForceInstallation(ReadOnlySet<BaseComputerAccount> computers)
        {
            Check.ObjectIsNotNull(computers, "computers");

            ThreadPoolExtensions.RunWithNoWait(InternalObjects.AgentManager.InstallAgent, computers);
        }
    }
}
