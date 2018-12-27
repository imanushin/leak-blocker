using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using LeakBlocker.AdminView.Desktop.Common.Collections;
using LeakBlocker.AdminView.Desktop.Controls.Common.AccountWindows;
using LeakBlocker.AdminView.Desktop.Controls.Common.Converters;
using LeakBlocker.AdminView.Desktop.Resources;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Resources;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.AdminView.Desktop.Controls.Common.ScopeSelections
{
    internal abstract class AbstractScopeSelectionWindow : AbstractSelectionWindow
    {
        private readonly FilteredCollection<Scope> items;

        protected AbstractScopeSelectionWindow(
                ScopeCollection baseScope,
                ReadOnlySet<Scope> excludedScopes,
                string pleaseEnterText,
                string examples = null)
            : base(pleaseEnterText, ScopeToImageConverter.ImageForScope, examples)
        {
            baseScope.UpdateAsync(Dispatcher);

            items = baseScope.Where(item => !excludedScopes.Contains(item));

            SetItemsSource(items);

            UserInputFinished += InputFinished;
            FindInOtherLocationClicked += FindItemInOtherDomain;
        }

        protected Scope ResultScope
        {
            get;
            private set;
        }

        protected sealed override string GetHint(object searchObject)
        {
            var scope = (Scope)searchObject;

            return scope.ScopeType.GetValueDescription();
        }

        private void InputFinished()
        {
            string text = SearchText;

            Scope result = items.TryFind(text);

            if (result != null)
            {
                CloseControl(result);

                return;
            }

            FindItemInOtherDomain();
        }

        private void FindItemInOtherDomain()
        {
            BaseDomainAccount result = FindInOtherDomainWindow.OpenWindow(GenerateDescriptionText(SearchText), this);

            if (result != null)
            {
                items.UpdateAsync(Dispatcher);
            }

            MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }

        protected void SetPreferredScope(Scope scope)
        {
            SearchText = scope.Name;
        }

        private static string GenerateDescriptionText(string textToFind)
        {
            if (string.IsNullOrWhiteSpace(textToFind))
                return AdminViewResources.PleaseEnterDomain;

            ReadOnlySet<BaseDomainAccount> domains = UiObjects.DomainCache.AllDomains();

            if (domains.Any())
            {
                string joinedDomains = string.Join(", ", domains);

                return AdminViewResources.UnableToFindObjectInDomains.Combine(textToFind, joinedDomains);
            }

            return AdminViewResources.UnableToFindObject.Combine(textToFind);
        }

        private void CloseControl(Scope scope)
        {
            ResultScope = scope;

            Close();
        }
    }
}
