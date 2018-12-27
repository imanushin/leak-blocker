using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.Libraries.Storage.Entities
{
    partial class DbLocalGroupAccount
    {
        private LocalGroupAccount ForceGetLocalGroupAccount()
        {
            return new LocalGroupAccount(
                ShortName,
                Identifier.GetAccountSecurityIdentifier(),
                (LocalComputerAccount)Parent.GetBaseDomainAccount());
        }
    }
}
