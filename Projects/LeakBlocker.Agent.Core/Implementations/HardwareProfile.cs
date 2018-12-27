using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using LeakBlocker.Agent.Core.Settings;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Entities.Settings;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules;
using LeakBlocker.Libraries.SystemTools.Entities;

namespace LeakBlocker.Agent.Core.Implementations
{
    internal sealed class HardwareProfile : IHardwareProfile
    {
        public DeviceAccessMap AccessMap
        {
            get;
            private set;
        }

        public ReadOnlyDictionary<DeviceDescription, DeviceAccessType> DeviceStates
        {
            get;
            private set;
        }

        public ReadOnlyDictionary<DeviceDescription, bool> BlockingStates
        {
            get;
            private set;
        }

        public ReadOnlyMatrix<BaseUserAccount, DeviceDescription, bool> FileAudit
        {
            get;
            private set;
        }

        public ReadOnlyMatrix<BaseUserAccount, DeviceDescription, DeviceAccessType> FileAccess
        {
            get;
            private set;
        }

        internal HardwareProfile(CachedComputerData computer, IReadOnlyCollection<CachedUserData> users, IReadOnlyCollection<DeviceDescription> devices, ProgramConfiguration settings)
        {
            bool onlyServiceAccount = users.All(user => user.ServiceAccount);
            ReadOnlySet<CachedUserData> managedUsers = (onlyServiceAccount ? users : users.Where(user => !user.ServiceAccount)).ToReadOnlySet();
            ReadOnlySet<DeviceDescription> managedDevices = devices.Where(device => device.Category != DeviceCategory.Unsupported).ToReadOnlySet();

            var agentState = new AgentComputerState(
                computer.Computer,
                computer.Groups,
                managedUsers.ToReadOnlyDictionary(user => user.User, user => user.Groups),
                managedDevices);

            Log.Add("\r\nComputer: {0} ({1})\r\n{2}\r\n{3}", computer.Computer, string.Join(", ", computer.Groups.Select(group => group.ToString())),
                string.Join("\r\n", managedUsers.Select(user => "User: {0} ({1})".Combine(user.User.ToString(), string.Join(", ", user.Groups.Select(group => group.ToString()))))),
                string.Join("\r\n", managedDevices.Select(device => "Device: {0}".Combine(device.ToString()))));

            RuleCheckResult checkResult = (settings != null) ? AgentObjects.ProgramConfigurationChecker.GetRequiredActions(agentState, settings) :
                new RuleCheckResult(users.Select(user => user.User).ToReadOnlySet(), devices, (user, device) =>  new CommonActionData(DeviceAccessType.Allowed, AuditActionType.Skip));

            AccessMap = checkResult.AccessMap;
            if (onlyServiceAccount)
                AccessMap = new DeviceAccessMap(ReadOnlySet<BaseUserAccount>.Empty, managedDevices.ToReadOnlySet(), (user, device) => 0);

            var deviceStates = new Dictionary<DeviceDescription, DeviceAccessType>();
            var fileAccess = new List<Tuple<BaseUserAccount, DeviceDescription, DeviceAccessType>>();
            var fileAudit = new List<Tuple<BaseUserAccount, DeviceDescription, bool>>();

            foreach (DeviceDescription device in AccessMap.Keys2)
            {
                deviceStates[device] = DeviceAccessType.Allowed;

                foreach (BaseUserAccount user in AccessMap.Keys1)
                {
                    deviceStates[device] = ModifyDeviceState(deviceStates[device], AccessMap[user, device]);
                    fileAccess.Add(new Tuple<BaseUserAccount, DeviceDescription, DeviceAccessType>(user, device, deviceStates[device]));
                }
            }

            foreach (DeviceDescription device in checkResult.AuditMap.Keys2)
            {
                foreach (BaseUserAccount user in checkResult.AuditMap.Keys1)
                {
                    bool auditFiles = checkResult.AuditMap[user, device] == AuditActionType.DeviceAndFiles;
                    fileAudit.Add(new Tuple<BaseUserAccount, DeviceDescription, bool>(user, device, auditFiles));
                }
            }

            FileAudit = new ReadOnlyMatrix<BaseUserAccount, DeviceDescription, bool>(
                fileAccess.Select(item => item.Item1).ToReadOnlySet(),
                fileAccess.Select(item => item.Item2).ToReadOnlySet(),
                (user, device) => fileAudit.First(item => item.Item1.Equals(user) && item.Item2.Equals(device)).Item3);

            FileAccess = new ReadOnlyMatrix<BaseUserAccount, DeviceDescription, DeviceAccessType>(
                fileAccess.Select(item => item.Item1).ToReadOnlySet(),
                fileAccess.Select(item => item.Item2).ToReadOnlySet(),
                (user, device) => fileAccess.First(item => item.Item1.Equals(user) && item.Item2.Equals(device)).Item3);

            DeviceStates = deviceStates.ToReadOnlyDictionary();

            BlockingStates = deviceStates.ToDictionary(item => item.Key, item => GetFullBlockingState(item.Value)).ToReadOnlyDictionary();
        }
        
