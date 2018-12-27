using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Entities.Settings;
using LeakBlocker.Libraries.Common.Tests.Entities.Settings;
using LeakBlocker.Libraries.SystemTools.Entities;
using LeakBlocker.Libraries.SystemTools.Tests.Entities;

namespace LeakBlocker.Agent.Core.Tests.External
{
    class AgentDataStorageImplementation : BaseTestImplementation, IAgentDataStorage
    {
        private ProgramConfiguration config;
        private readonly Dictionary<DeviceDescription, DeviceAccessType> deviceStates = new Dictionary<DeviceDescription, DeviceAccessType>();

        public ProgramConfiguration Settings
        {
            get
            {
                return config ?? ProgramConfigurationTest.First;
            }
            set
            {
                Check.ObjectIsNotNull(value);
                base.RegisterMethodCall("Settings", value);
                config = value;
            }
        }

        public ReadOnlySet<CachedUserData> Users
        {
            get { return CachedUserDataTest.objects.ToReadOnlySet(); }
        }

        public CachedComputerData Computer
        {
            get { return CachedComputerDataTest.First; }
        }

        public BaseUserAccount ConsoleUser
        {
            get;
            private set;
        }

        public ReadOnlyDictionary<DeviceDescription, DeviceAccessType> DeviceStates
        {
            get { return deviceStates.ToReadOnlyDictionary(); }
        }

        public AgentDataStorageImplementation(bool allowConsoleUser)
        {
            if (allowConsoleUser)
                ConsoleUser = CachedUserDataTest.First.User;
        }

        public void SaveDeviceState(DeviceDescription device, DeviceAccessType state)
        {
            base.RegisterMethodCall("SaveDeviceState", device, state);
        }

        public void Update()
        {
            base.RegisterMethodCall("Update");
        }


        public void Delete()
        {
            //base.RegisterMethodCall("Delete");
        }
    }
}
