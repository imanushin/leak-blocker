using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Agent.Core.Tests.External
{
    class AgentPrivateStorageImplementation : IAgentPrivateStorage
    {
        public bool StandaloneMode
        {
            get;
            set;
        }

        public string ServerAddress
        {
            get;
            set;
        }

        public string SecretKey
        {
            get;
            set;
        }

        public string PasswordHash
        {
            get;
            set;
        }

        public bool FirstRun
        {
            get;
            set;
        }

        public bool Licensed
        {
            get;
            set;
        }

        public bool Running
        {
            get;
            set;
        }

        public int ServerPort
        {
            get;
            set;
        }

        public AgentPrivateStorageImplementation()
        {
            StandaloneMode = false;
            ServerAddress = string.Empty;
            SecretKey = string.Empty;
            PasswordHash = string.Empty;
            FirstRun = false;
            Licensed = true;
            Running = false;
        }
    }
}
