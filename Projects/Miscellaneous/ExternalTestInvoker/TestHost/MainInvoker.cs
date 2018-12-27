using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Network;
using SharedTestLibrary;

namespace TestHost
{
    internal static class MainInvoker
    {
        private static readonly IBaseNetworkHost serviceHost = SharedObjects.CreateBaseNetworkHost(CommonConstants.TestTcpPort,TimeSpan.FromHours(1));
        private static readonly TestInvokerService testInvokerService = new TestInvokerService();

        public static void Start()
        {
            serviceHost.RegisterServer(testInvokerService);
        }

        public static void Stop()
        {
            Disposable.DisposeSafe(serviceHost);
        }

    }
}
