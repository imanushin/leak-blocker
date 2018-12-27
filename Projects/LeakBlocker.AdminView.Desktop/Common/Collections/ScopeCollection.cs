using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using LeakBlocker.Libraries.Common;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.AdminView.Desktop.Common.Collections
{
    internal abstract class ScopeCollection : SortedObservableCollection<Scope>
    {
    }
}
