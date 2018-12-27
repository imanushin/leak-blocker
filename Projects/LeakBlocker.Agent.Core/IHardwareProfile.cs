using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.Agent.Core
{
    internal interface IHardwareProfile
    {
        DeviceAccessMap AccessMap
        {
            get;
        }

        ReadOnlyDictionary<DeviceDescription, DeviceAccessType> DeviceStates
        {
            get;
        }

        ReadOnlyDictionary<DeviceDescription, bool> BlockingStates
        {
            get;
        }

        ReadOnlyMatrix<BaseUserAccount, DeviceDescription, bool> FileAudit
        {
            get;
        }

        ReadOnlyMatrix<BaseUserAccount, DeviceDescription, DeviceAccessType> FileAccess
        {
            get;
        }

        void SetCompletelyBlockedDevices(IReadOnlyCollection<DeviceDescription> devices);
    }
}
