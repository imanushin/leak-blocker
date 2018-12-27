using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Audit;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.SystemTools;
using LeakBlocker.ServerShared.AgentCommunication;

namespace LeakBlocker.Agent.Core.Implementations
{
    internal sealed class AuditStorage : IAuditStorage
    {
        private const int packageSize = 1024 * 1024;

        private readonly IStackStorage stackStorage;
        private readonly IAgentDataStorage dataStorage;

        internal AuditStorage(IStackStorage stackStorage, IAgentDataStorage dataStorage)
        {
            Check.ObjectIsNotNull(stackStorage, "stackStorage");
            Check.ObjectIsNotNull(dataStorage, "dataStorage");

            this.stackStorage = stackStorage;
            this.dataStorage = dataStorage;

            Log.Write("Audit storage was initialized.");
        }

        void IAuditStorage.Read(Action<AuditItemPackage> processingCallback)
        {
            using (new TimeMeasurement("Reading audit package"))
            {
                Check.ObjectIsNotNull(processingCallback, "processingCallback");

                stackStorage.Read(packageSize, delegate(IReadOnlyCollection<string> values)
                {
                    ReadOnlySet<AgentAuditItem> items = values.Select(Convert.FromBase64String).Select(BaseObjectSerializer.DeserializeFromXml<AgentAuditItem>).ToReadOnlySet();
                    processingCallback(new AuditItemPackage(items));
                });
            }
        }

        void IStateNotifierHandler.ServiceStarted()
        {
            Log.Add("Service started notification.");
            Write(new AgentAuditItem(AuditItemType.AgentStarted, SystemObjects.TimeProvider.CurrentTime));
        }

        void IStateNotifierHandler.ServiceStopped()
        {
            Log.Add("Service stopped notification.");
            Write(new AgentAuditItem(AuditItemType.AgentStopped, SystemObjects.TimeProvider.CurrentTime));
        }

        void IStateNotifierHandler.SystemStarted()
        {
            Log.Add("System started notification.");
            Write(new AgentAuditItem(AuditItemType.ComputerTurnedOn, SystemObjects.TimeProvider.CurrentTime));
        }

        void IStateNotifierHandler.SystemShutdown()
        {
            Log.Add("System shutdown notification.");
            Write(new AgentAuditItem(AuditItemType.ComputerTurnedOff, SystemObjects.TimeProvider.CurrentTime));
        }

        void IStateNotifierHandler.UnplannedServiceShutdown()
        {
            Log.Add("Unplanned service shutdown notification.");
            Write(new AgentAuditItem(AuditItemType.UnplannedServiceShutdown, SystemObjects.TimeProvider.CurrentTime));
        }

        void IStateNotifierHandler.ServerInaccessible()
        {
            Log.Add("Server inaccessible notification.");
            Write(new AgentAuditItem(AuditItemType.ServerInaccessible, SystemObjects.TimeProvider.CurrentTime));
        }

        void IStateNotifierHandler.FileAccessed(DeviceDescription device, string file, AgentFileActionType state, string process, BaseUserAccount user)
        {
            Check.ObjectIsNotNull(device, "device");
            Check.StringIsMeaningful(file, "file");
            Check.EnumerationValueIsDefined(state, "state");
            Check.StringIsMeaningful(process, "process");
            Check.ObjectIsNotNull(user, "user");

            AuditItemType? eventType = null;

            switch (state)
            {
                case AgentFileActionType.ReadAllowed:
                    eventType = AuditItemType.ReadFileAllowed;
                    break;
                case AgentFileActionType.ReadBlocked:
                    eventType = AuditItemType.ReadFileBlocked;
                    break;
                case AgentFileActionType.ReadTemporarilyAllowed:
                    eventType = AuditItemType.ReadFileTemporarilyAllowed;
                    break;
                case AgentFileActionType.ReadWriteAllowed:
                    eventType = AuditItemType.ReadWriteFileAllowed;
                    break;
                case AgentFileActionType.ReadWriteBlocked:
                    eventType = AuditItemType.ReadWriteFileBlocked;
                    break;
                case AgentFileActionType.ReadWriteTemporarilyAllowed:
                    eventType = AuditItemType.ReadWriteFileTemporarilyAllowed;
                    break;
                case AgentFileActionType.WriteAllowed:
                    eventType = AuditItemType.WriteFileAllowed;
                    break;
                case AgentFileActionType.WriteBlocked:
                    eventType = AuditItemType.WriteFileBlocked;
                    break;
                case AgentFileActionType.WriteTemporarilyAllowed:
                    eventType = AuditItemType.WriteFileTemporarilyAllowed;
                    break;
            }

            Log.Add("File accessed notification.");

            if (eventType.HasValue)
                Write(new AgentAuditItem(eventType.Value, SystemObjects.TimeProvider.CurrentTime, user, file, process, device, (dataStorage.Settings == null) ? 0 : dataStorage.Settings.ConfigurationVersion));
            else
                Log.Write("File access event {0} is not supported.".Combine(state));
        }

