using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using LeakBlocker.AdminView.Desktop.Common;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Audit;
using LeakBlocker.Libraries.Common.Resources;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.AdminView.Desktop.Controls.Simple
{
    internal sealed partial class ReportsTab
    {
        public ReportsTab()
        {
            InitializeComponent();

            SharedObjects.AsyncInvoker.Invoke(LoadConfiguration);

            emailSettingsControl.SendTestEmailClicked += SendTestEmail;
        }

        private void LoadConfiguration()
        {
            ReportConfiguration settings = UiObjects.UiConfigurationManager.CurrentReportConfiguration;

            Dispatcher.BeginInvoke(
                new Action(() =>
                           {
                               areEnabledBox.IsChecked = settings.AreEnabled;

                               errorsCheckBox.IsChecked = settings.Filter.Errors;

                               deviceBlocking.IsChecked = settings.Filter.BlockOperations != OperationDetail.None;
                               deviceAllowing.IsChecked = settings.Filter.AllowOperations != OperationDetail.None;
                               temporaryAccess.IsChecked = settings.Filter.TemporaryAccessOperations != OperationDetail.None;

                               includeFileForBlockCheckBox.IsChecked = settings.Filter.BlockOperations == OperationDetail.DevicesAndFiles;
                               includeFileToAllowCheckBox.IsChecked = settings.Filter.AllowOperations == OperationDetail.DevicesAndFiles;
                               includeFileToTeporaryAccessCheckBox.IsChecked = settings.Filter.TemporaryAccessOperations == OperationDetail.DevicesAndFiles;

                               configurationChangesCheckBox.IsChecked = settings.Filter.ConfigurationChanges;
                               warningsCheckBox.IsChecked = settings.Filter.Warnings;

                               emailSettingsControl.Settings = settings.Email;
                           })
            );
        }


        public bool CheckIsValidAndNavigate()
        {
            if (false == areEnabledBox.IsChecked)
                return true;

            string addressErrors = emailSettingsControl.Errors;

            if (!string.IsNullOrWhiteSpace(addressErrors))
            {
                MessageBox.Show(addressErrors, CommonStrings.ProductName, MessageBoxButton.OK, MessageBoxImage.Information);

                return false;
            }
            
            return true;
        }

        public void SaveSettingsAsync()
        {
            var newConfig = CollectConfiguration();

            SharedObjects.AsyncInvoker.Invoke(SaveSettings, newConfig);
        }


        private void SendTestEmail()
        {
            if (emailSettingsControl.Errors.Any())
                return;

            emailSettingsControl.SetBusyIndicatorValue(true);

            var newConfig = CollectConfiguration();

            SharedObjects.AsyncInvoker.Invoke(SendTestEmail, newConfig);
        }

        private void SendTestEmail(ReportConfiguration configuration)
        {
            using (IReportTools client = UiObjects.CreateReportToolsClient())
            {
                string error = client.TrySendTestReport(configuration);

                Dispatcher.BeginInvoke(
                    new Action(() =>
                    {
                        if (string.IsNullOrWhiteSpace(error))
                            MessageBox.Show(ReportStrings.EmailSentSuccessfully, CommonStrings.ProductName, MessageBoxButton.OK, MessageBoxImage.Information);
                        else
                            MessageBox.Show(ReportStrings.UnableToSendEmail.Combine(error), CommonStrings.ProductName, MessageBoxButton.OK, MessageBoxImage.Warning);

                        emailSettingsControl.SetBusyIndicatorValue(false);
                    }));
            }
        }


        private ReportConfiguration CollectConfiguration()
        {
            var newConfig = new ReportConfiguration
                (
                true == areEnabledBox.IsChecked,
                new ReportFilter(
                    true == errorsCheckBox.IsChecked,
                    GetOperationDetails(deviceBlocking, includeFileForBlockCheckBox),
                    GetOperationDetails(deviceAllowing, includeFileToAllowCheckBox),
                    GetOperationDetails(temporaryAccess, includeFileToTeporaryAccessCheckBox),
                    true == configurationChangesCheckBox.IsChecked,
                    true == warningsCheckBox.IsChecked),
                emailSettingsControl.Settings
                );
            return newConfig;
        }

        private static OperationDetail GetOperationDetails(ToggleButton primaryCheckBox, ToggleButton includeFiles)
        {
            if (true != primaryCheckBox.IsChecked)
                return OperationDetail.None;

            if (true == includeFiles.IsChecked)
                return OperationDetail.DevicesAndFiles;

            return OperationDetail.OnlyDevices;
        }

        private static void SaveSettings(ReportConfiguration newConfig)
        {
            UiObjects.UiConfigurationManager.SaveReportConfiguration(newConfig);
        }
    }
}
