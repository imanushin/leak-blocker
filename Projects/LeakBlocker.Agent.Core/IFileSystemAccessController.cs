using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.SystemTools.Devices;

namespace LeakBlocker.Agent.Core
{
    internal interface IFileSystemAccessControllerUpdateSession : IDisposable
    {
        void SetAccessRule(DeviceDescription device, VolumeName volume, AccountSecurityIdentifier user, DeviceAccessType access);
        void SetAuditRule(DeviceDescription device, VolumeName volume, AccountSecurityIdentifier user, bool monitoring);
    }

    internal interface IFileSystemAccessController
    {
        ReadOnlySet<long> Instances
        {
            get;
        }

        ReadOnlySet<VolumeName> Volumes
        {
            get;
        }

        ReadOnlyDictionary<AccountSecurityIdentifier, DeviceAccessType> GetDriverInstanceConfiguration(long instance);

        IFileSystemAccessControllerUpdateSession GetUpdateSession();

        void AddDriverInstance(VolumeName volume, long identifier);

        void RemoveDriverInstance(long identifier);

        bool CheckMonitoredVolume(VolumeName volume, AccountSecurityIdentifier user);

        DeviceAccessType GetConfiguredAccessType(VolumeName volume, AccountSecurityIdentifier user);

        DeviceDescription GetDevice(VolumeName volume);
    }
}
