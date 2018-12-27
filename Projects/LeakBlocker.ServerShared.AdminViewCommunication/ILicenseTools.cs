using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.License;
using LeakBlocker.Libraries.Common.Network;

namespace LeakBlocker.ServerShared.AdminViewCommunication
{
    /// <summary>
    /// Сервис для работы с лицензиями
    /// </summary>
    [NetworkObject]
    public interface ILicenseTools : IDisposable
    {
        /// <summary>
        /// Выдает все корректные лицензии, действующие на данный момент
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        ReadOnlySet<LicenseInfo> GetAllActualLicenses();

        /// <summary>
        /// Добавляет лицензию в хранилище
        /// </summary>
        void AddLicense(LicenseInfo licenseInfo);
    }
}
