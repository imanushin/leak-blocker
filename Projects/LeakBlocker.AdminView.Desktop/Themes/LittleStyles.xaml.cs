using LeakBlocker.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LeakBlocker.AdminView.Desktop.Themes
{
    internal sealed partial class Generics
    {
        public Generics()
        {
            InitializeComponent();
        }

        private void TransparentButtonLoadedHandler(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;

            var targetBorder = (Border)button.Template.FindName("Bd",button);

            if (targetBorder == null)
                return;

            targetBorder.BorderBrush = Brushes.Transparent;
            targetBorder.BorderThickness = new Thickness(0);
        }
    }
}
