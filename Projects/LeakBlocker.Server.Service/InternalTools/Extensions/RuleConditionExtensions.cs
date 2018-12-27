using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules.Conditions;
using LeakBlocker.Libraries.SystemTools.Entities;

namespace LeakBlocker.Server.Service.InternalTools.Extensions
{
    internal static class RuleConditionExtensions
    {
        /// <summary>
        /// Выдает все компьютеры, которые каким-нибудь образом участвуют в Condition'е.
        /// </summary>
        public static IEnumerable<BaseComputerAccount> GetAllComputersUsedInCondition(this BaseRuleCondition condition)
        {
            Check.ObjectIsNotNull(condition, "condition");

            var composite = condition as CompositeRuleCondition;
            var computers = condition as ComputerListRuleCondition;

            if (computers != null)
            {
                var result = new HashSet<BaseComputerAccount>();

                result.AddRange(computers.Computers);

                IDomainSnapshot securityObjectContainer = InternalObjects.SecurityObjectCache.Data;

                Check.ObjectIsNotNull(securityObjectContainer);

                foreach (DomainAccount domain in computers.Domains)
                {
                    var domainComputers = securityObjectContainer.GetObjectsInDomain(domain).OfType<BaseComputerAccount>().ToReadOnlySet();

                    result.AddRange(domainComputers);
                }

                foreach (OrganizationalUnit ou in computers.OrganizationalUnits)
                {
                    if (!securityObjectContainer.OrganizationalUnits.Contains(ou))
                        InternalObjects.AuditItemHelper.DomainMemberIsNotAccessible(ou);

                    result.AddRange(securityObjectContainer.GetObjectsInOrganizationalUnit(ou).OfType<BaseComputerAccount>());
                }

                foreach (DomainGroupAccount group in computers.Groups)
                {
                    if (!securityObjectContainer.Groups.Contains(group))
                        InternalObjects.AuditItemHelper.DomainMemberIsNotAccessible(group);

                    result.AddRange(securityObjectContainer.GetObjectsInGroup(group).OfType<BaseComputerAccount>());
                }

                return result;
            }

            if (composite != null)
            {
                var result = new HashSet<BaseComputerAccount>();

                foreach (BaseRuleCondition child in composite.Conditions)
                {
                    result.AddRange(child.GetAllComputersUsedInCondition());
                }

                return result;
            }

            if (condition is DeviceListRuleCondition)
                return ReadOnlySet<BaseComputerAccount>.Empty;

            if (condition is UserListRuleCondition)
                return ReadOnlySet<BaseComputerAccount>.Empty;

            if (condition is DeviceTypeRuleCondition)
                return ReadOnlySet<BaseComputerAccount>.Empty;

            throw new InvalidOperationException("The condition with type {0} is not supported".Combine(condition.GetType()));
        }

        public static IEnumerable<DomainAccount> GetDomains(this BaseRuleCondition condition)
        {
            var composite = condition as CompositeRuleCondition;
            var computers = condition as ComputerListRuleCondition;
            var users = condition as UserListRuleCondition;

            if (composite != null)
            {
                return composite.Conditions.SelectMany(GetDomains).Distinct();
            }

            if (computers != null)
            {
                return computers.Computers.OfType<DomainComputerAccount>().Select(c => c.Parent)
                                .Union(computers.Domains)
                                .Union(computers.Groups.Select(group => group.Parent))
                                .Union(computers.OrganizationalUnits.Select(ou => ou.Parent)).Distinct();
            }

            if (users != null)
            {
                return users.Users.OfType<DomainUserAccount>().Select(user => user.Parent)
                        .Union(users.Domains)
                        .Union(users.Groups.OfType<DomainGroupAccount>().Select(group => group.Parent))
                        .Union(users.OrganizationalUnits.Select(ou => ou.Parent)).Distinct();

            }

            if (condition is DeviceListRuleCondition)
                return ReadOnlySet<DomainAccount>.Empty;

            if (condition is DeviceTypeRuleCondition)
                return ReadOnlySet<DomainAccount>.Empty;

            throw new InvalidOperationException("The condition with type {0} is not supported".Combine(condition.GetType()));
        }
    }
}
