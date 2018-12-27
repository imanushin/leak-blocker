using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.AdminView.Desktop.Common.Collections
{
    internal interface IChangingEnumerable<out T> : IEnumerable<T>, INotifyCollectionChanged
    {
    }
}
