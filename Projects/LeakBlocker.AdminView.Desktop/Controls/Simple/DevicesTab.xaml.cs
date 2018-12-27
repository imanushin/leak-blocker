using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using LeakBlocker.AdminView.Desktop.Common;
using LeakBlocker.AdminView.Desktop.Common.Collections;
using LeakBlocker.AdminView.Desktop.Controls.Common.ScopeSelections;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Resources;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.AdminView.Desktop.Controls.Simple
{
    internal sealed partial class DevicesTab
    {
        private readonly SortedObservableCollection<DeviceDescription> originalDevices = new SortedObservableCollection<DeviceDescription>();

        public DevicesTab()
        {
            InitializeComponent();

            originalDevices.CollectionChanged += (source, args) => excludedDevices.SetSourceItems(originalDevices);

            if (CollectionContainer.ServerDevices.Any())
            {
                originalDevices.Update(CollectionContainer.ServerDevices);
            }
            else
            {
                excludedDevices.IsRightPanelIsBusy = true;

                CollectionContainer.ServerDevices.CollectionChanged += ServerDevicesCollectionChanged;
            }
        }

        private void ServerDevicesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CollectionContainer.ServerDevices.CollectionChanged -= ServerDevicesCollectionChanged;

            originalDevices.Update(CollectionContainer.ServerDevices);

            excludedDevices.IsRightPanelIsBusy = false;
        }

        internal void Init(SimpleConfiguration config)
        {
            allowInputDevicesCheckBox.IsChecked = config.AreInputDevicesAllowed;
            blockRadioButton.IsChecked = config.IsBlockEnabled;
            allowRadioButton.IsChecked = !config.IsBlockEnabled;
            enableFileAuditCheckBox.IsChecked = config.IsFileAuditEnabled;
            allowReadonlyAccessCheckBox.IsChecked = config.IsReadOnlyAccessEnabled;
            excludedDevices.CurrentItems = config.ExcludedDevices;
        }

        public bool IsBlockEnabled
        {
            get
            {
                return GetBoolValue(blockRadioButton.IsChecked);
            }
        }

        public bool IsReadonlyAccessEnabled
        {
            get
            {
                return GetBoolValue(allowReadonlyAccessCheckBox.IsChecked);
            }
        }

        public bool AreInputDevicesAllowed
        {
            get
            {
                return GetBoolValue(allowInputDevicesCheckBox.IsChecked);
            }
        }

        public bool IsFileAuditEnabled
        {
            get
            {
                return GetBoolValue(enableFileAuditCheckBox.IsChecked);
            }
        }

        private static bool GetBoolValue(bool? original)
        {
            return true == original;
        }


        public ReadOnlySet<DeviceDescription> ExcludedDevices
        {
            get
            {
                return excludedDevices.CurrentItems.Cast<DeviceDescription>().ToReadOnlySet();
            }
        }
    }
}
