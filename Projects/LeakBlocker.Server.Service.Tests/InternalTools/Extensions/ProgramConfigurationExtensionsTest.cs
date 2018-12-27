using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Settings;
using LeakBlocker.Libraries.Common.Tests;
using LeakBlocker.Libraries.Common.Tests.Entities.Settings;
using LeakBlocker.Server.Service.InternalTools.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Server.Service.Tests.InternalTools.Extensions
{
    [TestClass]
    public sealed class ProgramConfigurationExtensionsTest : BaseTest
    {
        [TestMethod]
        public void GetDomains()
        {
            Mocks.ReplayAll();

            foreach (ProgramConfiguration configuration in ProgramConfigurationTest.objects)
            {
                Assert.IsNotNull(configuration.GetAllDomains().ToReadOnlySet());
            }
        }
    }
}
