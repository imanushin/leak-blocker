using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Settings;

namespace LeakBlocker.Agent.Core.Settings
{
    internal interface IProgramConfigurationChecker
    {
        RuleCheckResult GetRequiredActions(AgentComputerState agentState, ProgramConfiguration configuration);
    }
}
