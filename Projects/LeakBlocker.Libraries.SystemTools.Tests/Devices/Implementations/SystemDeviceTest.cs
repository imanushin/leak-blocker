using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.SystemTools.Devices.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Libraries.SystemTools.Tests.Devices.Implementations
{
    partial class SystemDeviceTest
    {
        private static IEnumerable<SystemDevice> GetInstances()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void EnumerateDevices()
        {
            if (Dns.GetHostEntry("localhost").HostName.ToLower().Contains("deltacorvi.com"))
                return;

            foreach (bool includeOffline in new[] {true, false})
            {
                foreach (bool onlyRemovable in new[] {true, false})
                {
                    SystemDevice.EnumerateDevices(includeOffline, default(SystemAccessOptions), onlyRemovable).OfType<SystemDevice>();
                }
            }
        }

        protected override bool SkipTesting
        {
            get
            {
                return true;
            }
        }
    }
}
