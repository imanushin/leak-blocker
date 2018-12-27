using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.AdminView.Desktop.Generated;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Resources;

namespace LeakBlocker.AdminView.Desktop.Controls.Common.Buttons
{
    internal sealed class OkButton : TextAndImageButton
    {
        public OkButton()
        {
            Text = CommonStrings.Ok;
            ImageTemplate = ButtonOk.ImageTemplate;
        }
    }
}
