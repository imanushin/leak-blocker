using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.AdminView.Desktop.Common
{
    internal sealed class DomainCache : IDomainCache
    {
        private readonly object syncRoot = new object();

        private readonly HashSet<BaseDomainAccount> domains = new HashSet<BaseDomainAccount>();

        public void AddDomain(BaseDomainAccount domain)
        {
            Check.ObjectIsNotNull(domain);

            lock (syncRoot)
            {
                domains.Add(domain);
            }
        }

        public ReadOnlySet<BaseDomainAccount> AllDomains()
        {
            lock (syncRoot)
            {
                return domains.ToReadOnlySet();
            }
        }
    }
}
