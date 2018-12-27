using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.Libraries.Storage.Entities
{
    partial class DbDomainAccount
    {
        private DomainAccount ForceGetDomainAccount()
        {
            return new DomainAccount(ShortName, Identifier.GetAccountSecurityIdentifier(), CanonicalName);
        }
    }
}
