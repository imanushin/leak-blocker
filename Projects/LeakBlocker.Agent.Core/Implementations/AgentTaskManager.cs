using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Entities.Settings;
using LeakBlocker.Libraries.SystemTools;
using LeakBlocker.Libraries.SystemTools.Devices;
using LeakBlocker.Libraries.SystemTools.Drivers;
using LeakBlocker.ServerShared.AgentCommunication;

namespace LeakBlocker.Agent.Core.Implementations
{
    internal sealed class AgentTaskManager : IAgentTaskManager
    {
        private readonly IAuditStorage auditStorage;
        private readonly IDriverController driverController;
        private readonly IFileSystemAccessController fileSystemAccessController;
        private readonly IStateNotifier stateNotifier;
        private readonly IAgentDataStorage dataStorage;
        private readonly IAgentServiceController serviceController;

        private volatile AgentConfiguration lastConfiguration;
        
        internal AgentTaskManager(IAuditStorage auditStorage, IDriverController driverController, IAgentDataStorage dataStorage, IAgentServiceController serviceController)
        {
            Check.ObjectIsNotNull(auditStorage, "auditStorage");
            Check.ObjectIsNotNull(driverController, "driverController");
            Check.ObjectIsNotNull(dataStorage, "dataStorage");
            Check.ObjectIsNotNull(serviceController, "serviceController");

            this.auditStorage = auditStorage;
            this.driverController = driverController;
            this.dataStorage = dataStorage;
            this.serviceController = serviceController;

            using (new TimeMeasurement("Initializing file system access controller"))
            {
                fileSystemAccessController = AgentObjects.CreateFileSystemAccessController();
            }

            ReadOnlySet<BaseUserAccount> users = dataStorage.Users.Select(data => data.User).ToReadOnlySet();
            ReadOnlySet<DeviceDescription> devices = SystemObjects.DeviceProvider.EnumerateDevices();
            ReadOnlyDictionary<DeviceDescription, DeviceAccessType> deviceStates = dataStorage.DeviceStates;

            using (new TimeMeasurement("Initializing state notifier"))
            {
                stateNotifier = AgentObjects.CreateStateNotifier(auditStorage, users, devices, deviceStates);
            }

            stateNotifier.ServiceStart();
        }

        void IAgentTaskManager.OverrideConfiguration()
        {
            Log.Write("Configuration override.");
            IReadOnlyCollection<byte> data = SystemObjects.FileTools.ReadFile(AgentObjects.AgentConstants.AgentConfigurationOverrideFile);
            var configuration = BaseObjectSerializer.DeserializeFromXml<ProgramConfiguration>(data.ToArray());
            dataStorage.Settings = configuration;
            serviceController.QueryMainTask();
            SystemObjects.FileTools.Delete(AgentObjects.AgentConstants.AgentConfigurationOverrideFile);
        }

        void IAgentTaskManager.QueryTask()
        {
            dataStorage.Update();
            ReadOnlySet<BaseUserAccount> users = dataStorage.Users.Select(data => data.User).ToReadOnlySet();
            stateNotifier.SetNewUsers(users);
        }

        void IAgentTaskManager.SystemShutdownTask()
        {
            Log.Write("Sending shutdown notification.");
            SharedObjects.ExceptionSuppressor.Run(AgentObjects.AgentControlServiceClient.SendShutdownNotification);
            
            stateNotifier.SystemShutdown();
        }

        void IAgentTaskManager.SystemStartTask()
        {
            stateNotifier.SystemStart();
        }

        void IAgentTaskManager.NetworkTask(bool onlyUploadAudit)
        {
            AgentConfiguration configuration = null;

            try
            {
                auditStorage.Read(delegate(AuditItemPackage package)
                {
                    Log.Write("Sending data to the server ({0} audit items).".Combine(package.Count));
                    configuration = AgentObjects.AgentControlServiceClient.Synchronize(new AgentState(package, stateNotifier.DeviceAccess));
                });
            }
            catch (Exception exception)
            {
                Log.Write(exception);
            }

            if (configuration == null)
                stateNotifier.ServerInaccessible();
            else if (onlyUploadAudit)
                return;
            else
            {
                lastConfiguration = configuration;

                Log.Write("Received flags: Licensed {0}, Managed {1}.".Combine(configuration.Licensed, configuration.Managed));

                if (!configuration.Managed)
                    serviceController.Uninstall();
            }
        }

