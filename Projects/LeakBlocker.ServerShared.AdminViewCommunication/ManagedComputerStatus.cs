using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Resources;

namespace LeakBlocker.ServerShared.AdminViewCommunication
{
    /// <summary>
    /// Статус
    /// </summary>
    [EnumerationDescriptionProvider(typeof(ManagedComputerStatusResources))]
    public enum ManagedComputerStatus
    {
        /// <summary>
        /// Компьютер выключен
        /// </summary>
        TurnedOff,

        /// <summary>
        /// Идет установка агента
        /// </summary>
        AgentInstalling,

        /// <summary>
        /// Установка агента провалилась
        /// </summary>
        AgentInstallationFailed,

        /// <summary>
        /// Агент работает нормально
        /// </summary>
        Working,

        /// <summary>
        /// Один из вариантов
        /// 1. Сервер только что включился и еще не успел соединиться с агентом.
        /// 2. Предыдущая установка провалилась
        /// </summary>
        Unknown
    }
}