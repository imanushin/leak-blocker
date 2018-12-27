using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Resources;

namespace LeakBlocker.AdminView.Desktop.Controls.Common
{
    internal enum SizeRestriction
    {
        None,
        MinValue,
        Both
    }

    internal abstract class ToolWindow : Window
    {
        private bool isSizeSetAsFixed;

        protected ToolWindow()
        {
            Style = (Style) Application.Current.Resources["DefaultWindowStyle"];
            RestrictHeight = SizeRestriction.Both;
            RestrictWidth = SizeRestriction.MinValue;
            SizeToContent = SizeToContent.WidthAndHeight;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            WindowStyle = WindowStyle.ToolWindow;

            Title = CommonStrings.ProductName;

            SizeChanged += ToolWindowSizeChanged;
            KeyUp += ToolWindowKeyUp;
        }

        private void ToolWindowKeyUp(object sender, KeyEventArgs e)
        {
            if (!e.Handled && e.Key == Key.Escape)
            {
                Close();

                e.Handled = true;
            }
        }

        private void ToolWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (isSizeSetAsFixed)
                return;

            if (RestrictHeight == SizeRestriction.Both)
                MaxHeight = MinHeight = ActualHeight;
            else if (RestrictHeight == SizeRestriction.MinValue)
                MinHeight = ActualHeight;


            if (RestrictWidth == SizeRestriction.Both)
                MaxWidth = MinWidth = ActualWidth;
            else if (RestrictWidth == SizeRestriction.MinValue)
                MinWidth = ActualWidth;

            isSizeSetAsFixed = true;
        }

        public SizeRestriction RestrictHeight
        {
            get;
            set;
        }

        public SizeRestriction RestrictWidth
        {
            get;
            set;
        }
    }
}
