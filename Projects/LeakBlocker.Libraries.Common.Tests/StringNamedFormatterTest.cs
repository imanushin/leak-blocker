using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Libraries.Common.Tests
{
    [TestClass]
    public sealed class StringNamedFormatterTest : BaseTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ApplyTemplate1()
        {
            StringNamedFormatter.ApplyTemplate(null, new Dictionary<string, GetString>().ToReadOnlyDictionary());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ApplyTemplate2()
        {
            StringNamedFormatter.ApplyTemplate("qqq", null);
        }

        [TestMethod]
        public void ApplyTemplate3()
        {
            var targetObject = new object();

            const string key1 = "key1";
            const string value1 = "value1";

            var dictionary = new Dictionary<string, GetString>() { { key1, () => value1 } };

            const string goodTemplate = "123{key1}321";
            const string expectedResult = "123value1321";

            string actualResult = StringNamedFormatter.ApplyTemplate(goodTemplate, dictionary.ToReadOnlyDictionary());

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
