using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Agent.Core.Implementations.AgentDataStorageObjects;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Entities.Settings;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;
using LeakBlocker.Libraries.Common.Tests.Entities.Settings;
using LeakBlocker.Libraries.SystemTools.Entities;
using LeakBlocker.Libraries.SystemTools.Tests.Entities;

namespace LeakBlocker.Agent.Core.Tests.Implementations.AgentDataStorageObjects
{
    partial class DataDiskCacheTest
    {
        static IEnumerable<DataDiskCache> GetInstances()
        {
            foreach (ProgramConfiguration settings in ProgramConfigurationTest.objects)
            {
                foreach (CachedComputerData computer in CachedComputerDataTest.objects)
                {
                    foreach (BaseUserAccount consoleUser in BaseUserAccountTest.objects)
                    {
                        yield return new DataDiskCache(CachedUserDataTest.objects.ToReadOnlySet(), DeviceStateTest.objects.ToReadOnlySet(), settings, computer, consoleUser);
                    }
                }
            }
        }
    }
}
