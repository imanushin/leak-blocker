using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using LeakBlocker.Libraries.Common;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.AdminView.Desktop.Common.Collections
{
    internal sealed class AvailableUserScopesCollection : ScopeCollection
    {
        protected override void Update(Dispatcher dispatcher)
        {
            using (IAccountTools client = UiObjects.CreateAccountToolsClient())
            {
                List<Scope> scopes = client.GetAvailableUserScopes().ToList();

                dispatcher.BeginInvoke(new Action(() => AddItems(scopes)));
            }
        }
    }
}
