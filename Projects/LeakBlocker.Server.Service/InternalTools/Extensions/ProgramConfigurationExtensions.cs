using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Entities.Settings;

namespace LeakBlocker.Server.Service.InternalTools.Extensions
{
    internal static class ProgramConfigurationExtensions
    {
        public static IEnumerable<DomainAccount> GetAllDomains(this ProgramConfiguration configuration)
        {
            return configuration.TemporaryAccess.SelectMany(access => access.GetDomains())
                         .Union(configuration.Rules.SelectMany(rule => rule.GetDomains())).Distinct();
        }
    }
}
