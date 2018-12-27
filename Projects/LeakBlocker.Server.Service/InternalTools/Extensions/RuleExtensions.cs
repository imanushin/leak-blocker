using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules.Conditions;

namespace LeakBlocker.Server.Service.InternalTools.Extensions
{
    internal static class RuleExtensions
    {
        /// <summary>
        /// Если Rule исключает компьютеры из блокировки, то с помощью этого метода можно получить список исключенных компьютеров.
        /// Если не гарантируется, что компьютеры исключены из блокировки, то результатом будет пустой список
        /// </summary>
        public static IEnumerable<BaseComputerAccount> GetExcludedComputers(this Rule rule)
        {
            Check.ObjectIsNotNull(rule, "rule");

            var rootCondition = rule.RootCondition as ComputerListRuleCondition;

            if (rootCondition == null)
                return ReadOnlySet<BaseComputerAccount>.Empty;

            if (rule.Actions != new ActionData(BlockActionType.Unblock, AuditActionType.DisableAudit))
                return ReadOnlySet<BaseComputerAccount>.Empty;

            return rule.RootCondition.GetAllComputersUsedInCondition();
        }

        public static IEnumerable<DomainAccount> GetDomains(this Rule rule)
        {
            return rule.RootCondition.GetDomains();
        }
    }
}
