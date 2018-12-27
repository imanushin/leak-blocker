using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeakBlocker.Libraries.SystemTools.Network
{
    /// <summary>
    /// Mailslot client. Sends messages.
    /// </summary>
    public interface IMailslotClient : IDisposable
    {
        /// <summary>
        /// Sends a message to the server.
        /// </summary>
        /// <param name="data">Message data (server should be configured for receiving messages of the specified size).</param>
        void SendMessage(byte[] data);
    }
}
