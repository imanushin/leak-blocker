using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Settings.TemporaryAccess;
using LeakBlocker.Libraries.Common.Tests;
using LeakBlocker.Libraries.Common.Tests.Entities.Settings.TemporaryAccess;
using LeakBlocker.Server.Service.InternalTools.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Server.Service.Tests.InternalTools.Extensions
{
    [TestClass]
    public sealed class BaseTemporaryAccessConditionExtensionsTest : BaseTest
    {
        [TestMethod]
        public void GetDomains()
        {
            Mocks.ReplayAll();

            foreach (BaseTemporaryAccessCondition baseTemporaryAccessCondition in BaseTemporaryAccessConditionTest.objects)
            {
                Assert.IsNotNull(baseTemporaryAccessCondition.GetDomains().ToReadOnlySet());
            }
        }
    }
}
