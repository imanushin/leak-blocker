using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeakBlocker.ServerShared.AdminViewCommunication.Tests
{
    partial class AgentSetupPasswordTest
    {
        private static IEnumerable<AgentSetupPassword> GetInstances()
        {
            yield return new AgentSetupPassword("123");
            yield return new AgentSetupPassword("234");
            yield return new AgentSetupPassword("345");
            yield return new AgentSetupPassword("456");
            yield return new AgentSetupPassword("567");
            yield return new AgentSetupPassword("678");
            yield return new AgentSetupPassword("789");
            yield return new AgentSetupPassword("890");
            yield return new AgentSetupPassword("901");
            yield return new AgentSetupPassword("012");
        }
    }
}
