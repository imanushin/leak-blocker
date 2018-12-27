using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common.License
{
    /// <summary>
    /// Обработка ссылок на покупку и обработку лицензий
    /// </summary>
    public interface ILicenseLinkManager
    {
        /// <summary>
        /// Выдает ссылку на покупку лицензии
        /// </summary>
        string GetLink(LicenseRequestData licenseRequest);
    }
}
