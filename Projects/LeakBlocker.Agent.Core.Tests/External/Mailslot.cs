using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.SystemTools.Network;

namespace LeakBlocker.Agent.Core.Tests.External
{
    class Mailslot : IMailslotClient, IMailslotServer
    {
        public event Action<byte[]> MessageReceived;

        public void SendMessage(byte[] data)
        {
            Check.ObjectIsNotNull(data);

            if (MessageReceived != null)
                MessageReceived(data);
        }

        public void Dispose()
        {
        }
    }
}
