using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Network;

namespace LeakBlocker.ServerShared.AdminViewCommunication.KeysAgreement
{
    /// <summary>
    /// Сетевой интерфейс 
    /// </summary>
    [NetworkObject]
    public interface ILocalKeyAgreement : IDisposable
    { 
        /// <summary>
        /// Регистрирует текущего пользователя.
        /// На вход передается непосредственно пользователь (в дальнейшем проверка будет идти через реестр) и время.
        /// Время не должно отличаться от текущего больше, чем на один час.
        /// </summary>
        int RegisterUser(string fullUserName, AccountSecurityIdentifier userSid, Time timeMark);
    }
}
