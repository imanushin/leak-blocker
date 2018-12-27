using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.SystemTools;

namespace LeakBlocker.Agent.Core.Implementations
{
    internal sealed class AgentPrivateStorage : IAgentPrivateStorage
    {
        private static readonly string serverAddress = "004_" + SharedObjects.Constants.VersionString;
        private static readonly string serverPort = "005_" + SharedObjects.Constants.VersionString;
        private static readonly string secretKey = "011_" + SharedObjects.Constants.VersionString;

        private static readonly string passwordHash = "012_" + SharedObjects.Constants.VersionString;

        private static readonly string firstRun = "017_" + SharedObjects.Constants.VersionString;
        private static readonly string licensed = "018_" + SharedObjects.Constants.VersionString;

        private static readonly Guid storageIdentifier = new Guid("{8D581631-9714-4CF2-B44D-FF99AA094B5D}");

        private readonly IPrivateRegistryStorage storage = SystemObjects.CreatePrivateRegistryStorage(storageIdentifier);

        bool IAgentPrivateStorage.StandaloneMode
        {
            get
            {
                return storage.GetValue(serverAddress).Equals(AgentObjects.AgentConstants.StandaloneServerAddress);
            }
        }

        string IAgentPrivateStorage.ServerAddress
        {
            get
            {
                return storage.GetValue(serverAddress);
            }
            set
            {
                storage.SetValue(serverAddress, value);
            }
        }

        int IAgentPrivateStorage.ServerPort
        {
            get
            {
                try
                {
                    string value = storage.GetValue(serverPort);
                    return string.IsNullOrWhiteSpace(value) ? 0 : int.Parse(value, CultureInfo.InvariantCulture);
                }
                catch (Exception exception)
                {
                    Log.Write(exception);
                    return 0;
                }
            }
            set
            {
                storage.SetValue(serverPort, (value == 0) ? null : value.ToString(CultureInfo.InvariantCulture));
            }
        }

        string IAgentPrivateStorage.SecretKey
        {
            get
            {
                return storage.GetValue(secretKey);
            }
            set
            {
                storage.SetValue(secretKey, value);
            }
        }

        string IAgentPrivateStorage.PasswordHash
        {
            get
            {
                return storage.GetValue(passwordHash);
            }
            set
            {
                storage.SetValue(passwordHash, value);
            }
        }

        bool IAgentPrivateStorage.FirstRun
        {
            get
            {
                return !string.IsNullOrEmpty(storage.GetValue(firstRun));
            }
            set
            {
                storage.SetValue(firstRun, value ? "1" : null);
            }
        }

        bool IAgentPrivateStorage.Licensed
        {
            get
            {
                return !string.IsNullOrEmpty(storage.GetValue(licensed));
            }
            set
            {
                storage.SetValue(licensed, value ? "1" : null);
            }
        }
    }
}
