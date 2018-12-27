using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Equality;

namespace LeakBlocker.Libraries.Common.Collections
{
    /// <summary>
    /// Матрица ограниченного размера.
    /// В процесса создания полностью заполняется.
    /// </summary>
    /// <typeparam name="TKey1"></typeparam>
    /// <typeparam name="TKey2"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix"), SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes"), Serializable]
    [DataContract(IsReference = true)]
    public class ReadOnlyMatrix<TKey1, TKey2, TValue> : BaseReadOnlyObject, IEnumerable<Tuple<TKey1, TKey2, TValue>>
        where TKey1 : BaseReadOnlyObject
        where TKey2 : BaseReadOnlyObject
    {
        [DataMember]
        private readonly Dictionary<TKey1, int> keys1 = new Dictionary<TKey1, int>();

        [DataMember]
        private readonly Dictionary<TKey2, int> keys2 = new Dictionary<TKey2, int>();

        [SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Member")]
        [DataMember]
        private readonly TValue[] map;

        /// <summary>
        /// Полностью создает структуру. В дальнейшем она меняться не будет.
        /// </summary>
        public ReadOnlyMatrix(IReadOnlyCollection<TKey1> keys1, IReadOnlyCollection<TKey2> keys2, Func<TKey1, TKey2, TValue> constructor)
        {
            Check.ObjectIsNotNull(keys1, "keys1");
            Check.ObjectIsNotNull(keys2, "keys2");
            Check.ObjectIsNotNull(constructor, "constructor");

            FillIndexes(keys1, this.keys1);
            FillIndexes(keys2, this.keys2);

            map = new TValue[keys1.Count * keys2.Count];

            foreach (TKey1 key1 in keys1)
            {
                foreach (TKey2 key2 in keys2)
                {
                    var createdItem = constructor(key1, key2);

                    Check.ObjectIsNotNull(createdItem);

                    this[key1, key2] = createdItem;
                }
            }
        }

        private static void FillIndexes<T>(IEnumerable<T> items, IDictionary<T, int> dictionary)
        {
            int index = 0;

            foreach (T item in items)
            {
                dictionary[item] = index++;
            }
        }

        private int GetIndex(int first, int second)
        {
            return first * keys2.Count + second;
        }

        private int GetIndex(TKey1 user, TKey2 device)
        {
            return GetIndex(keys1[user], keys2[device]);
        }

        /// <summary>
        /// Выдает значение соответствующее указанным ключам.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1023:IndexersShouldNotBeMultidimensional")]
        public TValue this[TKey1 user, TKey2 device]
        {
            get
            {
                Check.ObjectIsNotNull(user, "user");
                Check.ObjectIsNotNull(device, "device");

                if (!keys1.ContainsKey(user))
                    Exceptions.Throw(ErrorMessage.NotFound, "User {0} does not exist in map".Combine(user));

                if (!keys2.ContainsKey(device))
                    Exceptions.Throw(ErrorMessage.NotFound, "Device {0} does not exist in map".Combine(device));

                return map[GetIndex(user, device)];
            }
            private set
            {
                map[GetIndex(user, device)] = value;
            }
        }

        /// <summary>
        /// Набор одних ключей
        /// </summary>
        public IEnumerable<TKey1> Keys1
        {
            get
            {
                return keys1.Keys;
            }
        }

        /// <summary>
        /// Набор других ключей
        /// </summary>
        public IEnumerable<TKey2> Keys2
        {
            get
            {
                return keys2.Keys;
            }
        }

        #region Equality

        /// <summary>
        /// Возвращает все вложенные объекты, которые имеют смысл для вычисления хеш-кода и метода Equals
        /// </summary>
        protected sealed override IEnumerable<object> GetInnerObjects()
        {
            Exceptions.Throw(ErrorMessage.NotSupported);
            return null;
        }

        /// <summary>
        /// Вычисляет хеш код
        /// </summary>
        protected sealed override int CalculateHashCode()
        {
            int result = 0;

            foreach (int firstIndex in keys1.Values)
            {
                foreach (int secondIndex in keys2.Values)
                {
                    result ^= map[GetIndex(firstIndex, secondIndex)].GetHashCode();
                }
            }

            result ^= EnumerableHashCode.GetHashCode(keys1.Keys);
            result ^= EnumerableHashCode.GetHashCode(keys2.Keys);

            return result;
        }

        /// <summary>
        /// Проверяет, что объекты равны
        /// </summary>
        protected sealed override bool CheckEquality(BaseReadOnlyObject target)
        {
            var other = (ReadOnlyMatrix<TKey1, TKey2, TValue>)target;

            if (!EnumerableComparer.Compare(keys1.Keys, other.keys1.Keys))
                return false;

            return EnumerableComparer.Compare(keys2.Keys, other.keys2.Keys) && 
                keys1.Keys.All(key1 => keys2.Keys.All(key2 => Equals(this[key1, key2], other[key1, key2])));
        }

        #endregion

        /// <summary>
        /// Итератор по всем элементам коллекции
        /// </summary>
        public IEnumerator<Tuple<TKey1, TKey2, TValue>> GetEnumerator()
        {
            foreach (TKey1 key1 in Keys1)
            {
                foreach (TKey2 key2 in Keys2)
                {
                    yield return new Tuple<TKey1, TKey2, TValue>(key1, key2, this[key1, key2]);
                }
            }
        }

        /// <summary>
        /// Итератор по всем элементам коллекции
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        [SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Body")]
        protected override string GetString()
        {
            ReadOnlyList<TKey1> keys1List = Keys1.ToReadOnlyList();
            ReadOnlyList<TKey2> keys2List = Keys2.ToReadOnlyList();

            var resultStrings = new string[keys1List.Count + 1, keys2List.Count + 1];

            resultStrings[0, 0] = string.Empty;

            for (int i = 0; i < keys1List.Count; i++)
            {
                resultStrings[i + 1, 0] = keys1List[i].ToString();
            }

            for (int i = 0; i < keys2List.Count; i++)
            {
                resultStrings[0, i + 1] = keys2List[i].ToString();
            }

            for (int x = 0; x < keys1List.Count; x++)
            {
                for (int y = 0; y < keys2List.Count; y++)
                {
                    resultStrings[x + 1, y + 1] = this[keys1List[x], keys2List[y]].ToString();
                }
            }

            int minWidth = resultStrings.Cast<string>().Select(str => str.Length).Max();

            string template = "{{0,-{0}}}".Combine(minWidth + 1);

            var result = new StringBuilder();

            for (int x = 0; x < keys1List.Count+1; x++)
            {
                for (int y = 0; y < keys2List.Count+1; y++)
                {
                    result.AppendFormat(template, resultStrings[x, y]);
                }

                result.AppendLine();
            }

            return result.ToString();
        }
    }
}
