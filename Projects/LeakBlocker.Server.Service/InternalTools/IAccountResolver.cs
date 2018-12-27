using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.Server.Service.InternalTools
{
    internal interface IAccountResolver
    {
        BaseUserAccount ResolveUser(AccountSecurityIdentifier userIdentifier);
    }
}