        void IStateNotifierHandler.DeviceAdded(DeviceDescription device)
        {
            Check.ObjectIsNotNull(device, "device");

            Log.Add("Device added notification.");

            Write(new AgentAuditItem(AuditItemType.DeviceConnected, SystemObjects.TimeProvider.CurrentTime, dataStorage.ConsoleUser, null, null, device));
        }

        void IStateNotifierHandler.DeviceRemoved(DeviceDescription device)
        {
            Check.ObjectIsNotNull(device, "device");

            Log.Add("Device removed notification.");

            Write(new AgentAuditItem(AuditItemType.DeviceDisconnected, SystemObjects.TimeProvider.CurrentTime, dataStorage.ConsoleUser, null, null, device));
        }

        void IStateNotifierHandler.UserLoggedOn(BaseUserAccount user)
        {
            Check.ObjectIsNotNull(user, "user");

            Log.Add("User logged on notification.");

            Write(new AgentAuditItem(AuditItemType.UserLoggedOn, SystemObjects.TimeProvider.CurrentTime, user));
        }

        void IStateNotifierHandler.UserLoggedOff(BaseUserAccount user)
        {
            Check.ObjectIsNotNull(user, "user");

            Log.Add("User logged off notification.");

            Write(new AgentAuditItem(AuditItemType.UserLoggedOff, SystemObjects.TimeProvider.CurrentTime, user));
        }

        void IStateNotifierHandler.DeviceStateChanged(DeviceDescription device, DeviceAccessType state)
        {
            Check.ObjectIsNotNull(device, "device");
            Check.EnumerationValueIsDefined(state, "state");
            
            AuditItemType? eventType = LinkedEnumHelper<DeviceAccessType, AuditItemType>.TryGetLinkedEnum(state);

            Log.Add("Device state changed notification.");

            if (!eventType.HasValue)
                Log.Write("Device state event {0} is not supported.".Combine(state));
            else
            {
                Write(new AgentAuditItem(eventType.Value, SystemObjects.TimeProvider.CurrentTime, dataStorage.ConsoleUser,
                    null, null, device, (dataStorage.Settings == null) ? 0 : dataStorage.Settings.ConfigurationVersion));
            }
        }

        private void Write(AgentAuditItem auditItem)
        {
            Check.ObjectIsNotNull(auditItem, "auditItem");

            switch (auditItem.EventType)
            {
                case AuditItemType.ReadFileBlocked:
                case AuditItemType.ReadFileAllowed:
                case AuditItemType.ReadFileTemporarilyAllowed:
                case AuditItemType.WriteFileBlocked:
                case AuditItemType.WriteFileAllowed:
                case AuditItemType.WriteFileTemporarilyAllowed:
                case AuditItemType.ReadWriteFileBlocked:
                case AuditItemType.ReadWriteFileAllowed:
                case AuditItemType.ReadWriteFileTemporarilyAllowed:
                case AuditItemType.DeviceAccessBlocked:
                case AuditItemType.DeviceAccessReadOnly:
                case AuditItemType.DeviceAccessAllowed:
                case AuditItemType.DeviceAccessTemporarilyReadOnly:
                case AuditItemType.DeviceAccessTemporarilyAllowed:
                case AuditItemType.DeviceAccessAllowedNotLicensed:
                    if (dataStorage.Settings != null)
                        break;
                    Log.Write("Event {0} will be skipped because settings were not received.", auditItem.EventType);
                    return;
            }

            string serialized = Convert.ToBase64String(auditItem.SerializeToXml());
            stackStorage.Write(new[] { serialized }.ToReadOnlyList());

            Log.Write("Event {0} was written.".Combine(auditItem.EventType));
        }
    }
}
