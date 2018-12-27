using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using LeakBlocker.AdminView.Desktop.Resources;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.AdminView.Desktop.Controls.Common.AccountWindows
{
    internal sealed partial class AskAndUpdateCredentialsWindow
    {
        private readonly BaseDomainAccount account;

        private double baseHeight;

        private AskAndUpdateCredentialsWindow(BaseDomainAccount account)
        {
            this.account = account;
            InitializeComponent();

            userNameBox.Text = account.FullName + "\\Administrator";
            userNameBox.CaretIndex = userNameBox.Text.Length;
            userNameBox.Focus();
        }

        private void OkButtonClicked(object sender, RoutedEventArgs e)
        {
            EnterFinished();
        }

        private void EnterFinished()
        {
            if (!CheckAndShowError())
                return;

            busyIndicator.IsBusy = true;

            SharedObjects.AsyncInvoker.Invoke(CheckCredentials, new DomainCredentials(userNameBox.Text, passwordBox.Password, account.FullName));
        }

        private bool CheckAndShowError()
        {
            if (string.IsNullOrWhiteSpace(userNameBox.Text))
            {
                userNameBox.Focus();
                userNameBox.CaretIndex = userNameBox.Text.Length;
                SetErrorText(AdminViewResources.PleaseEnterUserName);

                return false;
            }

            if (string.IsNullOrEmpty(passwordBox.Password))
            {
                passwordBox.Focus();
                SetErrorText(AdminViewResources.PleaseEnterPassword);

                return false;
            }

            SetErrorText(string.Empty);
            return true;
        }

        private void CheckCredentials(DomainCredentials credentials)
        {
            using (var client = UiObjects.CreateAccountToolsClient())
            {
                try
                {
                    client.CheckAndSetCredentials(credentials);

                    Dispatcher.BeginInvoke(
                        new Action(() =>
                                   {
                                       SetErrorText(string.Empty);
                                       DialogResult = true;
                                       busyIndicator.IsBusy = false;
                                   }));
                }
                catch (Exception ex)
                {
                    Log.Write(ex);

                    Dispatcher.BeginInvoke(
                        new Action(() =>
                                   {
                                       SetErrorText(ex.GetExceptionMessage());
                                       busyIndicator.IsBusy = false;
                                   }));
                }
            }
        }

        private void CancelButtonClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void SizeChangedHandler(object sender, SizeChangedEventArgs e)
        {
            if (Math.Abs(baseHeight - 0) < 0.1)
                baseHeight = MinHeight = MaxHeight = ActualHeight;
        }

        private void KeyUpHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                EnterFinished();

                e.Handled = true;
            }
        }

        private void SetErrorText(string newErrorText)
        {
            newErrorText = newErrorText.Trim();

            errorText.Text = newErrorText;
            errorText.Visibility = string.IsNullOrEmpty(newErrorText) ? Visibility.Collapsed : Visibility.Visible;
            errorText.UpdateLayout();

            if (errorText.Visibility == Visibility.Visible)
                MinHeight = MaxHeight = baseHeight + errorText.ActualHeight + errorText.Margin.Top + errorText.Margin.Bottom;
            else
                MaxHeight = MinHeight = baseHeight;

            Height = MinHeight;
        }

        public static void OpenWindow(BaseDomainAccount account, Window owner)
        {
            Check.ObjectIsNotNull(account, "account");
            Check.ObjectIsNotNull(owner, "owner");

            var domain = account as DomainAccount;
            var localComputer = account as LocalComputerAccount;

            if (domain == null && localComputer == null)
                throw new NotSupportedException("The type {0} is not supported".Combine(account.GetType().Name));

            var window = new AskAndUpdateCredentialsWindow(account);

            window.Owner = owner;

            window.objectNameRun.Text = account.FullName;

            if (domain != null)
            {
                window.preffixRun.Text = AdminViewResources.PleaseEnterCredentialsForAccessingToEntireDomain;
            }
            else
            {
                window.preffixRun.Text = AdminViewResources.PleaseEnterCredentialsForAccessingToLocalComputer;
            }

            window.ShowDialog();
        }

    }
}
