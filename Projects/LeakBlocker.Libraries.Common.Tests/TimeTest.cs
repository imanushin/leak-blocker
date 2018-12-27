using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Libraries.Common.Tests
{
    partial class TimeTest
    {
        private static IEnumerable<Time> GetInstances()
        {
            yield return new Time(new DateTime(2012, 12, 21, 1, 2, 3, 6, DateTimeKind.Local));

            yield return new Time(new DateTime(2030, 12, 21, 1, 2, 3, 6, DateTimeKind.Local));

            yield return new Time(132456789);

            yield return new Time(new DateTime(2031, 1, 2, 1, 2, 3, 6, DateTimeKind.Utc));
        }

        [TestMethod]
        public void CompareTest()
        {
            var first = new Time(123);
            var second = new Time(123);
            var third = new Time(234);

            Assert.IsTrue(null < first && first == second && second < third);
            Assert.IsTrue(third > second &&  !(first != second) && first > null);
            Assert.IsTrue(null <= first && second <= third);
            Assert.IsTrue(third >= second && first >= null);
            Assert.IsTrue(third != first && null != first && first != null);

        }
    }
}
