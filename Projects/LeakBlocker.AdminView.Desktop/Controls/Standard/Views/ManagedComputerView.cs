using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using LeakBlocker.AdminView.Desktop.Common.Collections;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.AdminView.Desktop.Controls.Standard.Views
{
    internal sealed class ManagedComputerView : INotifyPropertyChanged
    {
        private ManagedComputer computer;
        private readonly ObservableCollection<BaseUserAccount> users = new ObservableCollection<BaseUserAccount>();
        private readonly SortedObservableCollection<DeviceView> devices = new SortedObservableCollection<DeviceView>();

        public ManagedComputerView(ManagedComputer computer)
        {
            Check.ObjectIsNotNull(computer, "computer");

            UpdateComputer(computer);
        }

        public ManagedComputer Computer
        {
            get
            {
                return computer;
            }
            set
            {
                Check.ObjectIsNotNull(value, "value");

                if (computer != value)
                    throw new ArgumentException("Argument should be equals with {0}".Combine(computer), "value");

                UpdateComputer(value);
            }
        }

        private void UpdateComputer(ManagedComputer value)
        {
            Check.ObjectIsNotNull(value, "value");

            ManagedComputer oldValue = computer;

            computer = value;

            var oldUsers = users.ToReadOnlySet();
            var newUsers = computer.Data.LastLockMap.Keys1.ToReadOnlySet();

            if (!oldUsers.Equals(newUsers))
            {
                users.Clear();

                users.AddRange(computer.Data.LastLockMap.Keys1);
            }

            ReadOnlySet<DeviceView> oldDevices = devices.ToReadOnlySet();
            ReadOnlySet<DeviceDescription> newDevices = computer.Data.LastLockMap.Keys2.ToReadOnlySet();

            devices.RemoveItems(oldDevices.Where(device => !newDevices.Contains(device.TargetDevice)));

            foreach (DeviceDescription newDevice in newDevices)
            {
                var oldItem = devices.FirstOrDefault(device => Equals(device.TargetDevice, newDevice));

                if (oldItem != null)
                    oldItem.UpdateDevice(computer.Data.LastLockMap, newDevice);
                else
                    devices.AddItem(new DeviceView(computer.Data.LastLockMap, newDevice));
            }

            if (oldValue != null)
            {
                if (string.Equals(oldValue.DnsName, computer.DnsName, StringComparison.Ordinal))
                    OnPropertyChanged("Name");

                if (oldUsers != newUsers)
                    OnPropertyChanged("Users");

                if (oldDevices != newDevices)
                    OnPropertyChanged("Devices");

                if (oldValue.Data.Status != computer.Data.Status)
                    OnPropertyChanged("Status");
            }
        }

        [UsedImplicitly]//Из WPF биндинга
        public ManagedComputerStatus Status
        {
            get
            {
                return computer.Data.Status;
            }
        }

        [UsedImplicitly]//Из WPF биндинга
        public string Name
        {
            get
            {
                return computer.DnsName;
            }
        }

        public ObservableCollection<BaseUserAccount> Users
        {
            get
            {
                return users;
            }
        }

        public SortedObservableCollection<DeviceView> Devices
        {
            get
            {
                return devices;
            }
        }

        public override int GetHashCode()
        {
            return computer.GetHashCode();
        }

        public override bool Equals(object target)
        {
            var other = target as ManagedComputerView;

            if (other == null)
                return false;

            return computer.Equals(other.computer);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}