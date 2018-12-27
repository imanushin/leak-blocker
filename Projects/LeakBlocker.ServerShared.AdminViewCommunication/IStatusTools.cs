using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Network;

namespace LeakBlocker.ServerShared.AdminViewCommunication
{
    /// <summary>
    /// Контракт для работы с текущим состоянием
    /// </summary>
    [NetworkObject]
    public interface IStatusTools : IDisposable
    {
        /// <summary>
        /// Выдает весь Scope со статусами
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        ReadOnlySet<ManagedComputer> GetStatuses();
    }
}
