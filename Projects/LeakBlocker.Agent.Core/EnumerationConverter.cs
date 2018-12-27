using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.SystemTools.Drivers;

namespace LeakBlocker.Agent.Core
{
    internal static class EnumerationConverter
    {
        internal static AgentFileActionType GetAgentFileActionType(FileActionType fileActionType, DeviceAccessType accessType)
        {
            bool temporaryAccess = ((accessType == DeviceAccessType.TemporarilyAllowed) || (accessType == DeviceAccessType.TemporarilyAllowedReadOnly));

            switch (fileActionType)
            {
                case FileActionType.ReadAllowed:
                    return temporaryAccess ? AgentFileActionType.ReadTemporarilyAllowed : AgentFileActionType.ReadAllowed;
                case FileActionType.ReadBlocked:
                    return AgentFileActionType.ReadBlocked;
                case FileActionType.ReadWriteAllowed:
                    return temporaryAccess ? AgentFileActionType.ReadWriteTemporarilyAllowed : AgentFileActionType.ReadWriteAllowed;
                case FileActionType.ReadWriteBlocked:
                    return AgentFileActionType.ReadWriteBlocked;
                case FileActionType.WriteAllowed:
                    return temporaryAccess ? AgentFileActionType.WriteTemporarilyAllowed : AgentFileActionType.WriteAllowed;
                case FileActionType.WriteBlocked:
                    return AgentFileActionType.WriteBlocked;
                case FileActionType.Unknown:
                    return AgentFileActionType.Unknown;
                default:
                    Log.Write("Unknown value: " + fileActionType + ".");
                    return AgentFileActionType.Unknown;
            }
        }

        internal static FileAccessType GetFileAccessType(DeviceAccessType accessType)
        {
            switch (accessType)
            {
                case DeviceAccessType.TemporarilyAllowed:
                case DeviceAccessType.Allowed:
                case DeviceAccessType.AllowedNotLicensed:
                    return FileAccessType.Allow;
                case DeviceAccessType.TemporarilyAllowedReadOnly:
                case DeviceAccessType.ReadOnly:
                    return FileAccessType.ReadOnly;
                case DeviceAccessType.Blocked:
                    return FileAccessType.Block;
                default:
                    Log.Write("Unknown value: " + accessType + ".");
                    return FileAccessType.Allow;
            }
        }
    }
}
