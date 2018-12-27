using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Settings;

namespace LeakBlocker.Agent.Core
{
    internal interface ILocalControlServerHandler
    {
        void SetConfiguration();

        void RequestUninstall();
    }

    internal interface ILocalControlServer : IDisposable
    {
    }

    internal interface ILocalControlClient : IDisposable
    {
        void SetConfiguration(string secretKey);

        void RequestUninstall(string secretKey);
    }
}
