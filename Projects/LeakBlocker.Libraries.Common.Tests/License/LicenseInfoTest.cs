using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.InternalLicenseManager;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.License;

namespace LeakBlocker.Libraries.Common.Tests.License
{
    partial class LicenseInfoTest
    {
        private static IEnumerable<LicenseInfo> GetInstances()
        {
            var licenses = new List<LicenseInfo>();

            foreach (string companyName in new[] { "123", "321" })
            {
                foreach (int count in new[] { 10, 25 })
                {
                    LicenseInfo first = LicenseCreator.CreateLicense(companyName, count, ReadOnlyList<LicenseInfo>.Empty);
                    
                    licenses.Add(first);
                }

                foreach (Time time in TimeTest.objects.Take(2))
                {
                    LicenseInfo first = LicenseCreator.CreateLicense(companyName, time);
                    
                    licenses.Add(first);
                }
            }

            foreach (string companyName in new[] { "123", "321" })
            {
                foreach (int count in new[] { 1000,  2500 })
                {
                    LicenseInfo first = LicenseCreator.CreateLicense(companyName, count, licenses.ToReadOnlyList());

                    licenses.Add(first);
                }
            }

            return licenses;
        }

    }
}
