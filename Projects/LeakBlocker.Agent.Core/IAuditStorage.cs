using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.ServerShared.AgentCommunication;

namespace LeakBlocker.Agent.Core
{
    internal interface IAuditStorage : IStateNotifierHandler
    {
        void Read(Action<AuditItemPackage> processingCallback);
    }
}
