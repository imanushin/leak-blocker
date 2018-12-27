using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Settings;
using LeakBlocker.Server.Service.InternalTools.LocalStorages;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.Server.Service.InternalTools
{
    internal interface IAgentSetupPasswordManager : IBaseConfigurationManager<AgentSetupPassword>
    {
        [NotNull]
        new AgentSetupPassword Current
        {
            get;
        }
    }
}
