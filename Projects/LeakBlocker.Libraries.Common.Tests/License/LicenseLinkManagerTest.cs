using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeakBlocker.Libraries.Common.License;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Libraries.Common.Tests.License
{
    [TestClass]
    public sealed class LicenseLinkManagerTest : BaseTest
    {
        [TestMethod]
        public void AllRequestsTest()
        {
            Mocks.ReplayAll();

            var target = new LicenseLinkManager();

            foreach (LicenseRequestData requestData in LicenseRequestDataTest.objects)
            {
                Assert.IsNotNull(target.GetLink(requestData));
            }
        }
    }
}
