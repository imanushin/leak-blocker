using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.Libraries.Storage.Entities
{
    partial class DbDomainUserAccount
    {
        private DomainUserAccount ForceGetDomainUserAccount()
        {
            return new DomainUserAccount(
                ShortName,
                Identifier.GetAccountSecurityIdentifier(),
                (DomainAccount)Parent.GetBaseDomainAccount(),
                CanonicalName);
        }
    }
}
