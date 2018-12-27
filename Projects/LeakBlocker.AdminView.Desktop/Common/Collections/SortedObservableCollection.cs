using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Windows.Threading;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;

namespace LeakBlocker.AdminView.Desktop.Common.Collections
{
    [DebuggerDisplay("Count = {Count}")]
    internal class SortedObservableCollection<T> : IChangingEnumerable<T>
        where T : IComparable<T>
    {
        private readonly HashSet<T> internalCollection = new HashSet<T>();
        private readonly object syncRoot = new object();

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private static int Compare(T left, T right)
        {
            return left.CompareTo(right);
        }

        public void Update(IEnumerable<T> source)
        {
            lock (syncRoot)
            {
                ReadOnlySet<T> items = source.ToReadOnlySet();

                if (items.Equals(internalCollection.ToReadOnlySet()))
                    return;

                internalCollection.Clear();

                internalCollection.AddRange(items);

                NotifyCollectionChanged();
            }
        }

        public void RemoveItems(IEnumerable<T> items)
        {
            var itemsToRemove = items.ToReadOnlySet();

            if (!itemsToRemove.Any()) //Избегаем перерисовки, если ничего не поменялось
                return;

            lock (syncRoot)
            {
                foreach (T item in items)
                {
                    internalCollection.Remove(item);
                }

                NotifyCollectionChanged();
            }
        }

        public int Count
        {
            get
            {
                return internalCollection.Count;
            }
        }

        public void AddItem(T item)
        {
            AddItems(new[] { item });
        }

        protected void AddItems(IEnumerable<T> items)
        {
            lock (syncRoot)
            {
                List<T> newItems = items.Where(item => !internalCollection.Contains(item)).ToList();

                if (newItems.Count == 0)
                    return;

                internalCollection.AddRange(newItems);

                NotifyCollectionChanged();
            }
        }

        private void NotifyCollectionChanged()
        {
            if (CollectionChanged == null)
                return;

            var arg = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);

            CollectionChanged(this, arg);
        }

        public FilteredCollection<T> Where(Func<T, bool> condition)
        {
            return new FilteredCollection<T>(this, condition);
        }

        public IEnumerator<T> GetEnumerator()
        {
            List<T> sorted;

            lock (syncRoot)
            {
                sorted = internalCollection.ToList();
            }

            sorted.Sort(Compare);

            return sorted.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            List<T> sorted;

            lock (syncRoot)
            {
                sorted = internalCollection.ToList();
            }

            sorted.Sort(Compare);

            return sorted.GetEnumerator();
        }

        protected virtual void Update(Dispatcher dispatcher)
        {
        }

        public void UpdateAsync(Dispatcher dispatcher)
        {
            SharedObjects.AsyncInvoker.Invoke(Update, dispatcher);
        }
    }
}
