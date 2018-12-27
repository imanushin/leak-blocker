using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Settings;
using LeakBlocker.Libraries.SystemTools.Entities;

namespace LeakBlocker.Agent.Core
{
    internal interface IAgentDataStorage : ILocalDataCache
    {
        ProgramConfiguration Settings
        {
            get;
            set;
        }

        ReadOnlyDictionary<DeviceDescription, DeviceAccessType> DeviceStates
        {
            get;
        }

        void SaveDeviceState(DeviceDescription device, DeviceAccessType state);

        void Delete();
    }
}
