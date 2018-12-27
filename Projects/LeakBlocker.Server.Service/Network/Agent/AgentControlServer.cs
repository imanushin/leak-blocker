using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Audit;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Entities.Settings;
using LeakBlocker.Libraries.Common.Network;
using LeakBlocker.Libraries.Storage;
using LeakBlocker.Server.Service.Generated;
using LeakBlocker.Server.Service.InternalTools;
using LeakBlocker.ServerShared.AdminViewCommunication;
using LeakBlocker.ServerShared.AgentCommunication;

namespace LeakBlocker.Server.Service.Network.Agent
{
    /// <summary>
    /// Server that receives data from agent.
    /// </summary>
    internal sealed class AgentControlServer : GeneratedAgentControlService
    {
        public AgentControlServer()
            : base((ISecuritySessionManager)InternalObjects.AgentSessionManager)
        {
        }

        protected override AgentConfiguration Synchronize(AgentState state)
        {
            BaseComputerAccount agentComputer = InternalObjects.AgentSessionManager.Agent;

            ReadOnlySet<AuditItem> auditItems = state.Audit.
                Select(item => new AuditItem(item.EventType, item.Time, agentComputer, item.User, item.TextData, item.AdditionalTextData, item.Device, item.Configuration, item.Number)).
                ToReadOnlySet();

            StorageObjects.AuditItemsManager.AddItems(auditItems);

            InternalObjects.AgentStatusStore.SetComputerData(
                agentComputer,
                new ManagedComputerData(ManagedComputerStatus.Working, state.DeviceAccess));

            InternalObjects.AgentStatusStore.UpdateLastCommunicationTimeAsCurrent(agentComputer);

            ProgramConfiguration config = InternalObjects.ConfigurationStorage.CurrentFullConfiguration;
            bool isLicensed = InternalObjects.AgentStatusStore.IsUnderLicense(agentComputer);

            bool managed = InternalObjects.ScopeManager.CurrentScope().Contains(agentComputer);

            return new AgentConfiguration(config, isLicensed, managed);
        }

        protected override void SendShutdownNotification()
        {
            BaseComputerAccount agentComputer = InternalObjects.AgentSessionManager.Agent;

            InternalObjects.AgentStatusStore.SetComputerData(
                agentComputer,
                new ManagedComputerData(ManagedComputerStatus.TurnedOff, DeviceAccessMap.Empty));
        }

        protected override void SendUninstallNotification()
        {
            BaseComputerAccount agentComputer = InternalObjects.AgentSessionManager.Agent;

            InternalObjects.AuditItemHelper.AgentUninstalled(agentComputer);
            InternalObjects.AgentStatusStore.ResetLastCommunicationTime(agentComputer);
        }
    }
}
