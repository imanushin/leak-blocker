using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Threading;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.AdminView.Desktop.Common.Collections
{
    internal sealed class FilteredCollection<T> : IChangingEnumerable<T>
        where T : IComparable<T>
    {
        private readonly Func<T, bool> filter;
        private readonly SortedObservableCollection<T> baseCollection;

        public FilteredCollection(SortedObservableCollection<T> baseCollection, Func<T, bool> filter)
        {
            this.filter = filter;
            this.baseCollection = baseCollection;

            baseCollection.CollectionChanged += BaseCollectionCollectionChanged;
        }

        private void BaseCollectionCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChanged != null)
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Enumerable.Where(baseCollection, filter).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public T TryFind(string scopeName)
        {
            string upperName = scopeName.ToUpperInvariant();

            List<T> validItems = this.Where(item => item.ToString().ToUpperInvariant().Contains(upperName)).ToList();

            validItems.Sort(StringComparer);

            return validItems.FirstOrDefault();
        }

        private static int StringComparer(T left, T right)
        {
            string leftString = left.ToString();
            string rightString = right.ToString();

            if (leftString.Length != rightString.Length)
                return leftString.Length - rightString.Length;

            return string.CompareOrdinal(leftString, rightString);
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public void UpdateAsync(Dispatcher dispatcher)
        {
            baseCollection.UpdateAsync(dispatcher);
        }
    }
}
