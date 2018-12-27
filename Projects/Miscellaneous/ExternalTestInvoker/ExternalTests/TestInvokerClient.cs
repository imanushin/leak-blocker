using LeakBlocker.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using ExternalTests.Generated;
using LeakBlocker.Libraries.Common.Cryptography;

namespace ExternalTests
{
    internal sealed class TestInvokerClient : TestInvokerServiceClientGenerated
    {
        private readonly string hostname;

        public TestInvokerClient(string hostname)
            : base(SymmetricEncryptionKey.Empty, new byte[]{0})
        {
            this.hostname = hostname;
        }

        protected override string HostName
        {
            get
            {
                return hostname;
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
