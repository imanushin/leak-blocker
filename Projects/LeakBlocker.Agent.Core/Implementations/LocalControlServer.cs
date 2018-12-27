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
    internal sealed class LocalControlServer : Disposable, ILocalControlServer
    {
        private static readonly string mailslotName = "LeakBlockerAgentLocalControl_" + SharedObjects.Constants.VersionString;
        private const int messageSize = 256;
        private const byte setConfigurationMessage = 1;
        private const byte requestUninstallMessage = 2;

        private readonly ILocalControlServerHandler eventHandler;
        private readonly IMailslotServer server;

        internal LocalControlServer(ILocalControlServerHandler eventHandler)
        {
            Check.ObjectIsNotNull(eventHandler, "eventHandler");
            
            this.eventHandler = eventHandler;
            server = SystemObjects.CreateMailslotServer(mailslotName, messageSize);
            server.MessageReceived += MessageReceived;
        }

        private void MessageReceived(byte[] data)
        {
            if(data.Length <= 3)
            {
                Log.Write("Incorrect data.");
                return;
            }

            string key = Encoding.Unicode.GetString(data, 2, data[1]);
            if (!AgentObjects.AgentPrivateStorage.SecretKey.Equals(key))
            {
                Log.Write("Incorrect key. Client is not authorized and operation is not allowed.");
                return;
            }

            SharedObjects.ExceptionSuppressor.Run(delegate
            {
                switch (data[0])
                {
                    case setConfigurationMessage:
                        eventHandler.SetConfiguration();
                        break;

                    case requestUninstallMessage:
                        eventHandler.RequestUninstall();
                        break;

                    default:
                        Log.Write("Unknown command.");
                        break;
                }
            });
        }

        protected override void DisposeManaged()
        {
            DisposeSafe(server);
        }
    }
}
