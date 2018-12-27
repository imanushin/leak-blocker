using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using LeakBlocker.AdminView.Desktop.Generated;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.AdminView.Desktop.Network
{
    internal sealed class StatusToolsClient : StatusToolsClientGenerated
    {
        public StatusToolsClient() 
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
