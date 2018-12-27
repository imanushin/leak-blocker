using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using LeakBlocker.Agent.Core.Generated;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Cryptography;
using LeakBlocker.Libraries.SystemTools;

namespace LeakBlocker.Agent.Core.Implementations
{
    internal sealed class AgentControlServiceClient : AgentControlServiceClientGenerated
    {
        private string serverAddress;
        private int port;

        public AgentControlServiceClient()
            : base(
            new SymmetricEncryptionKey(AgentObjects.AgentPrivateStorage.SecretKey),
            GetClientIdentifier())
        {
        }

        protected override string HostName
        {
            get
            {
                try
                {
                    string updatedAddress = AgentObjects.AgentPrivateStorage.ServerAddress;
                    int updatedPort = AgentObjects.AgentPrivateStorage.ServerPort;

                    serverAddress = updatedAddress;
                    port = updatedPort;
                }
                catch (Exception exception)
                {
                    Log.Write(exception.Message);
                }

                if (serverAddress == null)
                    Exceptions.Throw(ErrorMessage.InvalidOperation, "Server address is not initialized.");

                return serverAddress;
            }
        }

        protected override int Port
        {
            get
            {
                return port;
            }
        }

        private static byte[] GetClientIdentifier()
        {
            return SystemObjects.SystemAccountTools.LocalComputer.Identifier.ToBinaryForm();// AgentObjects.AgentDataStorage.Computer.Identifier.ToBinaryForm();
        }
    }
}
