﻿using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules;

namespace LeakBlocker.Libraries.Storage.Entities
{
    partial class DbActionData
    {
        private ActionData ForceGetActionData()
        {
            return new ActionData(BlockAction, AuditAction);
        }
    }
}
