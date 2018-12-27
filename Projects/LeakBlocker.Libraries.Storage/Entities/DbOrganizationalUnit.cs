using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.Libraries.Storage.Entities
{
    partial class DbOrganizationalUnit
    {
        private OrganizationalUnit ForceGetOrganizationalUnit()
        {
            return new OrganizationalUnit(ShortName, CanonicalName, Parent.GetDomainAccount());
        }
    }
}
