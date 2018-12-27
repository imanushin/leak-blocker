using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Agent.Core
{
    internal interface IAgentInstaller
    {
        int Start();
        bool WaitForInstaller(TimeSpan timeout);
        void UninstallSelf();
    }
}



