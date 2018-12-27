using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Entities.Settings;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules;
using LeakBlocker.Libraries.Common.Entities.Settings.TemporaryAccess;

namespace LeakBlocker.Agent.Core.Settings.Implementations
{
    internal sealed class ProgramConfigurationChecker : IProgramConfigurationChecker
    {
        private readonly IRuleConditionChecker ruleConditionChecker = AgentObjects.RuleConditionChecker;
        private readonly ITemporaryAccessConditionsChecker temporaryAccessConditionsChecker = AgentObjects.TemporaryAccessConditionsChecker;

        public RuleCheckResult GetRequiredActions(AgentComputerState agentState, ProgramConfiguration configuration)
        {
            return new RuleCheckResult(
                agentState.LoggedOnUsers.Keys.ToReadOnlySet(),
                agentState.ConnectedDevices,
                (user, device) => GetAccessAcions(user, device, agentState, configuration)
                );
        }

        private CommonActionData GetAccessAcions(BaseUserAccount user, DeviceDescription device, AgentComputerState agentState, ProgramConfiguration configuration)
        {
            var directActions = ActionData.SkipAll;

            foreach (Rule sortedRule in configuration.SortedRules)
            {
                if (!ruleConditionChecker.IsMatched(agentState, device, user, sortedRule.RootCondition))
                    continue;

                directActions = directActions.UpdateActions(sortedRule.Actions);
            }

            bool hasReadOnlyAccess = false;

            foreach (BaseTemporaryAccessCondition temporaryAccessCondition in configuration.TemporaryAccess)
            {
                if (temporaryAccessCondition.EndTime < Time.Now)
                    continue;

                if (!temporaryAccessConditionsChecker.IsMatched(device, agentState.TargetComputer, user, temporaryAccessCondition))
                    continue;

                if (temporaryAccessCondition.ReadOnlyAccess)
                {
                    hasReadOnlyAccess = true;
                    continue;
                }

                return new CommonActionData(DeviceAccessType.TemporarilyAllowed, directActions.AuditAction);//Ибо куда уж более демократично
            }

            return hasReadOnlyAccess ? new CommonActionData(DeviceAccessType.TemporarilyAllowedReadOnly, directActions.AuditAction) : new CommonActionData(directActions);
        }
    }
}
