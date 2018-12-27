using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Entities.Settings;
using LeakBlocker.Libraries.SystemTools.Entities;

namespace LeakBlocker.Agent.Core.Implementations.AgentDataStorageObjects
{
    [DataContract(IsReference = true)]
    internal sealed class DataDiskCache : BaseReadOnlyObject
    {
        [DataMember]
        public ProgramConfiguration Settings
        {
            get;
            private set;
        }

        [DataMember]
        public ReadOnlySet<DeviceState> DeviceStates
        {
            get;
            private set;
        }

        [DataMember]
        public ReadOnlySet<CachedUserData> Users
        {
            get;
            private set;
        }

        [DataMember]
        public CachedComputerData Computer
        {
            get;
            private set;
        }

        [DataMember]
        public BaseUserAccount ConsoleUser
        {
            get;
            private set;
        }

        public DataDiskCache(IReadOnlyCollection<CachedUserData> users, IReadOnlyCollection<DeviceState> deviceStates,
            ProgramConfiguration settings = null, CachedComputerData computer = null, BaseUserAccount consoleUser = null)
        {
            Check.CollectionHasNoDefaultItems(deviceStates, "deviceStates");
            Check.CollectionHasNoDefaultItems(users, "users");

            Settings = settings;
            DeviceStates = deviceStates.ToReadOnlySet();
            Users = users.ToReadOnlySet();
            Computer = computer;
            ConsoleUser = consoleUser;
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return Settings;
            yield return DeviceStates;
            yield return Users;
            yield return Computer;
            yield return ConsoleUser;
        }
    }
}
