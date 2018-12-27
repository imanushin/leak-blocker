using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.License;
using LeakBlocker.Libraries.Common.Tests.Entities;

namespace LeakBlocker.Libraries.Common.Tests.License
{
    partial class LicenseRequestDataTest
    {
        private static IEnumerable<LicenseRequestData> GetInstances()
        {
            foreach (UserContactInformation userContact in UserContactInformationTest.objects)
            {
                foreach (int count in new[] { 12, 21 })
                {
                    yield return new LicenseRequestData(userContact, count);
                }
            }
        }
    }
}
