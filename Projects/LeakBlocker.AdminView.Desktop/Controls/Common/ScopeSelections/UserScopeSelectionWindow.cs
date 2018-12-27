using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LeakBlocker.AdminView.Desktop.Common.Collections;
using LeakBlocker.AdminView.Desktop.Resources;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Resources;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.AdminView.Desktop.Controls.Common.ScopeSelections
{
    internal sealed class UserScopeSelectionWindow : AbstractScopeSelectionWindow
    {
        private UserScopeSelectionWindow(ReadOnlySet<Scope> excludedScopes) :
            base(
            CollectionContainer.AvailableUserScopes,
            excludedScopes,
            AdminViewResources.PleaseEnterUserScope,
            AdminViewResources.UserScopeExamples)
        {
        }


        internal static Scope OpenSelectionWindow(ReadOnlySet<Scope> excludedScopes, Window owner)
        {
            var resultControl = new UserScopeSelectionWindow(excludedScopes);

            resultControl.Owner = owner;

            resultControl.ShowDialog();

            return resultControl.ResultScope;
        }
    }
}
