using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.AdminView.Desktop.Controls.Common
{
    internal sealed partial class DynamicTabControl
    {
        public event Action AddTabClicked;

        public DynamicTabControl()
        {
            InitializeComponent();
        }

        private void AddTabEventHandler(object sender, RoutedEventArgs e)
        {
            if (AddTabClicked != null)
                AddTabClicked();
        }
    }
}
