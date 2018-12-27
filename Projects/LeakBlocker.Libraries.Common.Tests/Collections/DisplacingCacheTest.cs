using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Libraries.Common.Tests.Collections
{
    [TestClass]
    public sealed class DisplacingCacheTest : BaseTest
    {
        [TestMethod]
        public void AddGetTest()
        {
            var target = new DisplacingCache<string, string>(3);

            target.Push("1", "1");
            target.Push("2", "2");
            target.Push("3", "3");
            target.Push("4", "4");

            Assert.AreEqual(null, target.Get("1"));//Вытеснен
            Assert.AreEqual("3", target.Get("3"));
            Assert.AreEqual("4", target.Get("4"));

            target.Push("5", "5");

            Assert.AreEqual(null, target.Get("2"));//Так как обращались давно

        }
    }
}
