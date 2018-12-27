using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.SystemTools;

namespace LeakBlocker.Agent.Core.Implementations
{
    internal sealed class StateNotifier : IStateNotifier
    {
        private DeviceAccessMap lastDeviceAccessState = DeviceAccessMap.Empty;

        private ReadOnlySet<BaseUserAccount> users;
        private ReadOnlySet<DeviceDescription> devices;
        private readonly Dictionary<DeviceDescription, DeviceAccessType> deviceStates;

        private readonly IStateNotifierHandler handler;
        private readonly object synchronization = new object();

        private bool startHandled;
        private bool stopHandled;

        DeviceAccessMap IStateNotifier.DeviceAccess
        {
            get
            {
                return lastDeviceAccessState;
            }
            set
            {
                Check.ObjectIsNotNull(value);

                lock (synchronization)
                {
                    Log.Write("Updating device access map.");
                    lastDeviceAccessState = value;
                }
            }
        }

        internal StateNotifier(IStateNotifierHandler handler, IReadOnlyCollection<BaseUserAccount> users, IReadOnlyCollection<DeviceDescription> devices,
            ReadOnlyDictionary<DeviceDescription, DeviceAccessType> deviceStates)
        {
            Check.ObjectIsNotNull(handler, "handler");
            Check.CollectionHasNoDefaultItems(users, "users");
            Check.CollectionHasNoDefaultItems(devices, "devices");

            this.handler = handler;
            this.devices = devices.ToReadOnlySet();
            this.users = users.ToReadOnlySet();
            this.deviceStates = new Dictionary<DeviceDescription, DeviceAccessType>(deviceStates);

            Log.Write("Notification manager was initialized.");
        }

        void IStateNotifier.SetNewUsers(IReadOnlyCollection<BaseUserAccount> newUsers)
        {
            Check.CollectionHasNoDefaultItems(newUsers, "newUsers");

            lock (synchronization)
            {
                ReadOnlySet<BaseUserAccount> addedUsers = newUsers.Without(users).ToReadOnlySet();
                ReadOnlySet<BaseUserAccount> removedUsers = users.Without(newUsers).ToReadOnlySet();

                if(addedUsers.Any())
                    Log.Write("Added users notifications (Users: {0}).".Combine(string.Join(", ", addedUsers.Select(user => user.ToString()))));
                if (removedUsers.Any())
                    Log.Write("Removed users notifications (Users: {0}).".Combine(string.Join(", ", removedUsers.Select(user => user.ToString()))));
                if (!addedUsers.Any() && !removedUsers.Any())
                    Log.Write("No new users.");

                addedUsers.ForEach(user => SharedObjects.ExceptionSuppressor.Run(handler.UserLoggedOn, user));
                removedUsers.ForEach(user => SharedObjects.ExceptionSuppressor.Run(handler.UserLoggedOff, user));

                users = newUsers.ToReadOnlySet();
            }
        }

        void IStateNotifier.SetNewDevices(IReadOnlyCollection<DeviceDescription> newDevices)
        {
            Check.CollectionHasNoDefaultItems(newDevices, "newDevices");

            lock (synchronization)
            {
                ReadOnlySet<DeviceDescription> addedDevices = newDevices.Without(devices).ToReadOnlySet();
                ReadOnlySet<DeviceDescription> removedDevices = devices.Without(newDevices).ToReadOnlySet();

                if (addedDevices.Any())
                    Log.Write("Added devices notifications (Devices: {0}).".Combine(string.Join(", ", addedDevices.Select(device => device.ToString()))));
                if (removedDevices.Any())
                    Log.Write("Removed devices notifications (Devices: {0}).".Combine(string.Join(", ", removedDevices.Select(device => device.ToString()))));
                if (!addedDevices.Any() && !removedDevices.Any())
                    Log.Write("No new devices.");

                addedDevices.ForEach(device => SharedObjects.ExceptionSuppressor.Run(handler.DeviceAdded, device));
                removedDevices.ForEach(device => SharedObjects.ExceptionSuppressor.Run(handler.DeviceRemoved, device));

                devices = newDevices.ToReadOnlySet();
            }
        }

        void IStateNotifier.SetDeviceState(DeviceDescription device, DeviceAccessType state)
        {
            Check.ObjectIsNotNull(device, "device");
            Check.EnumerationValueIsDefined(state, "state");

            lock (synchronization)
            {
                if (deviceStates.TryGetValue(device) == state)
                    return;

                deviceStates[device] = state;

                Log.Write("Device state update notification (State: {0} Device: {1}).".Combine(state, device));
                SharedObjects.ExceptionSuppressor.Run(handler.DeviceStateChanged, device, state);
            }
        }

        void IStateNotifier.ServiceStart()
        {
            lock (synchronization)
            {
                if (startHandled)
                    Exceptions.Throw(ErrorMessage.AuditWriteFailed, "Stop event was already handled.");

                Log.Write("Service start notification.");
                SharedObjects.ExceptionSuppressor.Run(handler.ServiceStarted);

                if (SystemObjects.FileTools.Exists(AgentObjects.AgentConstants.UnexpectedTerminationFlagFile, default(SystemAccessOptions)))
                {
                    Log.Write("Unplanned service shutdown notification.");
                    SharedObjects.ExceptionSuppressor.Run(handler.UnplannedServiceShutdown);
                }

                SharedObjects.ExceptionSuppressor.Run(SystemObjects.FileTools.WriteFile, AgentObjects.AgentConstants.UnexpectedTerminationFlagFile,
                    new byte[1].ToReadOnlyList(), default(SystemAccessOptions));

                startHandled = true;
                Log.Write("Service start event was registered.");
            }
        }

        void IStateNotifier.ServiceStop()
        {
            lock (synchronization)
            {
                if (stopHandled)
                    Exceptions.Throw(ErrorMessage.AuditWriteFailed, "Stop event was already handled.");

                Log.Write("Service stop notification.");
                SharedObjects.ExceptionSuppressor.Run(handler.ServiceStopped);

                SharedObjects.ExceptionSuppressor.Run(SystemObjects.FileTools.Delete, AgentObjects.AgentConstants.UnexpectedTerminationFlagFile, default(SystemAccessOptions));

                stopHandled = true;
                Log.Write("Service stop event was registered.");
            }
        }

        void IStateNotifier.SystemStart()
        {
            Log.Write("System start notification.");
            SharedObjects.ExceptionSuppressor.Run(handler.SystemStarted);
        }

        void IStateNotifier.SystemShutdown()
        {
            Log.Write("System shutdown notification.");
            SharedObjects.ExceptionSuppressor.Run(handler.SystemShutdown);
        }

        void IStateNotifier.ServerInaccessible()
        {
            Log.Write("Server inaccessible notification.");
            SharedObjects.ExceptionSuppressor.Run(handler.ServerInaccessible);
        }

        void IStateNotifier.FileAccessed(DeviceDescription device, string file, AgentFileActionType state, string process, BaseUserAccount user)
        {
            Check.ObjectIsNotNull(device, "device");
            Check.ObjectIsNotNull(file, "file");
            Check.EnumerationValueIsDefined(state, "state");
            Check.ObjectIsNotNull(user, "user");

            Log.Add("File accessed notification.");
            SharedObjects.ExceptionSuppressor.Run(handler.FileAccessed, device, file, state, process, user);
        }
    }
}
