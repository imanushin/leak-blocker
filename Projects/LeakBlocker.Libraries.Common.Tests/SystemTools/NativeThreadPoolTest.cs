using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.SystemTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Libraries.Common.Tests.SystemTools
{
    [TestClass]
    public sealed class NativeThreadPoolTest : BaseTest
    {
        private const int threadCount = 3;

        [TestMethod]
        public void FullTest()
        {
            Mocks.ReplayAll();

            var threads = new ConcurrentBag<int>();

            using (var target = new NativeThreadPool(threadCount))
            {
                Thread.Sleep(1000);//Ждем старта

                Enumerable.Repeat(0, threadCount).ForEach(item => target.EnqueueAction(
                    () =>
                    {
                        Thread.Sleep(1000);

                        threads.Add(Thread.CurrentThread.ManagedThreadId);
                    }));

                for (int i = 0; i < 60; i++)//По-хитрому ждем завершения, ибо работает нестабильно
                {
                    if (threads.Count < threadCount)
                        Thread.Sleep(1000);
                    else
                        break;
                }

                Assert.AreEqual(threadCount, threads.Count);
                CollectionAssert.AllItemsAreUnique(threads);
            }
        }
    }
}
