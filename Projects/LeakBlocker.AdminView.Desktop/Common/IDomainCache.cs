using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.AdminView.Desktop.Common
{
    internal interface IDomainCache
    {
        void AddDomain(BaseDomainAccount domain);

        ReadOnlySet<BaseDomainAccount> AllDomains();
    }
}
