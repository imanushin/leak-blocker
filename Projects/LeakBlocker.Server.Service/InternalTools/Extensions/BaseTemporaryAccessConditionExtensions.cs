using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Entities.Settings.TemporaryAccess;

namespace LeakBlocker.Server.Service.InternalTools.Extensions
{
    internal static class BaseTemporaryAccessConditionExtensions
    {
        public static IEnumerable<DomainAccount> GetDomains(this BaseTemporaryAccessCondition condition)
        {
            var computers = condition as ComputerTemporaryAccessCondition;
            var users = condition as UserTemporaryAccessCondition;
            var devices = condition as DeviceTemporaryAccessCondition;

            if (computers != null)
            {
                var result = new HashSet<DomainAccount>();

                var domainComputerAccount = computers.Computer as DomainComputerAccount;

                if (domainComputerAccount != null)
                    result.Add(domainComputerAccount.Parent);

                return result;
            }

            if (users != null)
            {
                var domainUserAccount = users.User as DomainUserAccount;

                if (domainUserAccount != null)
                    return new[] { domainUserAccount.Parent };

                return ReadOnlySet<DomainAccount>.Empty;
            }

            if (devices != null)
            {
                return ReadOnlySet<DomainAccount>.Empty;
            }

            throw new InvalidOperationException("The condition with type {0} is not supported".Combine(condition.GetType()));
        }
    }
}
