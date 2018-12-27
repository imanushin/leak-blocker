using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LeakBlocker.AdminView.Desktop.Generated;
using LeakBlocker.Libraries.Common;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.AdminView.Desktop.Controls.Common.Converters
{
    internal sealed class ManagedComputerStatusToImageConverter : AbstractConverter<ManagedComputerStatus, FrameworkElement>
    {
        protected override FrameworkElement Convert(ManagedComputerStatus baseValue)
        {
            return GetStatusImage(baseValue);
        }

        public static FrameworkElement GetStatusImage(ManagedComputerStatus status)
        {
            switch (status)
            {
                case ManagedComputerStatus.TurnedOff:
                    return new AgentStatusOffline();

                case ManagedComputerStatus.AgentInstalling:
                    return new AgentStatusInstalling();

                case ManagedComputerStatus.Working:
                    return new AgentStatusWorking();

                case ManagedComputerStatus.AgentInstallationFailed:
                    return new AgentStatusInstallationFailed();

                case ManagedComputerStatus.Unknown:
                    return new AgentStatusUnknown();

                default:
                    throw new ArgumentOutOfRangeException("status");
            }
        }
    }
}
