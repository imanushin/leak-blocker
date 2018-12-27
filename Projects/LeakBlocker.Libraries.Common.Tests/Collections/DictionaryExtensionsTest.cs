﻿using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Libraries.Common.Tests.Collections
{
// ReSharper disable InvokeAsExtensionMethod
    [TestClass]
    public sealed class DictionaryExtensionsTest
    {
        private const int key1 = 5;
        private const int key2 = 12;
        private const string value1 = "234";
        private const string value2 = "543";

        [TestMethod]
        public void AsReadOnly()
        {
            var dictionary = new Dictionary<int, string>();

            ReadOnlyDictionary<int, string> result = DictionaryExtensions.ToReadOnlyDictionary(dictionary);

            Assert.IsNotNull(result);

            ReadOnlyDictionary<int, string> theSame = result.ToReadOnlyDictionary();

            Assert.AreSame(result,theSame);
        }

        [TestMethod]
        public void TryAdd()
        {
            var dictionary = new Dictionary<int, string>();

            bool result = DictionaryExtensions.TryAdd(dictionary, key1, value1);
            Assert.IsTrue(result);

            result = DictionaryExtensions.TryAdd(dictionary, key1, value1);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TryRemove()
        {
            var dictionary = new Dictionary<int, string>();

            bool result = DictionaryExtensions.TryRemove(dictionary, key1);
           Assert.IsFalse(result);

            dictionary.Add(key1, value1);

            result = DictionaryExtensions.TryRemove(dictionary, key1);
           Assert.IsTrue(result);

            result = DictionaryExtensions.TryRemove(dictionary, key1);
           Assert.IsFalse(result);
        }

        [TestMethod]
        public void TryRemove_Simple()
        {
            var dictionary = new Dictionary<int, string>();

            bool result = DictionaryExtensions.TryRemove(dictionary, key1);
           Assert.IsFalse(result);

            dictionary.Add(key1, value1);

            result = DictionaryExtensions.TryRemove(dictionary, key1);
           Assert.IsTrue(result);

            result = DictionaryExtensions.TryRemove(dictionary, key1);
           Assert.IsFalse(result);
        }

        [TestMethod]
        public void TryRemove_Out()
        {
            var dictionary = new Dictionary<int, string>();

            bool result = DictionaryExtensions.TryRemove(dictionary, key1);
           Assert.IsFalse(result);

            dictionary.Add(key1, value1);

            string outValue;

            result = DictionaryExtensions.TryRemove(dictionary, key1, out outValue);
           Assert.IsTrue(result);
           Assert.AreEqual(value1, outValue);

            result = DictionaryExtensions.TryRemove(dictionary, key1, out outValue);
           Assert.IsFalse(result);
           Assert.IsNull(outValue);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MergeWith_FirstIsNull()
        {
            DictionaryExtensions.MergeWith(null, new Dictionary<int, string>(), false);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MergeWith_SecondIsNull()
        {
            DictionaryExtensions.MergeWith(new Dictionary<int, string>(), null, false);
        }

        [TestMethod]
        public void MergeWith_OnlyAdd()
        {
            var first = new Dictionary<int, string>();
            var second = new Dictionary<int, string>();

            first.Add(key1, value1);
            second.Add(key1, value2);
            second.Add(key2, value2);

            /*int changedCount =*/ DictionaryExtensions.MergeWith(first, second, false);

           //Assert.AreEqual(1, changedCount);
           Assert.AreEqual(2, first.Count);
           Assert.AreEqual(value1, first[key1]);
        }

        [TestMethod]
        public void MergeWith_AddOrReplace()
        {
            var first = new Dictionary<int, string>();
            var second = new Dictionary<int, string>();

            first.Add(key1, value1);
            second.Add(key1, value2);
            second.Add(key2, value2);

            /*int changedCount =*/ DictionaryExtensions.MergeWith(first, second, true);

           //Assert.AreEqual(2, changedCount);
           Assert.AreEqual(2, first.Count);
           Assert.AreEqual(value2, first[key1]);
        }

        [TestMethod]
        public void GetOrAdd()
        {
            var target = new Dictionary<string, int>();

            string key = "key";

            int added = target.GetOrAdd(key);

            Assert.AreEqual(0, added);

            target[key] = 1;

            int getted = target.GetOrAdd(key);

            Assert.AreEqual(1, getted);
        }

        [TestMethod]
        public void GetOrNull()
        {
            var target = new Dictionary<string, string>();

            string key = "key";
            string value = "value";

            string firstResult = target.TryGetValue(key);

            Assert.IsNull(firstResult);

            target[key] = value;

            string secondResult = target.TryGetValue(key);

            Assert.AreEqual(value, secondResult);
        }
    }
    // ReSharper restore InvokeAsExtensionMethod

}
