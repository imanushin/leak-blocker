using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.License;

namespace LeakBlocker.Server.Service.InternalTools.LocalStorages
{
    internal interface ILicenseStorage : IDisposable
    {
        ReadOnlySet<LicenseInfo> GetAllActualLicenses();

        void AddLicense(LicenseInfo licenseInfo);

        int LicenseCount
        {
            get;
        }
    }
}
