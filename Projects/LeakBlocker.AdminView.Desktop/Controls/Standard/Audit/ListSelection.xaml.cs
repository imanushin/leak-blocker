using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LeakBlocker.AdminView.Desktop.Controls.Common.ScopeSelections;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.AdminView.Desktop.Controls.Standard.Audit
{
    internal sealed partial class ListSelection
    {
        public event Action FilterChanged;

        public ListSelection()
        {
            InitializeComponent();

            usersView.AddClicked += UsersViewAddClicked;
            computersView.AddClicked += ComputersViewAddClicked;
            devicesView.AddClicked += DevicesViewAddClicked;

            usersView.ItemRemoved += RaiseFilterChanged;
            computersView.ItemRemoved += RaiseFilterChanged;
            devicesView.ItemRemoved += RaiseFilterChanged;
        }

        private void DevicesViewAddClicked()
        {
            DeviceDescription device = DeviceSelectionWindow.OpenWindow(Window.GetWindow(this));

            if (device == null)
                return;

            devicesView.AppendItem(device);

            RaiseFilterChanged();
        }

        private void ComputersViewAddClicked()
        {
            BaseComputerAccount computer = ComputerSelectionWindow.OpenWindow(Window.GetWindow(this));

            if (computer == null)
                return;

            computersView.AppendItem(computer);

            RaiseFilterChanged();
        }

        private void UsersViewAddClicked()
        {
            BaseUserAccount user = UserSelectionWindow.OpenWindow(Window.GetWindow(this));

            if (user == null)
                return;

            usersView.AppendItem(user);

            RaiseFilterChanged();
        }

        private void RaiseFilterChanged()
        {
            if (FilterChanged != null)
                FilterChanged();
        }

        public ICollection<BaseUserAccount> Users
        {
            get
            {
                return usersView.Items.Cast<BaseUserAccount>().ToReadOnlySet();
            }
            set
            {
                usersView.SetItems(value);
            }
        }

        public ICollection<BaseComputerAccount> Computers
        {
            get
            {
                return usersView.Items.Cast<BaseComputerAccount>().ToReadOnlySet();
            }
            set
            {
                computersView.SetItems(value);
            }
        }

        public ICollection<DeviceDescription> Devices
        {
            get
            {
                return usersView.Items.Cast<DeviceDescription>().ToReadOnlySet();
            }
            set
            {
                devicesView.SetItems(value);
            }
        }
    }
}
