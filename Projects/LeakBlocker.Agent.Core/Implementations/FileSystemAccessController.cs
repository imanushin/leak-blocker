using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.SystemTools.Devices;

namespace LeakBlocker.Agent.Core.Implementations
{
    internal sealed class FileSystemAccessController : IFileSystemAccessController
    {
        #region FileSystemAccessControllerUpdateSession

        private sealed class FileSystemAccessControllerUpdateSession : Disposable, IFileSystemAccessControllerUpdateSession
        {
            private readonly FileSystemAccessController controller;
            private readonly Dictionary<VolumeName, VolumeConfiguration> fileSystemDeviceConfigurations = new Dictionary<VolumeName, VolumeConfiguration>();
            
            internal FileSystemAccessControllerUpdateSession(FileSystemAccessController controller)
            {
                Check.ObjectIsNotNull(controller, "controller");

                controller.updateSession = new Tuple<IFileSystemAccessControllerUpdateSession>(this);
                this.controller = controller;

                Log.Write("File system access controller update session was opened.");
            }

            public void SetAccessRule(DeviceDescription device, VolumeName volume, AccountSecurityIdentifier user, DeviceAccessType access)
            {
                Check.ObjectIsNotNull(device, "device");
                Check.StringIsMeaningful(volume, "volume");
                Check.ObjectIsNotNull(user, "user");
                Check.EnumerationValueIsDefined(access, "access");

                lock (controller.synchronization)
                {
                    if (!fileSystemDeviceConfigurations.ContainsKey(volume))
                        fileSystemDeviceConfigurations[volume] = new VolumeConfiguration(device);

                    fileSystemDeviceConfigurations[volume].Access[user] = access;
                }
            }

            public void SetAuditRule(DeviceDescription device, VolumeName volume, AccountSecurityIdentifier user, bool monitoring)
            {
                Check.ObjectIsNotNull(device, "device");
                Check.StringIsMeaningful(volume, "volume");
                Check.ObjectIsNotNull(user, "user");

                lock (controller.synchronization)
                {
                    if (!fileSystemDeviceConfigurations.ContainsKey(volume))
                        fileSystemDeviceConfigurations[volume] = new VolumeConfiguration(device);

                    fileSystemDeviceConfigurations[volume].Monitoring[user] = monitoring;
                }
            }

            protected override void DisposeManaged()
            {
                lock (controller.synchronization)
                {
                    controller.fileSystemDeviceConfigurations.Clear();
                    controller.fileSystemDeviceConfigurations.AddRange(fileSystemDeviceConfigurations);
                    controller.updateSession = null;

                    Log.Write("File system access controller update session was closed. Applied settings: {0}".Combine(string.Join("\r\n", 
                        fileSystemDeviceConfigurations.Select(item => "Volume: " + item.Key + " " + item.Value))));
                }
            }
        }

        #endregion

        #region VolumeConfiguration

        private sealed class VolumeConfiguration
        {
            public DeviceDescription Device
            {
                get;
                private set;
            }

            public Dictionary<AccountSecurityIdentifier, bool> Monitoring
            {
                get;
                private set;
            }

            public Dictionary<AccountSecurityIdentifier, DeviceAccessType> Access
            {
                get;
                private set;
            }

            internal VolumeConfiguration(DeviceDescription device)
            {
                Check.ObjectIsNotNull(device, "device");

                Device = device;
                Monitoring = new Dictionary<AccountSecurityIdentifier, bool>();
                Access = new Dictionary<AccountSecurityIdentifier, DeviceAccessType>();
            }

            public override string ToString()
            {
                var userData = new HashSet<string>();

                foreach(AccountSecurityIdentifier user in Monitoring.Keys.Union(Access.Keys).Distinct())
                {
                    string access = "Not configured";
                    string monitoring = "Not configured";

                    if(Monitoring.ContainsKey(user))
                        monitoring = Monitoring[user].ToString();
                    if(Access.ContainsKey(user))
                        access = Access[user].ToString();

                    userData.Add("   User: {0} Access: {1} Monitoring: {2}".Combine(user, access, monitoring));
                }

                return "Device: " + Device + "\r\n" + string.Join("\r\n", userData);
            }
        }

        #endregion

        private readonly Dictionary<VolumeName, VolumeConfiguration> fileSystemDeviceConfigurations = new Dictionary<VolumeName, VolumeConfiguration>();
        private readonly Dictionary<long, VolumeName> driverInstances = new Dictionary<long, VolumeName>();

