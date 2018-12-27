using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LeakBlocker.AdminView.Desktop.Common;
using LeakBlocker.AdminView.Desktop.Common.Collections;
using LeakBlocker.AdminView.Desktop.Controls.Common;
using LeakBlocker.AdminView.Desktop.Controls.Common.Converters;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Resources;

namespace LeakBlocker.AdminView.Desktop.Controls.Standard.Audit
{
    internal sealed class DeviceSelectionWindow : AbstractSelectionWindow
    {
        private readonly AuditDevicesCollection devices = CollectionContainer.AuditDevicesCollection;

        private DeviceDescription result;

        private DeviceSelectionWindow()
            : base(AuditStrings.PleaseEnterDevice, DeviceToImageConverter.GetTemplate)
        {
            CanFindInOtherLocation = false;

            SetItemsSource(devices);

            devices.UpdateAsync(Dispatcher);

            UserInputFinished += UserInputFinishedHandler;
        }

        private void UserInputFinishedHandler()
        {
            string text = SearchText;

            if(string.IsNullOrWhiteSpace(text))
                return;

            DeviceDescription enteredItem = devices.FirstOrDefault(device => string.Equals(device.FriendlyName, text, StringComparison.OrdinalIgnoreCase));

            if (enteredItem == null)
                return;

            result = enteredItem;

            Close();
        }

        protected override string GetHint(object searchObject)
        {
            var device = (DeviceDescription)searchObject;

            return device.FriendlyName;
        }

        [CanBeNull]
        public static DeviceDescription OpenWindow(Window owner)
        {
            var window = new DeviceSelectionWindow();

            window.Owner = owner;

            window.ShowDialog();

            return window.result;
        }
    }
}