        private static bool GetFullBlockingState(DeviceAccessType accessType)
        {
            return (accessType == DeviceAccessType.Blocked) || (accessType == DeviceAccessType.ReadOnly) || (accessType == DeviceAccessType.TemporarilyAllowedReadOnly);
        }

        private static DeviceAccessType ModifyDeviceState(DeviceAccessType oldState, DeviceAccessType newState)
        {
            if (newState == DeviceAccessType.AllowedNotLicensed)
                return newState;

            switch (oldState)
            {
                case DeviceAccessType.TemporarilyAllowed:
                case DeviceAccessType.Allowed:
                    return newState;
                case DeviceAccessType.Blocked:
                    return oldState;
                case DeviceAccessType.ReadOnly:
                case DeviceAccessType.TemporarilyAllowedReadOnly:
                    return (newState == DeviceAccessType.Blocked) ? newState : oldState;
                default:
                    return DeviceAccessType.Blocked;
            }
        }

        public override string ToString()
        {
            var result = new StringBuilder();

            result.AppendLine();
            result.AppendLine("   AccessMap:");
            foreach (DeviceDescription device in AccessMap.Keys2)
            {
                result.AppendLine("      " + device);
                foreach (BaseUserAccount user in AccessMap.Keys1)
                    result.AppendLine("         " + AccessMap[user, device] + " " + user);
            }
            result.AppendLine("   FileAudit:");
            foreach (DeviceDescription device in FileAudit.Keys2)
            {
                result.AppendLine("      " + device);
                foreach (BaseUserAccount user in FileAudit.Keys1)
                    result.AppendLine("         " + FileAudit[user, device] + " " + user);
            }
            result.AppendLine("   FileAccess:");
            foreach (DeviceDescription device in FileAccess.Keys2)
            {
                result.AppendLine("      " + device);
                foreach (BaseUserAccount user in FileAccess.Keys1)
                    result.AppendLine("         " + FileAccess[user, device] + " " + user);
            }
            result.AppendLine("   DeviceStates:");
            foreach (KeyValuePair<DeviceDescription, DeviceAccessType> currentItem in DeviceStates)
                result.AppendLine("      " + currentItem.Value + " " + currentItem.Key);
            result.AppendLine("   BlockingStates:");
            foreach (KeyValuePair<DeviceDescription, bool> currentItem in BlockingStates)
                result.AppendLine("      " + currentItem.Value + " " + currentItem.Key);
            return result.ToString();
        }


        public void SetCompletelyBlockedDevices(IReadOnlyCollection<DeviceDescription> devices)
        {
            Check.ObjectIsNotNull(devices, "devices");

            Func<DeviceDescription, DeviceAccessType, bool> requireUpdate = (device, access) => (GetFullBlockingState(access) && devices.Contains(device));

            var newStates = new Dictionary<DeviceDescription, DeviceAccessType>();
            foreach (KeyValuePair<DeviceDescription, DeviceAccessType> state in DeviceStates)
                newStates[state.Key] = requireUpdate(state.Key, state.Value) ? DeviceAccessType.Blocked : state.Value;
            DeviceStates = newStates.ToReadOnlyDictionary();

            AccessMap = new DeviceAccessMap(AccessMap.Keys1.ToReadOnlySet(), AccessMap.Keys2.ToReadOnlySet(),
                (user, device) => requireUpdate(device, AccessMap[user, device]) ? DeviceAccessType.Blocked : AccessMap[user, device]);
        }
    }
}
