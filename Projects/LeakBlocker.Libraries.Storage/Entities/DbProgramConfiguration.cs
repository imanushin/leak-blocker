using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Settings;

namespace LeakBlocker.Libraries.Storage.Entities
{
    partial class DbProgramConfiguration
    {
        private ProgramConfiguration ForceGetProgramConfiguration()
        {
            return new ProgramConfiguration(
                ConfigurationVersion,
                Rules.Select(rule => rule.GetRule()).ToReadOnlySet(),
                TemporaryAccess.Select(condition => condition.GetBaseTemporaryAccessCondition()).ToReadOnlySet());
        }
    }
}
