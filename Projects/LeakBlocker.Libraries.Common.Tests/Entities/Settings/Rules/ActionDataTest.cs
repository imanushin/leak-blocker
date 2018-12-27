using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules;

namespace LeakBlocker.Libraries.Common.Tests.Entities.Settings.Rules
{
    partial class ActionDataTest
    {
        private static IEnumerable<ActionData> GetInstances()
        {
            foreach (AuditActionType audit in EnumHelper<AuditActionType>.Values)
            {
                foreach (BlockActionType block in EnumHelper<BlockActionType>.Values)
                {
                    yield return new ActionData(block, audit);
                }
            }
        }
    }
}
