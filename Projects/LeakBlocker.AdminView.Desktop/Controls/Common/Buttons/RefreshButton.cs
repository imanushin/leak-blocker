using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using LeakBlocker.AdminView.Desktop.Generated;
using LeakBlocker.AdminView.Desktop.Resources;

namespace LeakBlocker.AdminView.Desktop.Controls.Common.Buttons
{
    internal sealed class RefreshButton : TextAndImageButton
    {
        public RefreshButton() 
        {
            Text = AdminViewResources.Refresh;
            ImageTemplate = LargeButtonRefresh.ImageTemplate;
        }
    }
}
