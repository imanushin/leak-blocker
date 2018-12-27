using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common.Collections
{
    /// <summary>
    /// Вытесняющий кеш. На вход подается максимальное количество элементов.
    /// Класс является потокобезопасным и не является коллекцией.
    /// Кеш не поддерживает удаления.
    /// default&lt;<typeparamref name="TValue"/>&gt; эквивалентен отсутствию значения.
    /// </summary>
    public sealed class DisplacingCache<TKey, TValue>
        where TKey : class
    {
        private readonly int maxCount;

        private ConcurrentDictionary<TKey, TValue> frequentItems = new ConcurrentDictionary<TKey, TValue>();
        private ConcurrentDictionary<TKey, TValue> middleItems = new ConcurrentDictionary<TKey, TValue>();
        private ConcurrentDictionary<TKey, TValue> rarelyItems = new ConcurrentDictionary<TKey, TValue>();

        private readonly object syncRoot = new object();

        /// <summary>
        /// Инициализируется максимальным количеством элементов.
        /// Не рекомендуется добавлять в кеш много значений подряд, больше, чем <paramref name="maxCount"/>, так как в этом случае первые значения
        /// будут потеряны
        /// В случае последовательного заполнения массива, количество элементов будет колебаться от 2/3*<paramref name="maxCount"/> до maxCount.
        /// </summary>
        public DisplacingCache(int maxCount)
        {
            this.maxCount = maxCount;
        }

        /// <summary>
        /// Добавляет элемент в кеш. Если элементов стало больше, чем надо, то старые удаляются
        /// </summary>
        public void Push(TKey key, TValue value)
        {
            Check.ObjectIsNotNull(key, "key");
            Check.ObjectIsNotNull(value, "value");

            frequentItems.AddOrUpdate(key, value, (k, v) => value);

            if (frequentItems.Count <= maxCount / 3 && middleItems.Count + frequentItems.Count + rarelyItems.Count < maxCount)
                return;

            lock (syncRoot)
            {
                if (frequentItems.Count <= maxCount / 3 && middleItems.Count + frequentItems.Count + rarelyItems.Count < maxCount)
                    return;

                rarelyItems = middleItems;
                middleItems = frequentItems;
                frequentItems = new ConcurrentDictionary<TKey, TValue>();
            }
        }

        /// <summary>
        /// Выдает элемент, попутно перетасовывая кеш. Если часто запрашивать определенный элемент, то он будет выдаваться быстрее, и не будет удаляться
        /// из кеша
        /// </summary>
        [CanBeNull]
        public TValue Get(TKey key)
        {
            Check.ObjectIsNotNull(key, "key");

            Check.ObjectIsNotNull(key, "key");

            TValue result = frequentItems.TryGetValue(key);

            if (!Equals(result, default(TValue)))
                return result;

            result = GetWithReplacing(key, middleItems, frequentItems);

            return !Equals(result, default(TValue)) ? result : GetWithReplacing(key, rarelyItems, middleItems);
        }

        private TValue GetWithReplacing(TKey key, ConcurrentDictionary<TKey, TValue> current, ConcurrentDictionary<TKey, TValue> topLevel)
        {
            TValue result = current.TryGetValue(key);

            if (Equals(result, default(TValue)))
                return default(TValue);

            lock (syncRoot)
            {
                topLevel.AddOrUpdate(key, result, (k, v) => result);

                TValue tempValue;

                current.TryRemove(key, out tempValue); //Из-за перемещений массивов, тут значения может уже и не оказаться
            }

            return result;
        }
    }
}
