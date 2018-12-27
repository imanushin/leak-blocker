using LeakBlocker.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.AdminView.Desktop.Controls.Standard.AgentSetupPasswordControls
{
    internal sealed partial class AgentSetupPasswordViewer
    {
        private AgentSetupPasswordViewer()
        {
            InitializeComponent();
        }

        internal static void ShowAgentSetupPasswordViewer(Window owner)
        {
            var window = new AgentSetupPasswordViewer();

            window.Owner = owner;

            window.ShowDialog();
        }
    }
}
