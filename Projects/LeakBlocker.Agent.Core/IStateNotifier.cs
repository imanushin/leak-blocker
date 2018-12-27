using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.Agent.Core
{
    internal enum AgentFileActionType
    {
        [ForbiddenToUse]
        [Obsolete("Incorrect value.", true)]
        [UsedImplicitly]
        None = 0,

        Unknown,

        ReadTemporarilyAllowed,
        WriteTemporarilyAllowed,
        ReadWriteTemporarilyAllowed,
        ReadAllowed,
        WriteAllowed,
        ReadWriteAllowed,
        ReadBlocked,
        WriteBlocked,
        ReadWriteBlocked
    }

    internal interface IStateNotifierHandler
    {
        void ServiceStarted();
        void ServiceStopped();
        void SystemStarted();
        void SystemShutdown();
        void UnplannedServiceShutdown();
        void ServerInaccessible();
        void FileAccessed(DeviceDescription device, string file, AgentFileActionType state, string process, BaseUserAccount user);

        void DeviceAdded(DeviceDescription device);
        void DeviceRemoved(DeviceDescription device);
        void UserLoggedOn(BaseUserAccount user);
        void UserLoggedOff(BaseUserAccount user);
        void DeviceStateChanged(DeviceDescription device, DeviceAccessType state);
    }

    internal interface IStateNotifier
    {
        DeviceAccessMap DeviceAccess
        {
            get;
            set;
        }

        void SetNewUsers(IReadOnlyCollection<BaseUserAccount> newUsers);
        void SetNewDevices(IReadOnlyCollection<DeviceDescription> newDevices);
        void SetDeviceState(DeviceDescription device, DeviceAccessType state);
        void ServiceStart();
        void ServiceStop();
        void SystemStart();
        void SystemShutdown();
        void ServerInaccessible();
        void FileAccessed(DeviceDescription device, string file, AgentFileActionType state, string process, BaseUserAccount user);
    }
}
