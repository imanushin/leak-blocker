using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Animation;

namespace LeakBlocker.AdminView.Desktop.Controls.Common.Animations
{
    internal sealed partial class WaitIndicatorControl
    {
        public WaitIndicatorControl()
        {
            InitializeComponent();

            var animation = (Storyboard)Resources["UpdatingAnimation"];

            animation.Begin();
        }
    }
}
