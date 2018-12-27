using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Resources;

namespace LeakBlocker.AdminView.Desktop.Controls.Standard.Views
{
    [EnumerationDescriptionProvider(typeof(DeviceDescriptionStateStrings))]
    internal enum DeviceDescriptionState
    {
        Blocked,
        Allowed,
        PartiallyBlocked,
        TemporaryAllowed
    }

    internal sealed class DeviceView : INotifyPropertyChanged, IComparable<DeviceView>
    {
        public DeviceView(DeviceAccessMap map, DeviceDescription targetDevice)
        {
            TargetDevice = targetDevice;
            State = GetDeviceState(map);
        }

        public void UpdateDevice(DeviceAccessMap map, DeviceDescription newDevice)
        {
            Check.ObjectIsNotNull(newDevice, "newDevice");

            if (!TargetDevice.Equals(newDevice))
                throw new ArgumentException("Argument should be equals with {0}".Combine(TargetDevice), "newDevice");

            TargetDevice = newDevice;

            DeviceDescriptionState newState = GetDeviceState(map);

            if (State != newState)
            {
                State = newState;
                OnPropertyChanged("State");
                OnPropertyChanged("Description");
            }

            OnPropertyChanged("Name");
        }

        public DeviceDescription TargetDevice
        {
            get;
            private set;
        }

        [UsedImplicitly]
        public string Name
        {
            get
            {
                return TargetDevice.FriendlyName;
            }
        }

        public DeviceDescriptionState State
        {
            get;
            private set;
        }

        [UsedImplicitly]
        public string Description
        {
            get
            {
                return State.GetValueDescription();
            }
        }

        private DeviceDescriptionState GetDeviceState(DeviceAccessMap map)
        {
            ReadOnlySet<DeviceAccessType> allStates = map.Keys1.Select(user => map[user, TargetDevice]).ToReadOnlySet();

            if (allStates.Count == 1)
                return ConvertFromAttachedDeviceState(allStates.First());

            return DeviceDescriptionState.PartiallyBlocked;
        }

        private static DeviceDescriptionState ConvertFromAttachedDeviceState(DeviceAccessType value)
        {
            switch (value)
            {
                case DeviceAccessType.Blocked:
                    return DeviceDescriptionState.Blocked;

                case DeviceAccessType.ReadOnly:
                    return DeviceDescriptionState.PartiallyBlocked;

                case DeviceAccessType.AllowedNotLicensed:
                case DeviceAccessType.Allowed:
                    return DeviceDescriptionState.Allowed;

                case DeviceAccessType.TemporarilyAllowedReadOnly:
                case DeviceAccessType.TemporarilyAllowed:
                    return DeviceDescriptionState.TemporaryAllowed;

                default:
                    throw new ArgumentOutOfRangeException("value", "Enum {0} is not supported".Combine(value));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public int CompareTo(DeviceView other)
        {
            Check.ObjectIsNotNull(other);

            return string.CompareOrdinal(Name, other.Name);
        }

        public override string ToString()
        {
            return TargetDevice.ToString();
        }

        public override bool Equals(object obj)
        {
            var other = obj as DeviceView;

            if (other == null)
                return false;

            return other.TargetDevice == TargetDevice;
        }

        public override int GetHashCode()
        {
            return TargetDevice.GetHashCode();
        }
    }
}
