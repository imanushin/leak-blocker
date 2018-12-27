using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using LeakBlocker.AdminView.Desktop.Common.Collections;
using LeakBlocker.AdminView.Desktop.Resources;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.AdminView.Desktop.Controls.Common.AccountWindows
{
    internal sealed partial class FindInOtherDomainWindow : IDisposable
    {
        private static readonly SolidColorBrush errorBrush = Brushes.LightCoral;
        private static ReadOnlySet<string> previouslyFoundNames = new ReadOnlySet<string>(new string[0]);

        private DomainUpdateRequest result;

        private readonly List<IDisposable> objectsToDispose = new List<IDisposable>();

        private static readonly Brush defaultControlBackground = Brushes.White;

        private bool isBusy;

        private double baseHeight;

        private string previousDomainName = string.Empty;

        public FindInOtherDomainWindow()
        {
            InitializeComponent();

            domainSelector.TextChanged += DomainNameTextChanged;
            domainSelector.SetItems(previouslyFoundNames);

            SharedObjects.AsyncInvoker.Invoke(RequestDnsNames);
        }

        private void RequestDnsNames()
        {
            using (IAccountTools client = UiObjects.CreateAccountToolsClient())
            {
                previouslyFoundNames = client.FindDnsDomains();

                Dispatcher.BeginInvoke(new Action(() => domainSelector.SetItems(previouslyFoundNames)));
            }
        }

        private void InitWithScope(string textToFind, string helpText, string preferedDomain)
        {
            findInformation.Inlines.Add(new Run(textToFind));

            if (!string.IsNullOrWhiteSpace(helpText))
            {
                findInformation.Inlines.Add(HelpTooltip.CreateHelpInjection(helpText));
            }

            if (preferedDomain != null)
            {
                domainSelector.Text = preferedDomain;
                userNameBox.Text = preferedDomain + "\\Administrator";
                passwordBox.Focus();
            }
        }

        private void AddClicked(object sender, RoutedEventArgs e)
        {
            InputCompleted();
        }

        private void InputCompleted()
        {
            if (!Verify())
                return;

            UpdateBusyState(true);

            var credentials = new DomainCredentials(userNameBox.Text, passwordBox.Password, domainSelector.Text);

            SharedObjects.AsyncInvoker.Invoke(FindDomain, credentials);
        }

        private bool Verify()
        {
            domainSelector.Background = defaultControlBackground;
            userNameBox.Background = defaultControlBackground;
            passwordBox.Background = defaultControlBackground;

            if (string.IsNullOrWhiteSpace(domainSelector.Text))
            {
                ChangeErrorText(AdminViewResources.PleaseEnterDomain);
                domainSelector.Focus();
                domainSelector.Background = errorBrush;

                return false;
            }

            if (string.IsNullOrWhiteSpace(userNameBox.Text))
            {
                ChangeErrorText(AdminViewResources.PleaseEnterUserName);
                userNameBox.Focus();
                userNameBox.Background = errorBrush;

                return false;
            }

            if (string.IsNullOrWhiteSpace(passwordBox.Password))
            {
                ChangeErrorText(AdminViewResources.PleaseEnterPassword);
                passwordBox.Focus();
                passwordBox.Background = errorBrush;

                return false;
            }

            ChangeErrorText(string.Empty);

            return true;
        }

        private void FindDomain(DomainCredentials credentials)
        {
            using (IAccountTools client = UiObjects.CreateAccountToolsClient())
            {
                try
                {
                    DomainUpdateRequest windowResult = client.CheckAndSetCredentials(credentials);

                    Dispatcher.BeginInvoke(new Action(() => ReturnResult(windowResult)));
                }
                catch (Exception ex)
                {
                    if (!IsVisible)
                        return;

                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        UpdateBusyState(false);
                        ChangeErrorText(ex.GetExceptionMessage());
                        Log.Write(ex);
                    }));
                }
            }
        }

        private void UpdateBusyState(bool newBusyValue)
        {
            isBusy = newBusyValue;

            domainSelector.IsEnabled = !isBusy;
            userNameBox.IsEnabled = !isBusy;
            passwordBox.IsEnabled = !isBusy;
            addDomainButton.IsEnabled = !isBusy;
            waitText.Visibility = isBusy ? Visibility.Visible : Visibility.Collapsed;
            waitIndicatorControl.Visibility = isBusy ? Visibility.Visible : Visibility.Collapsed;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (string.IsNullOrWhiteSpace(domainSelector.Text))
                domainSelector.Focus();
            else
                passwordBox.Focus();
        }

        private void CancelClicked(object sender, RoutedEventArgs e)
        {
            ReturnResult(null);
        }

        private void ReturnResult(DomainUpdateRequest dialogResult)
        {
            Close();

            result = dialogResult;

            if (result != null)
                UiObjects.DomainCache.AddDomain(result.Domain);
        }

        private void WindowKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Handled)
                return;

            if (!isBusy && e.Key == Key.Enter)
            {
                e.Handled = true;
                InputCompleted();
            }

            if (e.Key == Key.Escape)
            {
                e.Handled = true;
                ReturnResult(null);
            }
        }

        public void Dispose()
        {
            Disposable.DisposeSafe(objectsToDispose);
        }

        private void UserNameChanged(object sender, TextChangedEventArgs e)
        {
            if (userNameBox.Text.Length > 0)
                userNameBox.Background = defaultControlBackground;
        }

        private void DomainNameTextChanged()
        {
            string newDomainName = domainSelector.Text;

            if (newDomainName.Length > 0)
                domainSelector.Background = defaultControlBackground;

            if (!string.IsNullOrWhiteSpace(previousDomainName) && userNameBox.Text.StartsWith(previousDomainName))
                userNameBox.Text = newDomainName + userNameBox.Text.Substring(previousDomainName.Length);

            if (string.IsNullOrEmpty(previousDomainName) && userNameBox.Text.StartsWith("\\"))
                userNameBox.Text = newDomainName + userNameBox.Text;

            previousDomainName = newDomainName;
        }

        private void PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (passwordBox.Password.Length > 0)
                passwordBox.Background = defaultControlBackground;
        }

        private void DomainSelectorKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;

                userNameBox.Focus();
            }
        }

        private void UserNameBoxGotFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(userNameBox.Text))
            {
                userNameBox.Text = "{0}\\Administrator".Combine(domainSelector.Text);

                passwordBox.Focus();
            }
        }

        private void ChangeErrorText(string text)
        {
            errorText.Text = text;

            if (string.IsNullOrWhiteSpace(text))
            {
                errorText.Visibility = Visibility.Collapsed;
                UpdateWindowHeight(0);
            }
            else
            {
                errorText.Visibility = Visibility.Visible;
                errorText.UpdateLayout();
                UpdateWindowHeight(errorText.ActualHeight);
            }
        }

        internal static BaseDomainAccount OpenWindow(string textToFind, Window parentWindow, string helpText = null, string preferedDomain = null)
        {
            using (var window = new FindInOtherDomainWindow())
            {
                window.Owner = parentWindow;
                window.InitWithScope(textToFind, helpText, preferedDomain);
                window.ShowDialog();

                if (window.result != null)
                {
                    var scope = new Scope(window.result.Domain);

                    CollectionContainer.AvailableComputerScopes.AddItem(scope);
                    CollectionContainer.AvailableUserScopes.AddItem(scope);

                    if (!WaitScopeDataRetrieving.ShowWindow(window.result, parentWindow))
                        return null;
                }

                return (window.result == null) ? null : window.result.Domain;
            }
        }

        private void SizeChangedHandler(object sender, SizeChangedEventArgs e)
        {
            if (Math.Abs(baseHeight - 0) < 0.1)
                baseHeight = MinHeight = MaxHeight = ActualHeight;
        }

        private void UpdateWindowHeight(double height)
        {
            MinHeight = MaxHeight = baseHeight + height;
        }
    }
}
