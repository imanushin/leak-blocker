using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.SystemTools.Devices.Management;
using LeakBlocker.Libraries.SystemTools.Win32;

namespace LeakBlocker.Libraries.SystemTools.Tests.Devices.Management
{
    partial class DeviceTreeNodeTest
    {
        private static IEnumerable<DeviceTreeNode> GetInstances()
        {
            var data = new NativeMethods.SP_DEVINFO_DATA();

            data.DevInst++;

            var single = new DeviceTreeNode(data, ReadOnlySet<DeviceTreeNode>.Empty);

            data.DevInst++;

            var withChild = new DeviceTreeNode(data, new[] { single }.ToReadOnlySet());

            data.DevInst++;

            var withChilds = new DeviceTreeNode(data, new[] { withChild }.ToReadOnlySet());

            yield return single;
            yield return withChild;
            yield return withChilds;
        }

        protected override bool SkipSerializationTest()
        {
            return true;
        }
    }
}
