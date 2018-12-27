using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LeakBlocker.AdminView.Desktop.Controls.Common.ScopeSelections;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.AdminView.Desktop.Controls.Simple
{
    internal sealed partial class UsersTab
    {
        public UsersTab()
        {
            InitializeComponent();
        }

        internal void Init(SimpleConfiguration inputOptions)
        {
            inputOptions.UsersWhiteList.ForEach(item => users.Items.Add(item));
        }

        internal ReadOnlySet<Scope> Users
        {
            get
            {
                return users.Items.Cast<Scope>().ToReadOnlySet();
            }
        }

        private void AddButtonClicked(object sender, RoutedEventArgs e)
        {
            Scope selectedScope = UserScopeSelectionWindow.OpenSelectionWindow(Users, Window.GetWindow(this));

            if (selectedScope != null)
                users.Items.Add(selectedScope);
        }
    }
}
