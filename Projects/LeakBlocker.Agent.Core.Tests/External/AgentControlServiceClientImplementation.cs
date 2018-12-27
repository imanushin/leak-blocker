using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Settings;
using LeakBlocker.ServerShared.AgentCommunication;

namespace LeakBlocker.Agent.Core.Tests.External
{
    internal sealed class AgentControlServiceClientImplementation : BaseTestImplementation, IAgentControlService
    {
        readonly bool licensed;
        readonly ProgramConfiguration settings;
        readonly bool managed;

        public AgentControlServiceClientImplementation(ProgramConfiguration settings, bool licensed = true, bool managed = true)
        {
            this.licensed = licensed;
            this.settings = settings;
            this.managed = managed;
        }

        public AgentConfiguration Synchronize(AgentState state)
        {
            base.RegisterMethodCall("Synchronize", state);

            if (settings == null)
                throw new InvalidOperationException();

            return new AgentConfiguration(settings, licensed, managed);
        }

        public void SendShutdownNotification()
        {
            base.RegisterMethodCall("SendShutdownNotification");
        }

        public void SendUninstallNotification()
        {
            base.RegisterMethodCall("SendUninstallNotification");
        }

        public void Dispose()
        {
        }
    }
}
