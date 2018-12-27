using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Agent.Core.Tests.External
{
    class AgentServiceControllerImplementation : BaseTestImplementation, IAgentServiceController
    {
        public void Uninstall()
        {
            base.RegisterMethodCall("Uninstall");
        }

        public void QueryMainTask()
        {
            //base.RegisterMethodCall("QueryMainTask");
        }


        public void QueryDelayedNetworkTask()
        {
            //throw new NotImplementedException();
        }
    }
}
