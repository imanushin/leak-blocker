using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LeakBlocker.AdminView.Desktop.Common.Collections;
using LeakBlocker.AdminView.Desktop.Resources;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Resources;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.AdminView.Desktop.Controls.Common.ScopeSelections
{
    internal sealed class UserSelectionWindow : AbstractScopeSelectionWindow
    {
        private UserSelectionWindow()
            : base(CollectionContainer.AvailableUsers, ReadOnlySet<Scope>.Empty, AdminViewResources.PleaseEnterUserName)
        {
        }

        public static BaseUserAccount OpenWindow(Window owner)
        {
            var window = new UserSelectionWindow();

            window.Owner = owner;

            window.ShowDialog();

            Scope result = window.ResultScope;

            if (result == null)
                return null;

            return (BaseUserAccount)result.TargetObject;
        }
    }
}
