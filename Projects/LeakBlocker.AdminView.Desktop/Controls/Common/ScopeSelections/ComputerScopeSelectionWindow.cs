using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LeakBlocker.AdminView.Desktop.Common.Collections;
using LeakBlocker.AdminView.Desktop.Controls.Common.AccountWindows;
using LeakBlocker.AdminView.Desktop.Resources;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Resources;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.AdminView.Desktop.Controls.Common.ScopeSelections
{
    internal sealed class ComputerScopeSelectionWindow : AbstractScopeSelectionWindow
    {
        private static bool wasSuccesResult = false;

        private ComputerScopeSelectionWindow(ReadOnlySet<Scope> excludedScopes) :
            base(
            CollectionContainer.AvailableComputerScopes,
            excludedScopes,
            AdminViewResources.PleaseEnterComputerScope,
            AdminViewResources.ComputerScopeExamples)
        {
        }


        internal static Scope OpenSelectionWindow(ReadOnlySet<Scope> excludedScopes, Window owner)
        {
            Scope preferredScope = null;

            if (!UiObjects.UiConfigurationManager.WasPreviousConfig && !wasSuccesResult && !CollectionContainer.AvailableComputerScopes.Any())
            {
                BaseDomainAccount result = FindInOtherDomainWindow.OpenWindow(
                    AdminViewResources.FirstCredentialsEnterText,
                    owner,
                    AdminViewResources.FirstCredentialsEnterHelp,
                    App.PreferedDomain);

                if (result == null)
                    return null;

                wasSuccesResult = true;

                preferredScope = new Scope(result);
            }

            var resultControl = new ComputerScopeSelectionWindow(excludedScopes);

            resultControl.Owner = owner;

            if (preferredScope != null)
            {
                resultControl.SetPreferredScope(preferredScope);
            }

            resultControl.ShowDialog();

            return resultControl.ResultScope;
        }
    }
}
