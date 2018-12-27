using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.License;
using LeakBlocker.Libraries.Common.Network;
using LeakBlocker.Server.Service.Generated;
using LeakBlocker.Server.Service.InternalTools;

namespace LeakBlocker.Server.Service.Network.AdminView
{
    internal sealed class LicenseToolsServer : GeneratedLicenseTools
    {
        public LicenseToolsServer()
            : base((ISecuritySessionManager)InternalObjects.AdminViewSecuritySessionManager)
        {
        }

        protected override ReadOnlySet<LicenseInfo> GetAllActualLicenses()
        {
            return InternalObjects.LicenseStorage.GetAllActualLicenses();
        }

        protected override void AddLicense(LicenseInfo licenseInfo)
        {
            InternalObjects.LicenseStorage.AddLicense(licenseInfo);
        }
    }
}
