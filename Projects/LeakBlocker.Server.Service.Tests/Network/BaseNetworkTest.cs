using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Implementations;
using LeakBlocker.Libraries.Common.Network;
using LeakBlocker.Libraries.Common.SystemTools;
using LeakBlocker.Libraries.Common.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Server.Service.Tests.Network
{
    [TestClass]
    public abstract class BaseNetworkTest : BaseTest
    {
        private static readonly AsyncInvoker asyncInvoker = new AsyncInvoker();

        protected static void Initialize()
        {
            SharedObjects.Singletons.AsyncInvoker.SetTestImplementation(asyncInvoker);
            SharedObjects.Factories.ThreadPool.EnqueueConstructor(val => new NativeThreadPool(val));
        }

        protected static IDisposable InitServer<TBaseServer>()
            where TBaseServer : BaseServer, new()
        {
            var result = new BaseNetworkHost(SharedObjects.Constants.DefaultTcpPort, SharedObjects.Constants.DefaultTcpTimeout);

            result.RegisterServer(new TBaseServer());

            return result;
        }
    }
}
