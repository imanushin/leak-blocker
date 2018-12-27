using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Cryptography;

namespace LeakBlocker.Libraries.Common.Network
{
    /// <summary>
    /// Занимается работой с сессиями безопасности для серверной реализации протокола
    /// </summary>
    public interface ISecuritySessionManager
    {
        /// <summary>
        /// Читает поток и создает сессию
        /// </summary>
        SymmetricEncryptionProvider InitSession(byte[] token);

        /// <summary>
        /// Закрывает сессию
        /// </summary>
        void CloseSession();
    }
}
