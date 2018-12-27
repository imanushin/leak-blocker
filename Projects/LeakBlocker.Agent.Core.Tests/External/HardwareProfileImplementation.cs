using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Entities.Settings;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules;

namespace LeakBlocker.Agent.Core.Tests.External
{
    class HardwareProfileImplementation : IHardwareProfile
    {
        public DeviceAccessMap AccessMap
        {
            get;
            set;
        }

        ReadOnlyDictionary<DeviceDescription, DeviceAccessType> IHardwareProfile.DeviceStates
        {
            get 
            {
                Dictionary<DeviceDescription, DeviceAccessType> result = new Dictionary<DeviceDescription, DeviceAccessType>();
                foreach (var item in AccessMap)
                {
                    if (!result.ContainsKey(item.Item2))
                    {
                        result[item.Item2] = item.Item3;
                        continue;
                    }

                    result[item.Item2] = ModifyDeviceState(result[item.Item2], item.Item3);
                }
                return result.ToReadOnlyDictionary();
            }
        }


        private static DeviceAccessType ModifyDeviceState(DeviceAccessType oldState, DeviceAccessType newState)
        {
            switch (oldState)
            {
                case DeviceAccessType.TemporarilyAllowed:
                case DeviceAccessType.Allowed:
                    return newState;
                case DeviceAccessType.Blocked:
                    return oldState;
                case DeviceAccessType.ReadOnly:
                case DeviceAccessType.TemporarilyAllowedReadOnly:
                    if (newState == DeviceAccessType.Blocked)
                        return newState;
                    return oldState;
                default:
                    return DeviceAccessType.Blocked;
            }
        }

        ReadOnlyDictionary<DeviceDescription, bool> IHardwareProfile.BlockingStates
        {
            get
            {
                return ((IHardwareProfile)this).DeviceStates.ToDictionary(item => item.Key, item => item.Value == DeviceAccessType.ReadOnly || item.Value == DeviceAccessType.Blocked).ToReadOnlyDictionary();
            }
        }

        public ReadOnlyMatrix<BaseUserAccount, DeviceDescription, bool> FileAudit
        {
            get;
            set;
        }

        public void SetAuditMap(AuditMap map)
        {
            FileAudit = new ReadOnlyMatrix<BaseUserAccount, DeviceDescription, bool>(map.Keys1.ToReadOnlySet(), map.Keys2.ToReadOnlySet(), 
                (user, device) => (map.First(item => item.Item1 == user && item.Item2 == device).Item3 == AuditActionType.DeviceAndFiles));
        }

        ReadOnlyMatrix<BaseUserAccount, DeviceDescription, DeviceAccessType> IHardwareProfile.FileAccess
        {
            get
            {
                return new ReadOnlyMatrix<BaseUserAccount, DeviceDescription, DeviceAccessType>(AccessMap.Keys1.ToReadOnlySet(), AccessMap.Keys2.ToReadOnlySet(), (user, device) => AccessMap[user, device]);
            }
        }

        public HardwareProfileImplementation()
        {
            AccessMap = new DeviceAccessMap(ReadOnlySet<BaseUserAccount>.Empty, ReadOnlySet<DeviceDescription>.Empty, (q, w) => { throw new Exception(); });
            FileAudit = new ReadOnlyMatrix<BaseUserAccount, DeviceDescription, bool>(ReadOnlySet<BaseUserAccount>.Empty, ReadOnlySet<DeviceDescription>.Empty, (q, w) => { throw new Exception(); });
        }


        public void SetCompletelyBlockedDevices(IReadOnlyCollection<DeviceDescription> devices)
        {
        }
    }
}
