using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Network;
using LeakBlocker.Server.Service.Generated;
using LeakBlocker.Server.Service.InternalTools;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.Server.Service.Network.AdminView
{
    internal sealed class StatusToolsServer : GeneratedStatusTools
    {
        public StatusToolsServer()
            : base((ISecuritySessionManager)InternalObjects.AdminViewSecuritySessionManager)
        {
        }

        protected override ReadOnlySet<ManagedComputer> GetStatuses()
        {
            return InternalObjects.AgentStatusStore
                                  .GetManagedScope()
                                  .Select(pair => new ManagedComputer(pair.Key, pair.Value))
                                  .ToReadOnlySet();
        }
    }
}
