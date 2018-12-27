using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.Libraries.Storage.Entities
{
    partial class DbDomainComputerGroupAccount
    {
        private DomainComputerGroupAccount ForceGetDomainComputerGroupAccount()
        {
            return new DomainComputerGroupAccount(
                ShortName,
                Identifier.GetAccountSecurityIdentifier(),
                (DomainComputerAccount)Parent.GetBaseDomainAccount());
        }
    }
}
