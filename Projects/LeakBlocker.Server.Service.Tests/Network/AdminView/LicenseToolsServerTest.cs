using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.AdminView.Desktop.Network;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.License;
using LeakBlocker.Libraries.Common.Tests.License;
using LeakBlocker.Server.Service.InternalTools;
using LeakBlocker.Server.Service.InternalTools.LocalStorages;
using LeakBlocker.Server.Service.Network.AdminView;
using LeakBlocker.ServerShared.AdminViewCommunication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace LeakBlocker.Server.Service.Tests.Network.AdminView
{
    [TestClass]
    public sealed class LicenseToolsServerTest : BaseAdminViewNetworkTest
    {
        [TestInitialize]
        public void Init()
        {
            Initialize();
        }

        [TestMethod]
        public void AddLicense()
        {
            var licenseStorage = Mocks.StrictMock<ILicenseStorage>();

            licenseStorage.Expect(x => x.AddLicense(LicenseInfoTest.First));

            InternalObjects.Singletons.LicenseStorage.SetTestImplementation(licenseStorage);

            Mocks.ReplayAll();

            using (InitServer<LicenseToolsServer>())
            {
                using (ILicenseTools client = new LicenseToolsClient())
                {
                    client.AddLicense(LicenseInfoTest.First);
                }
            }

            Mocks.VerifyAll();
        }

        [TestMethod]
        public void GetAllActualLicenses()
        {
            var licenseStorage = Mocks.StrictMock<ILicenseStorage>();

            licenseStorage.Expect(x => x.GetAllActualLicenses()).Return(LicenseInfoTest.objects.Objects);

            InternalObjects.Singletons.LicenseStorage.SetTestImplementation(licenseStorage);

            Mocks.ReplayAll();

            using (InitServer<LicenseToolsServer>())
            {
                using (ILicenseTools client = new LicenseToolsClient())
                {
                    ReadOnlySet<LicenseInfo> result = client.GetAllActualLicenses();

                    Assert.AreEqual(LicenseInfoTest.objects.Objects, result);
                }
            }

            Mocks.VerifyAll();
        }
    }
}
