using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Tests.Entities;
using LeakBlocker.Libraries.Storage.Entities;
using LeakBlocker.Libraries.Storage.InternalTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Libraries.Storage.Tests
{
    [TestClass]
    public sealed class DeviceManagerTest : BaseDatabaseObjectsTests
    {
        [TestMethod]
        public void GetAllDevices()
        {
            Mocks.ReplayAll();

            ReadOnlySet<DeviceDescription> original = DeviceDescriptionTest.objects.Objects;

            using (IDatabaseModel model = StorageObjects.CreateDatabaseModel())
            {
                original
                    .Select(device => DbDeviceDescription.ConvertFromDeviceDescription(device, model))
                    .ForEach(device => model.DeviceDescriptionSet.Add(device));

                model.SaveChanges();
            }

            var target = new DevicesManager();

            ReadOnlySet<DeviceDescription> result = target.GetAllDevices();

            Assert.AreEqual(original, result);
        }
    }
}
