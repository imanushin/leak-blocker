using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Libraries.Common.Tests
{
    [TestClass]
    public sealed class StringExtensionsTest
    {
        [TestMethod]
        public void Combine()
        {
            string template = "!{0}!";

            string result = template.Combine("123");

            Assert.AreEqual(string.Format(template, "123"), result);
        }
    }
}
