
using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Agent.Core
{
    internal interface IAgentPrivateStorage
    {
        bool StandaloneMode
        {
            get;
        }

        string ServerAddress
        {
            get;
            set;
        }

        int ServerPort
        {
            get;
            set;
        }

        string PasswordHash
        {
            get;
            set;
        }

        bool FirstRun
        {
            get;
            set;
        }

        bool Licensed
        {
            get;
            set;
        }

        string SecretKey
        {
            get;
            set;
        }
    }
}
