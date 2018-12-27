using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common.Collections
{
    /// <summary>
    /// Generic immutable collection.
    /// </summary>
    /// <typeparam name="T">The type of the items in the collection.</typeparam>
    public interface IReadOnlyCollection<out T> : IEnumerable<T>
    {
        /// <summary>
        /// Количество элементов
        /// </summary>
        int Count
        {
            get;
        }
    }
}
