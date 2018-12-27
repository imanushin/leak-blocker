using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Settings;
using LeakBlocker.Libraries.Common.Tests.Entities.Settings;

namespace LeakBlocker.ServerShared.AgentCommunication.Tests
{
    partial class AgentConfigurationTest
    {
        private static IEnumerable<AgentConfiguration> GetInstances()
        {
            foreach (bool isLicensed in new[] { true, false })
            {
                foreach (bool isManaged in new[] { true, false })
                {
                    foreach (ProgramConfiguration programConfiguration in ProgramConfigurationTest.objects)
                    {
                        yield return new AgentConfiguration(programConfiguration, isLicensed, isManaged);
                    }
                }
            }
        }
    }
}
