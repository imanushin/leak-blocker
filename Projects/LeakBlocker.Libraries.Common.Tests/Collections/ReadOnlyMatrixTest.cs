using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Tests.Entities;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Libraries.Common.Tests.Collections
{
    [TestClass]
    public sealed class ReadOnlyMatrixTest : BaseTest
    {
        [TestMethod]
        public void EnumeratorTest()
        {
            Mocks.ReplayAll();

            var target = new ReadOnlyMatrix<DomainAccount, DeviceDescription, string>(
                DomainAccountTest.objects.Objects, 
                DeviceDescriptionTest.objects.Objects, 
                (a, b) => "{0}_{1}".Combine(a, b));

            foreach (Tuple<DomainAccount, DeviceDescription, string> tuple in target)
            {
                Assert.IsNotNull(tuple);

                Assert.IsNotNull(tuple.Item1);
                Assert.IsNotNull(tuple.Item2);
                Assert.IsNotNull(tuple.Item3);
            }
        }
    }
}
