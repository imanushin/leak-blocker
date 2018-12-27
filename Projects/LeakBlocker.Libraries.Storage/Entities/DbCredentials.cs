using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.Libraries.Storage.Entities
{
    partial class DbCredentials
    {
        private Credentials ForceGetCredentials()
        {
            return new Credentials(Domain.GetBaseDomainAccount(), User, Password);
        }
    }
}
