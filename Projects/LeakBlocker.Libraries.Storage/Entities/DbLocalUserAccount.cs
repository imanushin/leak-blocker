using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.Libraries.Storage.Entities
{
    partial class DbLocalUserAccount
    {
        private LocalUserAccount ForceGetLocalUserAccount()
        {
            return new LocalUserAccount(
                ShortName,
                Identifier.GetAccountSecurityIdentifier(),
                (LocalComputerAccount)Parent.GetBaseDomainAccount());
        }
    }
}
