using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common.License
{
    internal sealed class LicenseLinkManager : ILicenseLinkManager
    {
        private const string linkTemplate = "http://leakblocker.com/license.php?Firstname={0}&Lastname={1}&Email={2}&Company={3}&Phone={4}&Quantity={5}";

        public string GetLink(LicenseRequestData licenseRequest)
        {
            return linkTemplate.Combine(
                licenseRequest.UserContact.FirstName,
                licenseRequest.UserContact.LastName,
                licenseRequest.UserContact.Email,
                licenseRequest.UserContact.CompanyName,
                licenseRequest.UserContact.Phone,
                licenseRequest.Count);
        }
    }
}
