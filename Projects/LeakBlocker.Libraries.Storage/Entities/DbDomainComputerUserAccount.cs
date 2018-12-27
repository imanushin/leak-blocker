using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.Libraries.Storage.Entities
{
    partial class DbDomainComputerUserAccount
    {
        private DomainComputerUserAccount ForceGetDomainComputerUserAccount()
        {
            return new DomainComputerUserAccount(
                ShortName,
                Identifier.GetAccountSecurityIdentifier(),
                (DomainComputerAccount)Parent.GetBaseDomainAccount());
        }
    }
}
