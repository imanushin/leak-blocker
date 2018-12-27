using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Animation;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.AdminView.Desktop.Controls.Common.Animations
{
    internal sealed partial class BusyIndicator
    {
        private bool isBusy = false;

        public BusyIndicator()
        {
            InitializeComponent();
        }

        public bool IsBusy
        {
            get
            {
                return isBusy;
            }
            set
            {
                if (isBusy == value)
                    return;

                isBusy = value;

                if (isBusy)
                    StartWaiting();
                else
                    StopWaiting();
            }
        }

        private void StartWaiting()
        {
            var animation = (Storyboard)Resources["Animation"];

            animation.Begin();

            Visibility = Visibility.Visible;
        }

        private void StopWaiting()
        {
            var animation = (Storyboard)Resources["Animation"];

            animation.Stop();

            Visibility = Visibility.Collapsed;
        }
    }
}
