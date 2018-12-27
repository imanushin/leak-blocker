using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using LeakBlocker.AdminView.Desktop.Resources;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.License;
using LeakBlocker.Libraries.Common.Resources;

namespace LeakBlocker.AdminView.Desktop.Controls.Standard.License
{
    internal sealed partial class RequestCodeDialog
    {
        public RequestCodeDialog()
        {
            InitializeComponent();

            code.Focus();
        }

        private void OkClickedHandler(object sender, RoutedEventArgs e)
        {
            TryAcceptUserData();
        }

        private void TryAcceptUserData()
        {
            try
            {
                var license = BaseObjectSerializer.DeserializeFromXml<LicenseInfo>(Convert.FromBase64String(code.Text));

                license.CheckLicense();
            }
            catch (Exception ex)
            {
                Log.Write(ex);

                MessageBox.Show(this, AdminViewResources.LicenseIsInvalid, CommonStrings.ProductName, MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            DialogResult = true;
        }

        private void CancelClickedHandler(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        public static LicenseInfo RequestCode(Window owner)
        {
            var window = new RequestCodeDialog();
            window.Owner = owner;

            bool? result = window.ShowDialog();

            if (true == result)
                return BaseObjectSerializer.DeserializeFromXml<LicenseInfo>(Convert.FromBase64String(window.code.Text));

            return null;
        }

        private void CodeTextKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                TryAcceptUserData();
        }
    }
}
