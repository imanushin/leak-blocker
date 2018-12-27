using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.SystemTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Libraries.Common.Tests.Collections
{
    [TestClass]
    public sealed class EnumerableExtensionsTest : BaseTest
    {
        [TestInitialize]
        public void Init()
        {
            SharedObjects.Factories.ThreadPool.EnqueueConstructor(val => new NativeThreadPool(val));
        }

        [TestMethod]
        public void ToReadOnlyList()
        {
            var input = new[] { 1, 2, 3 };

            ReadOnlyList<int> output = EnumerableExtensions.ToReadOnlyList(input);

            CollectionAssert.AreEqual(output, input);
        }

        [TestMethod]
        public void ForEach_Action()
        {
            var input = new[] { "1", null };

            var output = new List<string>();

            input.ForEach(output.Add);

            CollectionAssert.AreEquivalent(input, output);
        }

        [TestMethod]
        public void ParallelForEach_Success()
        {
            int index = 0;

            EnumerableExtensions.ParallelForEach(Enumerable.Range(1, 10), item => Interlocked.Increment(ref index));

            Assert.AreEqual(10, index);
        }

        [TestMethod]
        public void ParallelSelect_Success()
        {
            ICollection<string> result = EnumerableExtensions.ParallelSelect(Enumerable.Range(1, 10), item => item.ToString(CultureInfo.InvariantCulture)).ToList();

            CollectionAssert.AllItemsAreUnique(result.ToList());

            Assert.AreEqual(10, result.Count);
        }

        [TestMethod]
        public void ParallelForEach_Fail()
        {
            try
            {
                EnumerableExtensions.ParallelForEach(Enumerable.Range(1, 10), item =>
                                                                                  {
                                                                                      throw new Exception();
                                                                                  });

                Assert.Fail();//В предыдущем методе должно вылететь исключение
            }
            catch (AggregateException ex)
            {
                Assert.AreEqual(10, ex.InnerExceptions.Count);
            }
        }

        [TestMethod]
        public void UnionWith()
        {
            IEnumerable<int> original = Enumerable.Range(0, 4);
            const int newElement = 4;
            IEnumerable<int> otherElements = Enumerable.Range(5, 9);

            var result = original.UnionWith(newElement, otherElements.ToArray()).ToReadOnlyList();

            Assert.AreEqual(Enumerable.Range(0, 14).ToReadOnlyList(), result);

        }
    }
}
