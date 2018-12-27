using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Libraries.Common.Tests.Collections
{
    [TestClass]
    public sealed class ReadOnlyDictionaryTest
    {
        private ReadOnlyDictionary<int, string> target;
        private IDictionary targetDictionary;
        private IDictionary<int, string> targetGenericDictionary;
        private Dictionary<int, string> expected;

        private const int key = 1;
        private const string value = "123";

        [TestInitialize]
        public void Initialize()
        {
            expected = new Dictionary<int, string>();

            expected.Add(key, value);

            target = new ReadOnlyDictionary<int, string>(expected);
            targetDictionary = target;
            targetGenericDictionary = target;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorWrongArguments()
        {
            new ReadOnlyDictionary<int, string>(null);
        }

        [TestMethod]
        public void This()
        {
           Assert.AreEqual(expected[key], target[key]);

            expected[key] = "321";

            Assert.AreEqual(value, target[key]);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void This_NotSupported()
        {
            target[key] = value;
        }

        [TestMethod]
        public void IsSynchronized()
        {
           Assert.IsTrue(((ICollection)target).IsSynchronized);
        }

        [TestMethod]
        public void SyncRoot()
        {
            Assert.IsNotNull(((ICollection)target).SyncRoot);
        }

        [TestMethod]
        public void Count()
        {
           Assert.AreEqual(expected.Count, target.Count);
        }

        [TestMethod]
        public void IsFixedSize()
        {
            Assert.IsTrue(((IDictionary)target).IsFixedSize);
        }

        [TestMethod]
        public void IsReadOnly()
        {
            Assert.IsTrue(((ICollection<KeyValuePair<int, string>>)target).IsReadOnly);
        }

        #region Keys and Values

        [TestMethod]
        public void Keys_CommonChecks()
        {
            ICollection<int> keys = target.Keys;

           Assert.IsNotNull(keys);
           Assert.IsTrue(keys.IsReadOnly);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Keys_AddFail()
        {
            ICollection<int> keys = target.Keys;

            keys.Add(5);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Keys_RemoveFail()
        {
            ICollection<int> keys = target.Keys;

            keys.Remove(key);
        }

        [TestMethod]
        public void Values_CommonChecks()
        {
            ICollection<string> values = target.Values;

           Assert.IsNotNull(values);
           Assert.IsTrue(values.IsReadOnly);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Values_AddFail()
        {
            ICollection<string> values = target.Values;

            values.Add("5");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Values_RemoveFail()
        {
            ICollection<string> values = target.Values;

            values.Remove(value);
        }

        #endregion

        [TestMethod]
        public void ContainsKey()
        {
           Assert.AreEqual(expected.ContainsKey(key), target.ContainsKey(key));
           Assert.AreEqual(expected.ContainsKey(key + 1), target.ContainsKey(key + 1));
        }

        [TestMethod]
        public void TryGetValue()
        {
            string expectedOutValue;
            string actualOutValue;

           Assert.AreEqual(expected.TryGetValue(key, out expectedOutValue), target.TryGetValue(key, out actualOutValue));
           Assert.AreEqual(expectedOutValue, actualOutValue);

           Assert.AreEqual(expected.TryGetValue(key + 1, out expectedOutValue), target.TryGetValue(key + 1, out actualOutValue));
           Assert.AreEqual(expectedOutValue, actualOutValue);
        }

        [TestMethod]
        public void GetEnumerator()
        {
           Assert.IsNotNull(target.GetEnumerator());
        }

        [TestMethod]
        public void CopyTo()
        {
            var objects = new object[expected.Count + 1];

            targetDictionary.CopyTo(objects, 1);

           Assert.IsNotNull(objects[1]);
           Assert.IsNull(objects[0]);
        }

        [TestMethod]
        public void IDictionary_Keys()
        {
           Assert.IsNotNull(targetDictionary.Keys);
        }

        [TestMethod]
        public void IDictionary_Values()
        {
           Assert.IsNotNull(targetDictionary.Values);
        }

        [TestMethod]
        public void IDictionary_GetEnumerator()
        {
           Assert.IsNotNull(targetDictionary.GetEnumerator());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Add()
        {
            targetGenericDictionary.Add(3, "654");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Remove()
        {
            targetGenericDictionary.Remove(3);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Clear()
        {
            targetGenericDictionary.Clear();
        }

    }
}
