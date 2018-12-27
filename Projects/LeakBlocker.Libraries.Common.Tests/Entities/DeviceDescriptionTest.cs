using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Libraries.Common.Tests.Entities
{
    partial class DeviceDescriptionTest
    {
        private static IEnumerable<DeviceDescription> GetInstances()
        {
            yield return CreateSingleDevice();

            const int deviceNumber = 357;

            string name = "test device " + deviceNumber;

            yield return new DeviceDescription(name, "{{eee:qqq}}", DeviceCategory.Storage);
        }

        private static DeviceDescription CreateSingleDevice()
        {
            const int deviceNumber = 953;

            string name = "test device " + deviceNumber;

            var children = new List<DeviceDescription>();

            return new DeviceDescription(name, "{{qqq:eee}}", DeviceCategory.Storage);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void LogicalDeviceConstructorTest4()
        {
            new DeviceDescription("test", string.Empty, DeviceCategory.Storage);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void LogicalDeviceConstructorTest5()
        {
            new DeviceDescription("test", "   ", DeviceCategory.Storage);
        }

    }
}
