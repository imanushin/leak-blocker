using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.AdminView.Desktop.Common.Collections
{
    internal static class CollectionContainer
    {
        public static readonly SortedObservableCollection<UiComputer> AvailableComputers = new SortedObservableCollection<UiComputer>();

        public static readonly ScopeCollection AvailableComputerScopes = new ScopeCollection();
    }
}
