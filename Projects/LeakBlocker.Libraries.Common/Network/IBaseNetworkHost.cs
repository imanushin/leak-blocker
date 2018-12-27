using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common.Network
{
    /// <summary>
    /// Хост для работы с Network Service'ом
    /// </summary>
    public interface IBaseNetworkHost : IDisposable
    {
        /// <summary>
        /// Регистрирует сервер в слушателе
        /// </summary>
        void RegisterServer(BaseServer server);
    }
}