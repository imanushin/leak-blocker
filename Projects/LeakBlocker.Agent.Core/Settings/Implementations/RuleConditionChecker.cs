using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules.Conditions;

namespace LeakBlocker.Agent.Core.Settings.Implementations
{
    internal sealed class RuleConditionChecker : IRuleConditionChecker
    {
        private delegate bool Matcher(AgentComputerState state, DeviceDescription device, BaseUserAccount user, BaseRuleCondition condition);

        private static readonly ReadOnlyDictionary<Type, Matcher> matchers = new Dictionary<Type, Matcher>
        {
            { typeof (CompositeRuleCondition), IsMatchedCompositeRuleCondition },
            { typeof (ComputerListRuleCondition), IsMatchedComputerListRuleCondition },
            { typeof (UserListRuleCondition), IsMatchedUserListRuleCondition },
            { typeof (DeviceListRuleCondition), IsMatchedDeviceListRuleCondition },
            { typeof (DeviceTypeRuleCondition), IsMatchedDeviceTypeRuleCondition },
        }.ToReadOnlyDictionary();

        public bool IsMatched(AgentComputerState state, DeviceDescription device, BaseUserAccount user, BaseRuleCondition condition)
        {
            return IsConditionMatched(state, device, user, condition);
        }

        private static bool IsConditionMatched(AgentComputerState state, DeviceDescription device, BaseUserAccount user, BaseRuleCondition condition)
        {
            Type conditionType = condition.GetType();

            if (!matchers.ContainsKey(conditionType))
                Exceptions.Throw(ErrorMessage.NotFound, "Unable to check the condition of type {0}".Combine(condition));

            bool result = matchers[conditionType](state, device, user, condition);

            return condition.Not ^ result;
        }

        private static bool IsMatchedComputerListRuleCondition(AgentComputerState state, DeviceDescription device, BaseUserAccount user, BaseRuleCondition condition)
        {
            var currentCondition = (ComputerListRuleCondition)condition;

            if (currentCondition.Computers.Contains(state.TargetComputer))
                return true;

            if (currentCondition.Groups.Select(group=>group.Identifier).Intersect(state.ComputerGroups).Any())
                return true;

            var domainComputer = state.TargetComputer as DomainComputerAccount;

            if (domainComputer == null)
                return false;

            if (currentCondition.Domains.Contains(domainComputer.Parent))
                return true;

            return currentCondition.OrganizationalUnits.Any(organizationalUnit => 
                OrganizationalUnitHelper.IsInOrganizationalUnit(organizationalUnit, state.TargetComputer));
        }

        private static bool IsMatchedUserListRuleCondition(AgentComputerState state, DeviceDescription device, BaseUserAccount user, BaseRuleCondition condition)
        {
            var currentCondition = (UserListRuleCondition)condition;

            if (currentCondition.Users.Contains(user))
                return true;

            if (currentCondition.Groups.Select(group=>group.Identifier).Intersect(state.LoggedOnUsers[user]).Any())
                return true;

            var domainUser = user as DomainUserAccount;

            if (domainUser == null)
                return false;

            if (currentCondition.Domains.Contains(domainUser.Parent))
                return true;

            return currentCondition.OrganizationalUnits.Any(organizationalUnit => 
                OrganizationalUnitHelper.IsInOrganizationalUnit(organizationalUnit, user));
        }

        private static bool IsMatchedDeviceListRuleCondition(AgentComputerState state, DeviceDescription device, BaseUserAccount user, BaseRuleCondition condition)
        {
            var currentCondition = (DeviceListRuleCondition)condition;

            return currentCondition.Devices.Contains(device);
        }

        private static bool IsMatchedDeviceTypeRuleCondition(AgentComputerState state, DeviceDescription device, BaseUserAccount user, BaseRuleCondition condition)
        {
            var currentCondition = (DeviceTypeRuleCondition)condition;

            return device.Category == currentCondition.DeviceType;
        }

        private static bool IsMatchedCompositeRuleCondition(AgentComputerState state, DeviceDescription device, BaseUserAccount user, BaseRuleCondition condition)
        {
            var currentCondition = (CompositeRuleCondition)condition;

            switch (currentCondition.OperationType)
            {
                case CompositeRuleConditionType.LogicalOr:
                    return currentCondition.Conditions.Any(otherCondition => IsConditionMatched(state, device, user, otherCondition));
                case CompositeRuleConditionType.LogicalAnd:
                    return currentCondition.Conditions.All(otherCondition => IsConditionMatched(state, device, user, otherCondition));
                default:
                    Exceptions.Throw(ErrorMessage.NotFound, "Operation type {0} was not found".Combine(currentCondition.OperationType));
                    return false;
            }
        }
    }
}
