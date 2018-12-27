using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LeakBlocker.AdminView.Desktop.Common.Collections;
using LeakBlocker.Libraries.Common;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.AdminView.Desktop.Controls.Simple
{
    internal sealed partial class AdvancedOptionsWindow
    {
        public AdvancedOptionsWindow()
        {
            CollectionContainer.AvailableComputers.UpdateAsync(Dispatcher);

            InitializeComponent();
        }

        internal static SimpleConfiguration OpenWindow(SimpleConfiguration previousConfig, Window parentWindow)
        {
            var window = new AdvancedOptionsWindow();

            window.devices.Init(previousConfig);
            window.users.Init(previousConfig);
            window.Owner = parentWindow;

            bool? result = window.ShowDialog();

            if (true != result)
                return previousConfig;

            return new SimpleConfiguration(
               window.devices.IsBlockEnabled,
               window.devices.IsReadonlyAccessEnabled,
               window.devices.AreInputDevicesAllowed,
               window.devices.IsFileAuditEnabled,
               previousConfig.BlockedScopes,
               previousConfig.ExcludedScopes,
               window.devices.ExcludedDevices,
               window.users.Users,
               previousConfig.TemporaryAccess);
        }

        private void CancelClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = null;
            Close();
        }

        private void OkClicked(object sender, RoutedEventArgs e)
        {
            if( !reports.CheckIsValidAndNavigate())
                return;

            reports.SaveSettingsAsync();

            DialogResult = true;
            Close();
        }
    }
}
