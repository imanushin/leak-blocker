using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Entities.Settings;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules.Conditions;
using LeakBlocker.Libraries.Common.Entities.Settings.TemporaryAccess;
using LeakBlocker.Libraries.SystemTools;
using LeakBlocker.Libraries.SystemTools.Entities;

namespace LeakBlocker.Agent.Core.Implementations.AgentDataStorageObjects
{
    internal sealed class DataCache
    {
        private readonly Dictionary<DeviceDescription, DeviceAccessType> deviceStates = new Dictionary<DeviceDescription, DeviceAccessType>();
        private readonly HashSet<CachedUserData> users = new HashSet<CachedUserData>();
        private CachedComputerData computer;
        private ProgramConfiguration settings;

        [CanBeNull]
        public ProgramConfiguration Settings
        {
            get
            {
                return settings;
            }
            set
            {
                Check.ObjectIsNotNull(value);
                settings = value;
            }
        }

        public Dictionary<DeviceDescription, DeviceAccessType> DeviceStates
        {
            get
            {
                return deviceStates;
            }
        }

        public HashSet<CachedUserData> Users
        {
            get
            {
                return users;
            }
        }

        public CachedComputerData Computer
        {
            get
            {
                return computer;
            }
            set
            {
                Check.ObjectIsNotNull(value);
                computer = value;
            }
        }

        [CanBeNull]
        public BaseUserAccount ConsoleUser
        {
            get;
            set;
        }

        public DataCache()
        {
        }

        public DataCache(DataDiskCache diskDataCache)
        {
            Check.ObjectIsNotNull(diskDataCache, "diskDataCache");

            if (diskDataCache.Computer != null)
                Computer = diskDataCache.Computer;

            if (diskDataCache.Settings != null)
                Settings = diskDataCache.Settings;

            if (diskDataCache.Users != null)
                Users.AddRange(diskDataCache.Users);

            if (diskDataCache.DeviceStates != null)
                DeviceStates.AddRange(diskDataCache.DeviceStates.Select(state => new KeyValuePair<DeviceDescription, DeviceAccessType>(state.Device, state.State)));

            ConsoleUser = diskDataCache.ConsoleUser;
        }
    }
}
