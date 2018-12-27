using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Network;

namespace LeakBlocker.ServerShared.AdminViewCommunication
{
    /// <summary>
    /// Установщик агентов
    /// </summary>
    [NetworkObject]
    public interface IAgentInstallationTools : IDisposable
    {
        /// <summary>
        /// Принудительный запуск установки. Она запустится даже если с компьютером всё ок, компьютер вне Scope и т. д.
        /// </summary>
        void ForceInstallation(ReadOnlySet<BaseComputerAccount> computers);
    }
}