        void IAgentTaskManager.UnblockAllDevices()
        {
            Dictionary<ISystemDevice, DeviceDescription> devices = EnumerateDevices(true);

            foreach (KeyValuePair<ISystemDevice, DeviceDescription> currentItem in devices)
            {
                DeviceAccessType deviceAccessType;

                if(!dataStorage.DeviceStates.TryGetValue(currentItem.Value, out deviceAccessType))
                    continue;

                if ((deviceAccessType == DeviceAccessType.ReadOnly) || (deviceAccessType == DeviceAccessType.Blocked))
                    currentItem.Key.SetBlockedState(false);
            }
        }

        void IAgentTaskManager.MainTask()
        {
            if (lastConfiguration != null)
            {
                Log.Write("Updated flags: Licensed {0}, Managed {1}.".Combine(lastConfiguration.Licensed, lastConfiguration.Managed));

                dataStorage.Settings = lastConfiguration.Settings;
                AgentObjects.AgentPrivateStorage.Licensed = lastConfiguration.Licensed;
            }
            bool licensed = AgentObjects.AgentPrivateStorage.Licensed;
            
            Dictionary<ISystemDevice, DeviceDescription> devices = EnumerateDevices();
                
            IHardwareProfile profile = AgentObjects.CreateHardwareProfile(dataStorage.Computer, dataStorage.Users, devices.Values.ToReadOnlySet(), dataStorage.Settings);

            Log.Add("New profile:\r\n" + profile);
            
            using (IFileSystemAccessControllerUpdateSession accessControllerUpdateSession = fileSystemAccessController.GetUpdateSession())
            {
                if (licensed)
                {
                    foreach (Tuple<BaseUserAccount, DeviceDescription, DeviceAccessType> fileAccessItem in profile.FileAccess)
                    {
                        Tuple<BaseUserAccount, DeviceDescription, DeviceAccessType> currentItem = fileAccessItem;
                        foreach (VolumeName currentLogicalDisk in devices.Where(item => item.Value == currentItem.Item2).SelectMany(item => item.Key.LogicalDisks))
                            accessControllerUpdateSession.SetAccessRule(currentItem.Item2, currentLogicalDisk, currentItem.Item1.Identifier, currentItem.Item3);
                    }

                    foreach (Tuple<BaseUserAccount, DeviceDescription, bool> fileAuditItem in profile.FileAudit)
                    {
                        Tuple<BaseUserAccount, DeviceDescription, bool> currentItem = fileAuditItem;
                        foreach (VolumeName currentLogicalDisk in devices.Where(item => item.Value == currentItem.Item2).SelectMany(item => item.Key.LogicalDisks))
                            accessControllerUpdateSession.SetAuditRule(currentItem.Item2, currentLogicalDisk, currentItem.Item1.Identifier, currentItem.Item3);
                    }
                }
            }

            ReadOnlySet<KeyValuePair<ISystemDevice, DeviceDescription>> allUpdatedDevices = devices.Where(item => profile.BlockingStates.Keys.Contains(item.Value)).ToReadOnlySet();
            ReadOnlySet<KeyValuePair<ISystemDevice, DeviceDescription>> updatedDevices = allUpdatedDevices;

            Log.Write("Driver availability: {0}", driverController.Available);
            
            if (driverController.Available)
                updatedDevices = updatedDevices.Where(device => device.Value.Category != DeviceCategory.Storage).ToReadOnlySet();

            profile.SetCompletelyBlockedDevices(updatedDevices.Select(item => item.Value).ToReadOnlySet());
            stateNotifier.DeviceAccess = profile.AccessMap;

            Log.Add("License status: {0}.", licensed);
            updatedDevices.ForEach(device => device.Key.SetBlockedState(licensed && profile.BlockingStates[device.Value]));
            allUpdatedDevices.ForEach(device => dataStorage.SaveDeviceState(device.Value, licensed ? profile.DeviceStates[device.Value] : DeviceAccessType.AllowedNotLicensed));

            fileSystemAccessController.Instances.ForEach(SetDriverInstanceConfiguration);
            driverController.SetManagedVolumes(fileSystemAccessController.Volumes);
            stateNotifier.SetNewDevices(devices.Values.ToReadOnlySet());
            profile.DeviceStates.ForEach(state => stateNotifier.SetDeviceState(state.Key, licensed ? state.Value : DeviceAccessType.AllowedNotLicensed));

            serviceController.QueryDelayedNetworkTask();
        }

