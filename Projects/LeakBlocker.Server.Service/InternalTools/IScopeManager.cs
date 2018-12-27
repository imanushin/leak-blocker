using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.Server.Service.InternalTools
{
    internal interface IScopeManager
    {
        event Action ScopeChanged;

        ReadOnlySet<BaseComputerAccount> CurrentScope();

        void ForceUpdateScope();
    }
}
