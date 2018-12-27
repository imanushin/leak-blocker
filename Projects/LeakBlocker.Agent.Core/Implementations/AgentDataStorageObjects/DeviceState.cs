using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities;

namespace LeakBlocker.Agent.Core.Implementations.AgentDataStorageObjects
{
    [DataContract(IsReference = true)]
    internal sealed class DeviceState : BaseReadOnlyObject
    {
        [DataMember]
        public DeviceDescription Device
        {
            get;
            private set;
        }

        [DataMember]
        public DeviceAccessType State
        {
            get;
            private set;
        }

        public DeviceState(DeviceDescription device, DeviceAccessType state)
        {
            Check.ObjectIsNotNull(device, "device");
            Check.EnumerationValueIsDefined(state, "state");

            Device = device;
            State = state;
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return Device;
        }
    }
}
