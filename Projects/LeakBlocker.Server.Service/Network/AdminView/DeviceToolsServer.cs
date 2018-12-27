using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Network;
using LeakBlocker.Libraries.Storage;
using LeakBlocker.Libraries.SystemTools;
using LeakBlocker.Server.Service.Generated;
using LeakBlocker.Server.Service.InternalTools;

namespace LeakBlocker.Server.Service.Network.AdminView
{
    internal sealed class DeviceToolsServer : GeneratedDeviceTools
    {
        public DeviceToolsServer()
            : base((ISecuritySessionManager)InternalObjects.AdminViewSecuritySessionManager)
        {
        }

        protected override ReadOnlySet<DeviceDescription> GetConnectedDevices()
        {
            return SystemObjects.DeviceProvider.EnumerateDevices();
        }
    }
}
