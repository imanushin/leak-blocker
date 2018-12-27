using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeakBlocker.Libraries.SystemTools.Network
{
    /// <summary>
    /// Mailslot server that receives messages.
    /// </summary>
    public interface IMailslotServer : IDisposable
    {        
        /// <summary>
        /// Event that is triggered when a new message was received.
        /// </summary>
        event Action<byte[]> MessageReceived;
    }
}
