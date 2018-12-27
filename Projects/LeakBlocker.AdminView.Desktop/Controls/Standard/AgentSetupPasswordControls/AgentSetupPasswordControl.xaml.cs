using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LeakBlocker.Libraries.Common;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.AdminView.Desktop.Controls.Standard.AgentSetupPasswordControls
{
    internal sealed partial class AgentSetupPasswordControl
    {
        private static AgentSetupPassword recievedPassword;

        public AgentSetupPasswordControl()
        {
            InitializeComponent();

            if (recievedPassword != null)
                UpdatePasswordData();
            else
                SharedObjects.AsyncInvoker.Invoke(UpdateAdminSetupPassword);
        }

        private void UpdateAdminSetupPassword()
        {
            using (IAgentSetupPasswordTools client = UiObjects.CreateAgentSetupPasswordToolsClient())
            {
                recievedPassword = client.GetPassword();

                Dispatcher.BeginInvoke(new Action(UpdatePasswordData));
            }
        }

        private void UpdatePasswordData()
        {
            agentSetupPasswordText.Text = recievedPassword.Value;
            copyToClipboard.IsEnabled = true;
            sendToEmail.IsEnabled = true;
        }

        private void CopyToClipbaordClick(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(recievedPassword.Value);
        }

        private void SendToEmailClick(object sender, RoutedEventArgs e)
        {
            SendPasswordByEmailWindow.SendPasswordByEmail(Window.GetWindow(this));
        }
    }
}
