using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.AdminView.Desktop.Controls.Standard.SettingsChangeActions
{
    internal abstract class BaseChangeAction : BaseReadOnlyObject
    {
        public abstract string ShortText
        {
            get;
        }

        public abstract SimpleConfiguration AddSettings(SimpleConfiguration originalConfiguration);
    }
}
