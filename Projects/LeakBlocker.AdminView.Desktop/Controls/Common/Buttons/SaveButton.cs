using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using LeakBlocker.AdminView.Desktop.Generated;
using LeakBlocker.AdminView.Desktop.Resources;
using LeakBlocker.Libraries.Common.Resources;

namespace LeakBlocker.AdminView.Desktop.Controls.Common.Buttons
{
    internal sealed class SaveButton : TextAndImageButton
    {
        public SaveButton()
        {
            Text = AdminViewResources.Save;
            ImageTemplate = ButtonSave.ImageTemplate;
        }
    }
}
