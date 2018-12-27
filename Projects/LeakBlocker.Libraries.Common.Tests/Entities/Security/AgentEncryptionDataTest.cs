using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.Libraries.Common.Tests.Entities.Security
{
    partial class AgentEncryptionDataTest
    {
        private static IEnumerable<AgentEncryptionData> GetInstances()
        {
            foreach (BaseComputerAccount computer in BaseComputerAccountTest.objects)
            {
                foreach (string key in new[] {
                    new byte[32], 
                    Enumerable.Repeat((byte)1, 32).ToArray() }.Select(Convert.ToBase64String))
                {
                    yield return new AgentEncryptionData(computer, key);
                }
            }
        }

        protected override bool SkipSerializationTest()
        {
            return true;
        }
    }
}
