using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Network;

namespace LeakBlocker.ServerShared.AgentCommunication
{
    /// <summary>
    /// Contract that descibes communication between client and server.
    /// </summary>
    [NetworkObject]
    public interface IAgentControlService : IDisposable
    {
        /// <summary>
        /// Synchronizes with server.
        /// </summary>
        /// <param name="state">Agent state.</param>
        AgentConfiguration Synchronize(AgentState state);

        /// <summary>
        /// Notifies the server that computer is being turned off.
        /// </summary>
        void SendShutdownNotification();

        /// <summary>
        /// Notifies the server that computer is being turned off.
        /// </summary>
        void SendUninstallNotification();
    }
}
