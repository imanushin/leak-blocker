using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Principal;
using System.Text;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Equality;

namespace LeakBlocker.Libraries.Security.Accounts
{
    public static class SecurityIdentifierExtensions
    {
        public static AccountSecurityIdentifier ToAccountSecurityIdentifier(this SecurityIdentifier identifier)
        {
            Check.ObjectIsNotNull(identifier, "identifier");

            return new AccountSecurityIdentifier(identifier);
        }
    }
}
