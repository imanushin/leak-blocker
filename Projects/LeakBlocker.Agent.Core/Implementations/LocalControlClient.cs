using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Settings;
using LeakBlocker.Libraries.SystemTools;
using LeakBlocker.Libraries.SystemTools.Network;

namespace LeakBlocker.Agent.Core.Implementations
{
    internal sealed class LocalControlClient : Disposable, ILocalControlClient
    {
        private static readonly string mailslotName = "LeakBlockerAgentLocalControl_" + SharedObjects.Constants.VersionString;
        private const int messageSize = 256;
        private const byte setConfigurationMessage = 1;
        private const byte requestUninstallMessage = 2;

        private readonly IMailslotClient client;
        
        internal LocalControlClient()
        {
            client = SystemObjects.CreateMailslotClient(mailslotName, messageSize);
        }

        protected override void DisposeManaged()
        {
            DisposeSafe(client);
        }

        void ILocalControlClient.SetConfiguration(string secretKey)
        {
            using (new TimeMeasurement("Configuration update client method"))
            {
                Check.StringIsMeaningful(secretKey, "secretKey");
                SendData(secretKey, setConfigurationMessage);
            }
        }

        void ILocalControlClient.RequestUninstall(string secretKey)
        {
            using (new TimeMeasurement("Uninstall request client method"))
            {
                Check.StringIsMeaningful(secretKey, "secretKey");
                SendData(secretKey, requestUninstallMessage);
            }
        }

        private void SendData(string secretKey, byte command)
        {
            byte[] data = new byte[messageSize];
            byte[] stringData = Encoding.Unicode.GetBytes(secretKey);
            stringData.CopyTo(data, 2);
            data[0] = command;
            data[1] = (byte)stringData.Length;
            client.SendMessage(data);
        }
    }
}
