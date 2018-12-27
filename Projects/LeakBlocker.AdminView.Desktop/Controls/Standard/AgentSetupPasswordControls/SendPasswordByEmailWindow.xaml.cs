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
using System.Windows.Shapes;
using LeakBlocker.AdminView.Desktop.Resources;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Resources;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.AdminView.Desktop.Controls.Standard.AgentSetupPasswordControls
{
    internal sealed partial class SendPasswordByEmailWindow
    {
        private SendPasswordByEmailWindow()
        {
            InitializeComponent();
        }

        public static void SendPasswordByEmail(Window owner)
        {
            var window = new SendPasswordByEmailWindow();

            window.Owner = owner;

            window.ShowDialog();
        }

        private void CancelClicked(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SendClicked(object sender, RoutedEventArgs e)
        {
            var errors = emailSettingsControl.Errors;

            if (errors != null && errors.Any())
                return;

            busyIndicator.IsBusy = true;

            SharedObjects.AsyncInvoker.Invoke(SendEmail, emailSettingsControl.Settings);
        }

        private void SendEmail(EmailSettings settings)
        {
            using (IAgentSetupPasswordTools clinet = UiObjects.CreateAgentSetupPasswordToolsClient())
            {
                string error = null;

                try
                {
                    clinet.SendPassword(settings);
                }
                catch (Exception ex)
                {
                    error = ex.GetExceptionMessage();
                }

                Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (string.IsNullOrWhiteSpace(error))
                        {
                            Close();

                            return;
                        }

                        MessageBox.Show(
                            this,
                            AdminViewResources.ThereWereErrorDuringSendingMailTemplate.Combine(error),
                            CommonStrings.ProductName,
                            MessageBoxButton.OK);

                        busyIndicator.IsBusy = false;

                    }));
            }
        }
    }
}
