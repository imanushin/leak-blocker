using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules.Conditions;

namespace LeakBlocker.Libraries.Storage.Entities
{
    partial class DbComputerListRuleCondition
    {
        private ComputerListRuleCondition ForceGetComputerListRuleCondition()
        {
            return new ComputerListRuleCondition(
                Not,
                Domains.Select(domain => domain.GetDomainAccount()).ToReadOnlySet(),
                OrganizationalUnits.Select(ou => ou.GetOrganizationalUnit()).ToReadOnlySet(),
                Groups.Select(group => group.GetDomainGroupAccount()).ToReadOnlySet(),
                Computers.Select(computer => computer.GetBaseComputerAccount()).ToReadOnlySet());
        }
    }
}