        private readonly object synchronization = new object();

        private Tuple<IFileSystemAccessControllerUpdateSession> updateSession;

        ReadOnlySet<VolumeName> IFileSystemAccessController.Volumes
        {
            get
            {
                lock (synchronization)
                {
                    return fileSystemDeviceConfigurations.Keys.ToReadOnlySet();
                }
            }
        }

        ReadOnlySet<long> IFileSystemAccessController.Instances
        {
            get
            {
                lock (synchronization)
                {
                    return driverInstances.Keys.ToReadOnlySet();
                }
            }
        }

        ReadOnlyDictionary<AccountSecurityIdentifier, DeviceAccessType> IFileSystemAccessController.GetDriverInstanceConfiguration(long instance)
        {
            lock (synchronization)
            {
                VolumeName volumeName = driverInstances.TryGetValue(instance);

                if (volumeName == null)
                {
                    Log.Write("Unknown volume instance.");
                    return ReadOnlyDictionary<AccountSecurityIdentifier, DeviceAccessType>.Empty;
                }

                VolumeConfiguration result = fileSystemDeviceConfigurations.TryGetValue(volumeName);
                if (result == null)
                {
                    Log.Write("Volume configuration was not initialized.");
                    return ReadOnlyDictionary<AccountSecurityIdentifier, DeviceAccessType>.Empty;
                }

                return result.Access.ToReadOnlyDictionary();
            }
        }

        IFileSystemAccessControllerUpdateSession IFileSystemAccessController.GetUpdateSession()
        {
            lock (synchronization)
            {
                if (updateSession != null)
                    Exceptions.Throw(ErrorMessage.InvalidHandleState, "Previous update session is still active.");

                return new FileSystemAccessControllerUpdateSession(this);
            }
        }

        void IFileSystemAccessController.AddDriverInstance(VolumeName volume, long identifier)
        {
            Check.StringIsMeaningful(volume, "volume");

            lock (synchronization)
            {
                driverInstances[identifier] = volume;

                Log.Write("Driver instance {0} ({1}) was added. Current instances: {2}\r\n".Combine(volume, identifier, string.Join("\r\n",
                    driverInstances.Select(item => item.Value + " ({0})".Combine(item.Key)))));
            }
        }

        void IFileSystemAccessController.RemoveDriverInstance(long identifier)
        {
            lock (synchronization)
            {
                driverInstances.Remove(identifier);

                Log.Write("Driver instance {0} was removed. Current instances: {1}\r\n".Combine(identifier, string.Join("\r\n",
                    driverInstances.Select(item => item.Value + " ({0})".Combine(item.Key)))));
            }
        }

        bool IFileSystemAccessController.CheckMonitoredVolume(VolumeName volume, AccountSecurityIdentifier user)
        {
            Check.ObjectIsNotNull(volume, "volume");
            Check.ObjectIsNotNull(user, "user");

            lock (synchronization)
            {
                return fileSystemDeviceConfigurations.Any(item => volume.Equals(item.Key) &&
                    item.Value.Monitoring.Any(monitoring => (monitoring.Key == user) && monitoring.Value));
            }
        }

        DeviceAccessType IFileSystemAccessController.GetConfiguredAccessType(VolumeName volume, AccountSecurityIdentifier user)
        {
            Check.ObjectIsNotNull(volume, "volume");
            Check.ObjectIsNotNull(user, "user");

            lock (synchronization)
            {
                IEnumerable<VolumeConfiguration> configurations = fileSystemDeviceConfigurations.Where(item => volume.Equals(item.Key)).Select(item => item.Value);
                IEnumerable<DeviceAccessType> accessTypes = configurations.SelectMany(configuration =>
                    configuration.Access.Where(item => item.Key == user).Select(item => item.Value));

                DeviceAccessType result = accessTypes.FirstOrDefault();
                return (result == default(DeviceAccessType)) ? DeviceAccessType.Allowed : result;
            }
        }

        DeviceDescription IFileSystemAccessController.GetDevice(VolumeName volume)
        {
            Check.ObjectIsNotNull(volume, "volume");

            lock (synchronization)
            {
                VolumeConfiguration result = fileSystemDeviceConfigurations.FirstOrDefault(item => volume.Equals(item.Key)).Value;
                return (result == null) ? null : result.Device;
            }
        }
    }
}
