using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Tests.Entities;

namespace LeakBlocker.Agent.Core.Tests.External
{
    class StateNotifierImplementation : BaseTestImplementation, IStateNotifier
    {
        private DeviceAccessMap deviceAccess;

        public DeviceAccessMap DeviceAccess
        {
            get
            {
                return deviceAccess ?? DeviceAccessMapTest.First;
            }
            set
            {
                Check.ObjectIsNotNull(value);
                base.RegisterMethodCall("DeviceAccess", value);
                deviceAccess = value;
            }
        }

        public void ServiceStart()
        {
            base.RegisterMethodCall("Start");
        }

        public void ServiceStop()
        {
            base.RegisterMethodCall("Stop");
        }

        public void SystemStart()
        {
            base.RegisterMethodCall("SystemStart");
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
            Check.EnumerationValueIsDefined(state);
            Check.ObjectIsNotNull(user);
            Check.StringIsMeaningful(file);
            Check.StringIsMeaningful(process);

            base.RegisterMethodCall("FileAccessed", device, file, state, process, user);
        }

        public void SetNewUsers(IReadOnlyCollection<BaseUserAccount> newUsers)
        {
            Check.CollectionHasNoDefaultItems(newUsers);

            base.RegisterMethodCall("SetNewUsers", newUsers);
        }

        public void SetNewDevices(IReadOnlyCollection<DeviceDescription> newDevices)
        {
            Check.CollectionHasNoDefaultItems(newDevices);

            base.RegisterMethodCall("SetNewDevices", newDevices);
        }

        public void SetDeviceState(DeviceDescription device, DeviceAccessType state)
        {
            Check.ObjectIsNotNull(device);
            Check.EnumerationValueIsDefined(state);

            base.RegisterMethodCall("SetDeviceState", device, state);
        }
    }
}
