using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Audit;
using LeakBlocker.Libraries.Common.Tests.Entities.Audit;

namespace LeakBlocker.ServerShared.AdminViewCommunication.Tests
{
    partial class ReportConfigurationTest
    {
        private static IEnumerable<ReportConfiguration> GetInstances()
        {
            foreach (bool areEnabled in new[] { true, false })
            {
                foreach (ReportFilter filter in ReportFilterTest.objects.Take(5))
                {
                    foreach (EmailSettings email in EmailSettingsTest.objects.Take(5))
                    {
                        yield return new ReportConfiguration(areEnabled, filter, email);
                    }
                }
            }
        }
    }
}
