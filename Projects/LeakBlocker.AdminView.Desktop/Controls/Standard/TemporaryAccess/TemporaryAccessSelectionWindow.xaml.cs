using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Entities.Settings.TemporaryAccess;
using LeakBlocker.Libraries.Common.Resources;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.AdminView.Desktop.Controls.Standard.TemporaryAccess
{
    internal sealed partial class TemporaryAccessSelectionWindow
    {
        private readonly ReadOnlySet<RadioButton> hardcodedButtons;

        private TemporaryAccessSelectionWindow(Window owner)
        {
            InitializeComponent();

            Owner = owner;

            Time now = Time.Now;

            minutes15Button.Tag = now.Add(TimeSpan.FromMinutes(15));
            minutes30Button.Tag = now.Add(TimeSpan.FromMinutes(30));
            hour1Button.Tag = now.Add(TimeSpan.FromHours(1));
            customEndDate.Value = now.ToUtcDateTime().AddHours(10).ToLocalTime();

            foreverButton.Tag = new Time(DateTime.MaxValue.Subtract(TimeSpan.FromDays(2)).Ticks); //Чтобы избежать ошибок переполнения при смене часового пояса

            hardcodedButtons = new ReadOnlySet<RadioButton>(new[] { minutes15Button, minutes30Button, hour1Button, foreverButton });

            SimpleConfiguration settings = UiObjects.UiConfigurationManager.Configuration;

            if (!settings.IsReadOnlyAccessEnabled)
                continueWriteAccessLock.Visibility = Visibility.Visible;
        }

        private Time GetSelectedTime()
        {
            if (true == customButton.IsChecked)
            {
                Check.ObjectIsNotNull(customEndDate.Value);

                return new Time(customEndDate.Value);
            }

            foreach (RadioButton button in hardcodedButtons)
            {
                if (true == button.IsChecked)
                    return (Time)button.Tag;
            }

            throw new InvalidOperationException("Nothing is checked");
        }

        public static IEnumerable<DeviceTemporaryAccessCondition> ShowDialogForDevices(
            ICollection<DeviceDescription> devices,
            Window parent)
        {
            Check.CollectionHasOnlyMeaningfulData(devices, "devices");

            var window = new TemporaryAccessSelectionWindow(parent);

            window.devicesTitle.Visibility = Visibility.Visible;
            
            if (devices.Count == 1)
            {
                window.devicesTitle.Text = TemporaryAccessStrings.GiveAccessForDevice.Combine(devices.First());
            }
            else
            {
                string devicesString = string.Join(", ", devices);

                window.devicesTitle.Text = TemporaryAccessStrings.GiveAccessForDevices.Combine(devicesString);
            }

            bool? result = window.ShowDialog();

            if (true != result)
                return ReadOnlySet<DeviceTemporaryAccessCondition>.Empty;

            var accessConditions = new List<DeviceTemporaryAccessCondition>();

            foreach (DeviceDescription device in devices)
            {
                accessConditions.Add(
                    new DeviceTemporaryAccessCondition(
                        device,
                        window.GetSelectedTime(),
                        true == window.continueWriteAccessLock.IsChecked));
            }

            return accessConditions.ToReadOnlySet();

        }

        public static IEnumerable<BaseTemporaryAccessCondition> ShowDialogForUsers(ICollection<BaseUserAccount> users, Window parent)
        {
            Check.CollectionHasOnlyMeaningfulData(users, "users");

            var window = new TemporaryAccessSelectionWindow(parent);

            window.usersTitle.Visibility = Visibility.Visible;
            
            if (users.Count == 1)
            {
                window.usersTitle.Text = TemporaryAccessStrings.GiveAccessForUser.Combine(users.First());
            }
            else
            {
                string usersString = string.Join(", ", users);

                window.usersTitle.Text = TemporaryAccessStrings.GiveAccessForUsers.Combine(usersString);
            }

            bool? result = window.ShowDialog();

            if (true != result)
                return ReadOnlySet<BaseTemporaryAccessCondition>.Empty;

            var accessConditions = new List<BaseTemporaryAccessCondition>();

            foreach (BaseUserAccount user in users)
            {
                accessConditions.Add(
                    new UserTemporaryAccessCondition(
                        user,
                        window.GetSelectedTime(),
                        true == window.continueWriteAccessLock.IsChecked));
            }

            return accessConditions.ToReadOnlySet();
        }

        public static IEnumerable<ComputerTemporaryAccessCondition> ShowDialogForComputers(
            ICollection<BaseComputerAccount> computers,
            Window parent)
        {
            Check.CollectionHasOnlyMeaningfulData(computers, "computers");

            var window = new TemporaryAccessSelectionWindow(parent);

            window.computersTitle.Visibility = Visibility.Visible;

            if (computers.Count == 1)
            {
                window.computersTitle.Text = TemporaryAccessStrings.GiveAccessForComputer.Combine(computers.First());
            }
            else
            {
                string computersString = string.Join(", ", computers);

                window.computersTitle.Text = TemporaryAccessStrings.GiveAccessForComputers.Combine(computersString);
            }

            bool? result = window.ShowDialog();

            if (true != result)
                return ReadOnlySet<ComputerTemporaryAccessCondition>.Empty;

            var accessConditions = new List<ComputerTemporaryAccessCondition>();

            foreach (BaseComputerAccount computer in computers)
            {
                accessConditions.Add(
                    new ComputerTemporaryAccessCondition(
                        computer,
                        window.GetSelectedTime(),
                        true == window.continueWriteAccessLock.IsChecked));
            }

            return accessConditions.ToReadOnlySet();
        }

        private void OkButtonClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CancelButtonClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
