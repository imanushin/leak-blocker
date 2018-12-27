using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules.Conditions;

namespace LeakBlocker.Agent.Core.Settings
{
    internal interface IRuleConditionChecker
    {
        bool IsMatched(AgentComputerState state, DeviceDescription device, BaseUserAccount user, BaseRuleCondition condition);
    }
}
