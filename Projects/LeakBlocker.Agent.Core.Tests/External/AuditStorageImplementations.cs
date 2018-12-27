using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Audit;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.ServerShared.AgentCommunication;

namespace LeakBlocker.Agent.Core.Tests.External
{
    class AuditStorageImplementation : BaseTestImplementation, IAuditStorage
    {
        readonly AuditItemPackage package;

        public AuditStorageImplementation(AuditItemPackage package = null)
        {
            this.package = package;
        }

        public void Read(Action<AuditItemPackage> processingCallback)
        {
            Check.ObjectIsNotNull(processingCallback);
            processingCallback(package ?? new AuditItemPackage(new AgentAuditItem[] { new AgentAuditItem(AuditItemType.AgentComputerTurnedOn, new Time(new DateTime(2000, 1, 1))) }.ToReadOnlySet()));
        }

        public void ServiceStarted()
        {
            base.RegisterMethodCall("ServiceStarted");
        }

        public void ServiceStopped()
        {
            base.RegisterMethodCall("ServiceStopped");
        }

        public void SystemStarted()
        {
            base.RegisterMethodCall("SystemStarted");
        }

        public void SystemShutdown()
        {
            base.RegisterMethodCall("SystemShutdown");
        }

        public void UnplannedServiceShutdown()
        {
            base.RegisterMethodCall("UnplannedServiceShutdown");
        }

        public void ServerInaccessible()
        {
            base.RegisterMethodCall("ServerInaccessible");
        }

        public void FileAccessed(DeviceDescription device, string file, AgentFileActionType state, string process, BaseUserAccount user)
        {
            Check.ObjectIsNotNull(device);
            Check.StringIsMeaningful(file);
            Check.EnumerationValueIsDefined(state);
            Check.StringIsMeaningful(process);
            Check.ObjectIsNotNull(user);

            base.RegisterMethodCall("FileAccessed", device, file, state, process, user);
        }

        public void DeviceAdded(DeviceDescription device)
        {
            Check.ObjectIsNotNull(device);

            base.RegisterMethodCall("DeviceAdded", device);
        }

        public void DeviceRemoved(DeviceDescription device)
        {
            Check.ObjectIsNotNull(device);

            base.RegisterMethodCall("DeviceRemoved", device);
        }

        public void UserLoggedOn(BaseUserAccount user)
        {
            Check.ObjectIsNotNull(user);

            base.RegisterMethodCall("UserLoggedOn", user);
        }

        public void UserLoggedOff(BaseUserAccount user)
        {
            Check.ObjectIsNotNull(user);

            base.RegisterMethodCall("UserLoggedOff", user);
        }

        public void DeviceStateChanged(DeviceDescription device, DeviceAccessType state)
        {
            Check.ObjectIsNotNull(device);
            Check.EnumerationValueIsDefined(state);

            base.RegisterMethodCall("DeviceAdded", device, state);
        }
    }
}
