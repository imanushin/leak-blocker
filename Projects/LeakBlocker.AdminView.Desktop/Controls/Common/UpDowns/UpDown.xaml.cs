using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.AdminView.Desktop.Controls.Common.UpDowns
{
    internal sealed partial class UpDown
    {
        public static readonly RoutedEvent UpClickedEvent = EventManager.RegisterRoutedEvent(
        "UpClicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UpDown));

        public static readonly RoutedEvent DownClickedEvent = EventManager.RegisterRoutedEvent(
        "DownClicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UpDown));

        public event RoutedEventHandler UpClicked
        {
            add
            {
                AddHandler(UpClickedEvent, value);
            }
            remove
            {
                RemoveHandler(UpClickedEvent, value);
            }
        }

        public event RoutedEventHandler DownClicked
        {
            add
            {
                AddHandler(DownClickedEvent, value);
            }
            remove
            {
                RemoveHandler(DownClickedEvent, value);
            }
        }

        public UpDown()
        {
            InitializeComponent();
        }

        private void IncreaseValue(object sender, RoutedEventArgs e)
        {
            var newEventArgs = new RoutedEventArgs(UpClickedEvent);
            RaiseEvent(newEventArgs);
        }

        private void DescreaseValue(object sender, RoutedEventArgs e)
        {
            var newEventArgs = new RoutedEventArgs(DownClickedEvent);
            RaiseEvent(newEventArgs);
        }
    }
}
