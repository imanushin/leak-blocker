using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules.Conditions;

namespace LeakBlocker.Libraries.Storage.Entities
{
    partial class DbUserListRuleCondition
    {
        private UserListRuleCondition ForceGetUserListRuleCondition()
        {
            return new UserListRuleCondition(
                Not,
                Domains.Select(domain => domain.GetDomainAccount()).ToReadOnlySet(),
                OrganizationalUnits.Select(ou => ou.GetOrganizationalUnit()).ToReadOnlySet(),
                Groups.Select(group => group.GetBaseGroupAccount()).ToReadOnlySet(),
                Users.Select(user => user.GetBaseUserAccount()).ToReadOnlySet());
        }
    }
}
