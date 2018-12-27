using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeakBlocker.Libraries.Common.Network
{
    /// <summary>
    /// Шифрует сообщения между сервером и клиентом.
    /// </summary>
    public interface IMessageEncoder
    {
        /// <summary>
        /// Шифрование исходящих данных
        /// </summary>
        byte[] Encode(byte[] data);

        /// <summary>
        /// Расшифровывание входящих данных
        /// </summary>
        byte[] Decode(byte[] data);
    }
}
