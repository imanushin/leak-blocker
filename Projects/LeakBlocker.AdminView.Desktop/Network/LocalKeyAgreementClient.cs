using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using LeakBlocker.AdminView.Desktop.Generated;
using LeakBlocker.Libraries.Common;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.AdminView.Desktop.Network
{
    internal sealed class LocalKeyAgreementClient : LocalKeyAgreementClientGenerated
    {
        public LocalKeyAgreementClient()
            : base(AdminViewCommunicationObjects.LocalKeysAgreementHelper.DefaultKey, new byte[1])
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
