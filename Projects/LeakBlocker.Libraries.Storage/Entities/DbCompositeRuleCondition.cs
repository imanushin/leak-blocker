using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules.Conditions;

namespace LeakBlocker.Libraries.Storage.Entities
{
    partial class DbCompositeRuleCondition
    {
        private CompositeRuleCondition ForceGetCompositeRuleCondition()
        {
            return new CompositeRuleCondition(
                Not,
                Conditions.Select(condition => condition.GetBaseRuleCondition()).ToReadOnlySet(),
                OperationType);
        }
    }
}
