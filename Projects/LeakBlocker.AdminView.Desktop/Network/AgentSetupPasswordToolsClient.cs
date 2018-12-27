using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using LeakBlocker.AdminView.Desktop.Generated;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Cryptography;

namespace LeakBlocker.AdminView.Desktop.Network
{
    internal sealed class AgentSetupPasswordToolsClient : AgentSetupPasswordToolsClientGenerated
    {
        public AgentSetupPasswordToolsClient()
            : base(UiObjects.AdminKeyStorage.EncryptionKey, UiObjects.AdminKeyStorage.CreateUserToken())
        {
        }

        protected override string HostName
        {
            get
            {
                return Environment.MachineName;
            }
        }

        protected override int Port
        {
            get
            {
                return SharedObjects.Constants.DefaultTcpPort;
            }
        }
    }
}
