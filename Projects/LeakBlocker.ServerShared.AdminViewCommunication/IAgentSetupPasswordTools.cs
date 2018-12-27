using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using LeakBlocker.Libraries.Common.Network;

namespace LeakBlocker.ServerShared.AdminViewCommunication
{
    /// <summary>
    /// Позволяет администратору сохранять пароль для удаления агентов
    /// </summary>
    [NetworkObject]
    public interface IAgentSetupPasswordTools : IDisposable
    {
        /// <summary>
        /// Выдает текущий пароль для разблокировки агентов
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        AgentSetupPassword GetPassword();

        /// <summary>
        /// Отсылает пароль для разблокировки агентов на заданный адрес
        /// </summary>
        void SendPassword(EmailSettings emailSettings);
    }
}
