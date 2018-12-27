using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Server.Service.InternalTools
{
    internal interface IAgentStatusObserver : IDisposable
    {
        void EnqueueObserving();
    }
}
