using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.Libraries.Storage.Entities
{
    partial class DbDomainGroupAccount
    {
        private DomainGroupAccount ForceGetDomainGroupAccount()
        {
            return new DomainGroupAccount(
                ShortName,
                Identifier.GetAccountSecurityIdentifier(),
                (DomainAccount)Parent.GetBaseDomainAccount(),
                CanonicalName);
        }
    }
}
