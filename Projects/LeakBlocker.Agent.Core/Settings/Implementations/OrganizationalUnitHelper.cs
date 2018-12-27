using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.Agent.Core.Settings.Implementations
{
    internal static class OrganizationalUnitHelper
    {
        public static bool IsInOrganizationalUnit(OrganizationalUnit ou, BaseUserAccount account)
        {
            return IsAccountInOrganizationalUnit(ou, account);
        }

        public static bool IsInOrganizationalUnit(OrganizationalUnit ou, BaseComputerAccount account)
        {
            return IsAccountInOrganizationalUnit(ou, account);
        }

        private static bool IsAccountInOrganizationalUnit(OrganizationalUnit ou, Account account)
        {
            string canonicalName = account.CanonicalName + '/';
            canonicalName = canonicalName.Replace("//", "/");

            string ouPath = ou.CanonicalName + '/';
            ouPath = ouPath.Replace("//", "/");

            return canonicalName.StartsWith(ouPath, StringComparison.OrdinalIgnoreCase);
        }
    }
}
