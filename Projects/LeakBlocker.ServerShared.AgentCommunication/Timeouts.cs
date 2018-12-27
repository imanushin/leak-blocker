using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.ServerShared.AgentCommunication
{
    /// <summary>
    /// Список таймаутов при работе агента и сервера
    /// </summary>
    public static class Timeouts
    {
        /// <summary>
        /// Время, через которое агент соединяется с сервером
        /// </summary>
        public static readonly TimeSpan DelayBetweenCommunications = TimeSpan.FromMinutes(15);
    }
}
