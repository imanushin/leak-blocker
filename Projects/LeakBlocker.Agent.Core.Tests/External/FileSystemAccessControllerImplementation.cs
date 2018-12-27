using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;
using LeakBlocker.Libraries.SystemTools.Devices;

namespace LeakBlocker.Agent.Core.Tests.External
{
    internal sealed class FileSystemAccessControllerImplementation : BaseTestImplementation, IFileSystemAccessController, IFileSystemAccessControllerUpdateSession
    {
        readonly Dictionary<long, VolumeName> instances = new Dictionary<long, VolumeName>();

        public bool NoDevices;

        readonly Dictionary<DeviceDescription, Dictionary<VolumeName, Dictionary<AccountSecurityIdentifier, DeviceAccessType>>> deviceAccess =
            new Dictionary<DeviceDescription, Dictionary<VolumeName, Dictionary<AccountSecurityIdentifier, DeviceAccessType>>>();

        readonly Dictionary<DeviceDescription, Dictionary<VolumeName, Dictionary<AccountSecurityIdentifier, bool>>> deviceAudit =
            new Dictionary<DeviceDescription, Dictionary<VolumeName, Dictionary<AccountSecurityIdentifier, bool>>>();

        public ReadOnlyDictionary<AccountSecurityIdentifier, DeviceAccessType> GetDriverInstanceConfiguration(long instance)
        {
            return new Dictionary<AccountSecurityIdentifier, DeviceAccessType> { { AccountSecurityIdentifierTest.First, DeviceAccessType.ReadOnly } }.ToReadOnlyDictionary();
        }

        public IFileSystemAccessControllerUpdateSession GetUpdateSession()
        {
            return this;
        }

        public void AddDriverInstance(VolumeName volume, long identifier)
        {
            instances.Add(identifier, volume);

            base.RegisterMethodCall("AddDriverInstance", volume, identifier);
        }

        public void RemoveDriverInstance(long identifier)
        {
            instances.Remove(identifier);

            base.RegisterMethodCall("RemoveDriverInstance", identifier);
        }

        public bool CheckMonitoredVolume(VolumeName volume, AccountSecurityIdentifier user)
        {
            Check.ObjectIsNotNull(volume);
            Check.ObjectIsNotNull(user);

            IEnumerable<bool> result = deviceAudit.Select(item => item.Value).Where(item => item.ContainsKey(volume)).Select(item => item[volume]).Where(item => item.ContainsKey(user)).Select(item => item[user]);

            if (result.Any())
                return result.First();

            return false;
        }

        public DeviceAccessType GetConfiguredAccessType(VolumeName volume, AccountSecurityIdentifier user)
        {
            Check.ObjectIsNotNull(volume);
            Check.ObjectIsNotNull(user);

            IEnumerable<DeviceAccessType> result = deviceAccess.Select(item => item.Value).Where(item => item.ContainsKey(volume)).Select(item => item[volume]).Where(item => item.ContainsKey(user)).Select(item => item[user]);

            if (result.Any())
                return result.First();

            return DeviceAccessType.Allowed;
        }

        public DeviceDescription GetDevice(VolumeName volume)
        {
            Check.ObjectIsNotNull(volume);

            if (NoDevices)
                return null;

            if (OverrideDevices.ContainsKey(volume))
                return OverrideDevices[volume];

            return new DeviceProviderImplementation().EnumerateDevices().Where(item => item.Category == DeviceCategory.Storage).First();
        }

        public ReadOnlySet<VolumeName> Volumes
        {
            get { return deviceAccess.SelectMany(item => item.Value.Keys).ToReadOnlySet(); }
        }

        public ReadOnlySet<long> Instances
        {
            get { return instances.Keys.ToReadOnlySet(); }
        }

        public void SetAccessRule(DeviceDescription device, VolumeName volume, AccountSecurityIdentifier user, DeviceAccessType access)
        {
            Check.ObjectIsNotNull(device);
            Check.ObjectIsNotNull(volume);
            Check.ObjectIsNotNull(user);
            Check.EnumerationValueIsDefined(access);

            base.RegisterMethodCall("SetAccessRule", device, volume, user, access);

            if (!deviceAccess.ContainsKey(device))
                deviceAccess[device] = new Dictionary<VolumeName, Dictionary<AccountSecurityIdentifier, DeviceAccessType>>();

            if (!deviceAccess[device].ContainsKey(volume))
                deviceAccess[device][volume] = new Dictionary<AccountSecurityIdentifier, DeviceAccessType>();

            deviceAccess[device][volume][user] = access;
        }

        public void SetAuditRule(DeviceDescription device, VolumeName volume, AccountSecurityIdentifier user, bool monitoring)
        {
            Check.ObjectIsNotNull(device);
            Check.ObjectIsNotNull(volume);
            Check.ObjectIsNotNull(user);

            base.RegisterMethodCall("SetAuditRule", device, volume, user, monitoring);

            if (!deviceAudit.ContainsKey(device))
                deviceAudit[device] = new Dictionary<VolumeName, Dictionary<AccountSecurityIdentifier, bool>>();

            if (!deviceAudit[device].ContainsKey(volume))
                deviceAudit[device][volume] = new Dictionary<AccountSecurityIdentifier, bool>();

            deviceAudit[device][volume][user] = monitoring;
        }

        public void Dispose()
        {
        }

        readonly Dictionary<VolumeName, DeviceDescription> overrideDevices = new Dictionary<VolumeName, DeviceDescription>();

        public Dictionary<VolumeName, DeviceDescription> OverrideDevices
        {
            get
            {
                return overrideDevices;
            }
        }
    }
}
