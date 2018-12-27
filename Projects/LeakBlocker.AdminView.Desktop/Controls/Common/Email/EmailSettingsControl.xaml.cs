using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using LeakBlocker.AdminView.Desktop.Resources;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Resources;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.AdminView.Desktop.Controls.Common.Email
{
    internal sealed partial class EmailSettingsControl
    {
        private readonly UiEmailSettings emailSettings;
        public event Action SendTestEmailClicked;

        public EmailSettingsControl()
        {
            InitializeComponent();

            emailSettings = (UiEmailSettings) Resources["EmailSettings"];

            emailLayout.DataContext = emailSettings;
            toTextBox.DataContext = emailSettings;
            fromTextBox.DataContext = emailSettings;
            smtpServerTextBox.DataContext = emailSettings;
            isSmtpAnthorizationEnabled.DataContext = emailSettings;
            userNameTextBox.DataContext = emailSettings;
            passwordBox.DataContext = emailSettings;
        }

        public bool IsSendTestEmailVisible
        {
            get
            {
                return sendTestEmailButton.Visibility == Visibility.Visible;
            }
            set
            {
                sendTestEmailButton.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public EmailSettings Settings
        {
            get
            {
                return new EmailSettings(
                    fromTextBox.Text,
                    toTextBox.Text,
                    smtpServerTextBox.Text,
                    port.Value,
                    true == useSslConnection.IsChecked,
                    true == isSmtpAnthorizationEnabled.IsChecked,
                    userNameTextBox.Text,
                    passwordBox.Password);
            }
            set
            {
                emailSettings.Load(value);
                passwordBox.Password = value.Password;
            }
        }

        public string Errors
        {
            get
            {
                string result = emailSettings.Error;

                if (result == null && emailSettings.IsAuthenticationEnabled && string.IsNullOrEmpty(passwordBox.Password))
                    return AdminViewResources.PleaseEnterThePassword;

                return result ?? string.Empty;
            }
        }

        public void SetBusyIndicatorValue(bool isBusy)
        {
            testSendBusyIndicator.IsBusy = isBusy;
        }

        private void SendTestEmailAsync(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Errors))
            {
                MessageBox.Show(Errors, CommonStrings.ProductName, MessageBoxButton.OK, MessageBoxImage.Information);

                return;
            }

            if (SendTestEmailClicked != null)
                SendTestEmailClicked();
        }

        private void GetHotmailSettings(object sender, RoutedEventArgs e)
        {
            var newSettings = KnownAccountSelector.RequestSettings(
                ReportStrings.Hotmail,
                "hotmail.com",
                HotmailSettingsCreator,
                Window.GetWindow(this));

            LoadNewSettings(newSettings);
        }

        private void GetGmailSettings(object sender, RoutedEventArgs e)
        {
            var newSettings = KnownAccountSelector.RequestSettings(
                ReportStrings.Gmail,
                "gmail.com",
                GmailSettingsCreator,
                Window.GetWindow(this));

            LoadNewSettings(newSettings);
        }

        private void LoadNewSettings(EmailSettings newSettings)
        {
            if (newSettings == null)
                return;

            emailSettings.Load(newSettings);
            passwordBox.Password = newSettings.Password;
        }

        private static EmailSettings HotmailSettingsCreator(string email, string password)
        {
            return new EmailSettings(email, email, "smtp.live.com", 587, true, true, email, password);
        }

        private static EmailSettings GmailSettingsCreator(string email, string password)
        {
            return new EmailSettings(email, email, "smtp.gmail.com", 587, true, true, email, password);
        }
    }
}
