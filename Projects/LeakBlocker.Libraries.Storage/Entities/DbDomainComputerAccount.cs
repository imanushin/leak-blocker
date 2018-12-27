using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.Libraries.Storage.Entities
{
    partial class DbDomainComputerAccount
    {
        private DomainComputerAccount ForceGetDomainComputerAccount()
        {
            return new DomainComputerAccount(
                ShortName,
                Identifier.GetAccountSecurityIdentifier(),
                Parent.GetDomainAccount(),
                CanonicalName);
        }
    }
}
