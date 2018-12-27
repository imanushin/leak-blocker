using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using LeakBlocker.Libraries.Common;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.AdminView.Desktop.Controls.Common.Email
{
    internal sealed partial class KnownAccountSelector
    {
        public delegate EmailSettings CreateSettings(string email, string password);
        
        private KnownAccountSelector(string title, string emailSuffix)
        {
            InitializeComponent();

            Title = title;
            email.Text = '@' + emailSuffix;

            email.Focus();
        }

        public static EmailSettings RequestSettings(string title, string emailSuffix, CreateSettings creator, Window owner)
        {
            var dialog = new KnownAccountSelector(title, emailSuffix);

            dialog.Owner = owner;

            bool? result = dialog.ShowDialog();

            if (result != true)
                return null;

            return creator(dialog.email.Text, dialog.password.Password);
        }

        private void OkClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CancelClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void CommonKeyUpHandler(object sender, KeyEventArgs e)
        {
            if( e.Key == Key.Enter )
                DialogResult = true;
        }
    }
}
