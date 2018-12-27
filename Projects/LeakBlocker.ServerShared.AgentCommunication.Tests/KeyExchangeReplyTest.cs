using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Tests.Cryptography;

namespace LeakBlocker.ServerShared.AgentCommunication.Tests
{
    partial class KeyExchangeReplyTest
    {
        private static IEnumerable<KeyExchangeReply> GetInstances()
        {
            yield return new KeyExchangeReply(KeyExchangeRequestTest.First, SymmetricEncryptionKeyTest.First);
            yield return new KeyExchangeReply(KeyExchangeRequestTest.Second, SymmetricEncryptionKeyTest.Second);
        }
    }
}
