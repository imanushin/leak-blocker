using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LeakBlocker.InternalLicenseManager;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.License;
using LeakBlocker.Libraries.Common.Tests;
using LeakBlocker.Server.Service.InternalTools.SingletonImplementation.LocalStorages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace LeakBlocker.Server.Service.Tests.InternalTools.LocalStorages
{
    [TestClass]
    public sealed class LicenseStorageTest : BaseTest
    {
        private const string subfolderName = "Licenses";

        [TestMethod]
        public void AddGetTest()
        {
            var scheduler = Mocks.StrictMock<IScheduler>();

            Action schedulerAction = null;

            SharedObjects.Factories.Scheduler.EnqueueConstructor(
                (action, a2, a4) =>
                {
                    schedulerAction = action;
                    return scheduler;
                });

            scheduler.Stub(x => x.RunNow());

            Mocks.ReplayAll();

            string dirName = Path.Combine(SharedObjects.Constants.UserDataFolder, subfolderName);

            if (Directory.Exists(dirName))
                Directory.Delete(dirName);

            var license1 = LicenseCreator.CreateLicense("123", 10, ReadOnlySet<LicenseInfo>.Empty);
            var license2 = LicenseCreator.CreateLicense("321", 20, new[] { license1 }.ToReadOnlySet());
            var license3 = LicenseCreator.CreateLicense("321", new Time(new DateTime(2050, 1, 1)));
            var license4 = LicenseCreator.CreateLicense("321", 30, new[] { license2, license3 }.ToReadOnlySet());

            var target = new LicenseStorage();

            Assert.IsNotNull(schedulerAction);

            schedulerAction = null;

            Assert.AreEqual(0, target.GetAllActualLicenses().Count);

            target.AddLicense(license1);

            Assert.AreEqual(1, target.GetAllActualLicenses().Count);

            target.AddLicense(license2);

            Assert.AreEqual(1, target.GetAllActualLicenses().Count);

            target.AddLicense(license3);

            Assert.AreEqual(2, target.GetAllActualLicenses().Count);

            target.AddLicense(license4);

            Assert.AreEqual(1, target.GetAllActualLicenses().Count);

            target = new LicenseStorage();

            Assert.IsNotNull(schedulerAction);

            schedulerAction();

            Assert.AreEqual(1, target.GetAllActualLicenses().Count);
        }
    }
}
