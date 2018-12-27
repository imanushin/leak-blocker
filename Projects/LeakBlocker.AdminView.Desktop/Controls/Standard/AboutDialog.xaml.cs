using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using LeakBlocker.AdminView.Desktop.Controls.Standard.License;
using LeakBlocker.AdminView.Desktop.Resources;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.License;
using LeakBlocker.Libraries.Common.Resources;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.AdminView.Desktop.Controls.Standard
{
    internal sealed partial class AboutDialog
    {
        private ReadOnlySet<LicenseInfo> licenses = ReadOnlySet<LicenseInfo>.Empty;
        private UserContactInformation userContactInformation = UserContactInformation.Empty;
        private int? computersInScope;

        public AboutDialog(int? computersInScope = null)
        {
            InitializeComponent();

            this.computersInScope = computersInScope;

            SharedObjects.AsyncInvoker.Invoke(UpdateLicenseData);
            SharedObjects.AsyncInvoker.Invoke(UpdateUserContactInformation);
        }

        private void UpdateUserContactInformation()
        {
            using (IAccountTools client = UiObjects.CreateAccountToolsClient())
            {
                userContactInformation = client.GetCurrentUserInformation();
            }
        }

        private void UpdateLicenseData()
        {
            using (ILicenseTools client = UiObjects.CreateLicenseToolsClient())
            {
                UpdateLicenseData(client);
            }
        }

        private void UpdateLicenseData(ILicenseTools client)
        {
            var newLicenses = client.GetAllActualLicenses();

            licenses = newLicenses;

            Dispatcher.BeginInvoke(new Action(() => UpdateLicenseTest(newLicenses)));
        }

        private void UpdateLicenseTest(ReadOnlySet<LicenseInfo> newLicenses)
        {
            if (newLicenses.Any(license => license.ExpirationDate != Time.Unknown))
            {
                var maxDate = newLicenses.Select(license => license.ExpirationDate).Where(date => date != Time.Unknown).Max();

                licenseData.Text = AdminViewResources.LicenseWillBeAvailableUntil.Combine(maxDate.ToUtcDateTime().ToShortDateString());

                return;
            }

            int result = 10;

            result += newLicenses.Sum(license => license.Count);

            licenseData.Text = AdminViewResources.License.Combine(result);
        }

        private void BuyHandler(object sender, RoutedEventArgs e)
        {
            LicenseInfo someLicense = licenses.FirstOrDefault();

            string companyName = someLicense == null ? string.Empty : someLicense.CompanyName;
            int availableLicenses = licenses.Sum(license => license.Count) + 10;
            int requiredLicenses = computersInScope.HasValue ? Math.Max(1, computersInScope.Value - availableLicenses) : 1;

            var userContact = userContactInformation;

            if (string.IsNullOrWhiteSpace(userContact.CompanyName) && !string.IsNullOrWhiteSpace(companyName))//Если в домене нет этих данных
                userContact = new UserContactInformation(userContact.FirstName, userContact.LastName, companyName, userContact.Email, userContact.Phone);

            var request = new LicenseRequestData(userContact, requiredLicenses);

            string link = SharedObjects.LicenseLinkManager.GetLink(request);

            Process.Start(link);
        }

        private void EnterCodeHandler(object sender, RoutedEventArgs e)
        {
            LicenseInfo license = RequestCodeDialog.RequestCode(this);

            if (license == null)
                return;

            busyIndicator.IsBusy = true;

            SharedObjects.AsyncInvoker.Invoke(ApplyLicense, license);
        }

        private void ApplyLicense(LicenseInfo license)
        {
            using (ILicenseTools client = UiObjects.CreateLicenseToolsClient())
            {
                client.AddLicense(license);

                UpdateLicenseData(client);

                Dispatcher.BeginInvoke(
                    new Action(
                        () =>
                        {
                            busyIndicator.IsBusy = false;
                        }));
            }
        }
    }
}
