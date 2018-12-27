using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LeakBlocker.AdminView.Desktop.Network;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.SystemTools.Entities;
using LeakBlocker.Server.Service.InternalTools;
using LeakBlocker.Server.Service.InternalTools.LocalStorages;
using LeakBlocker.Server.Service.Network.AdminView;
using LeakBlocker.ServerShared.AdminViewCommunication;
using LeakBlocker.ServerShared.AdminViewCommunication.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace LeakBlocker.Server.Service.Tests.Network.AdminView
{
    [TestClass]
    public sealed class ReportToolsServerTest : BaseAdminViewNetworkTest
    {
        //private ISystemAccountTools accountTools;
        private IReportConfigurationStorage reportConfigurationStorage;

        [TestInitialize]
        public void Init()
        {
            reportConfigurationStorage = Mocks.StrictMock<IReportConfigurationStorage>();

            InternalObjects.Singletons.ReportConfigurationStorage.SetTestImplementation(reportConfigurationStorage);

            Initialize();
        }


        [TestMethod]
        public void LoadSettings()
        {
            ReportConfiguration expectedSettings = ReportConfigurationTest.objects.First();
            reportConfigurationStorage.Stub(x => x.GetCurrentOrDefaultConfiguration()).Return(expectedSettings);

            Mocks.ReplayAll();

            if (Directory.Exists(SharedObjects.Constants.UserDataFolder))
            {
                IEnumerable<string> files = Directory.EnumerateFiles(SharedObjects.Constants.UserDataFolder, "*.lbConfig");

                files.ForEach(File.Delete);
            }

            using (InitServer<ReportToolsServer>())
            {
                using (IReportTools client = new ReportToolsClient())
                {
                    var settings = client.LoadSettings();

                    Assert.AreEqual(expectedSettings, settings);
                }
            }
        }

        [TestMethod]
        public void SaveSettings()
        {
            ReportConfiguration expectedSettings = ReportConfigurationTest.objects.First();
            reportConfigurationStorage.Expect(x => x.Save(expectedSettings));

            Mocks.ReplayAll();

            if (Directory.Exists(SharedObjects.Constants.UserDataFolder))
            {
                IEnumerable<string> files = Directory.EnumerateFiles(SharedObjects.Constants.UserDataFolder, "*.lbConfig");

                files.ForEach(File.Delete);
            }

            using (InitServer<ReportToolsServer>())
            {
                using (IReportTools client = new ReportToolsClient())
                {
                    client.SaveSettings(expectedSettings);
                }
            }

            Mocks.VerifyAll();
        }
    }
}
