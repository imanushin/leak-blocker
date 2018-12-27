using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;
using LeakBlocker.Server.Service.InternalTools.SingletonImplementation.LocalStorages;
using LeakBlocker.ServerShared.AdminViewCommunication;
using LeakBlocker.ServerShared.AdminViewCommunication.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Server.Service.Tests.InternalTools.LocalStorages
{
    [TestClass]
    public sealed class ReportConfigurationStorageTest : BaseConfigurationManagerTest
    {
        [TestMethod]
        public void SaveLoadTest()
        {
            Mocks.ReplayAll();

            CleanupDirectory();

            BaseUserAccount user = BaseUserAccountTest.objects.First();

            var target = new ReportConfigurationStorage();

            Assert.IsNull(target.Current);

            ReportConfiguration emptySettings = target.GetCurrentOrDefaultConfiguration();

            Assert.IsNotNull(emptySettings);

            ReportConfiguration newSettings = ReportConfigurationTest.objects.First();

            target.Save(newSettings);

            Assert.AreEqual(newSettings, target.Current);
            Assert.AreEqual(newSettings, target.GetCurrentOrDefaultConfiguration());
        }
    }
}