        private static Dictionary<ISystemDevice, DeviceDescription> EnumerateDevices(bool includeOffline = false)
        {
            ReadOnlySet<ISystemDevice> systemDevices = SystemObjects.DeviceProvider.EnumerateSystemDevices(includeOffline);
            var devices = new Dictionary<ISystemDevice, DeviceDescription>();

            foreach (ISystemDevice currentDevice in systemDevices)
            {
                ReadOnlyDictionary<ISystemDevice, DeviceDescription> converted = currentDevice.Convert();
                devices.MergeWith(converted, false);
            }

            return devices;
        }

        void IAgentTaskManager.DriverAttachedToVolume(IVolumeAttachMessage message)
        {
            fileSystemAccessController.AddDriverInstance(message.Name, message.InstanceIdentifier);
            SetDriverInstanceConfiguration(message.InstanceIdentifier);
        }

        void IAgentTaskManager.DriverDetachedFromVolume(IVolumeDetachMessage message)
        {
            fileSystemAccessController.RemoveDriverInstance(message.InstanceIdentifier);
        }

        void IAgentTaskManager.FileAccessNotification(IFileNotificationMessage message)
        {
            using (new TimeMeasurement("File access event processing ({0})".Combine(message.FileName), true))
            {
                DeviceDescription device = fileSystemAccessController.GetDevice(message.Volume);
                if (device == null)
                {
                    Log.Write("Device was not found.");
                    return;
                }

                if (message.Directory || !fileSystemAccessController.CheckMonitoredVolume(message.Volume, message.UserIdentifier))
                    return;

                if (message.ResultAction == FileActionType.Unknown)
                {
                    Log.Write("Unknown operation type.");
                    return;
                }

                DeviceAccessType accessType = fileSystemAccessController.GetConfiguredAccessType(message.Volume, message.UserIdentifier);
                AgentFileActionType action = EnumerationConverter.GetAgentFileActionType(message.ResultAction, accessType);

                BaseUserAccount currentUser = dataStorage.Users.Select(data => data.User).FirstOrDefault(user => user.Identifier == message.UserIdentifier);

                string volumeName = message.Volume;
                string fileName = message.FileName;
                if (message.FileName.StartsWith(volumeName, StringComparison.OrdinalIgnoreCase))
                    fileName = fileName.Substring(volumeName.Length);
                fileName = fileName.TrimStart('\\');
                if (fileName.Length == 0)
                    return;

                stateNotifier.FileAccessed(device, fileName, action, message.ProcessName, currentUser);
            }
        }

        void IAgentTaskManager.ServiceStopTask()
        {
            stateNotifier.ServiceStop();
        }

        private void SetDriverInstanceConfiguration(long instance)
        {
            ReadOnlyDictionary<AccountSecurityIdentifier, DeviceAccessType> accessRules = fileSystemAccessController.GetDriverInstanceConfiguration(instance);
            ReadOnlyDictionary<AccountSecurityIdentifier, FileAccessType> driverSettings = accessRules.ToDictionary(item => item.Key, item =>
                EnumerationConverter.GetFileAccessType(item.Value)).ToReadOnlyDictionary();

            driverController.SetInstanceConfiguration(instance, driverSettings);

            Log.Write("Applying driver instance ({0}) settings: {1}\r\n".Combine(instance, string.Join("\r\n", driverSettings.Select(item => item.Key.ToString() + " " + item.Value))));
        }
    }
}
