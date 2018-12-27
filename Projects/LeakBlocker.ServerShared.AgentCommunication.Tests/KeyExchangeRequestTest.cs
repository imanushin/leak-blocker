using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Cryptography;
using LeakBlocker.Libraries.Common.Tests.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.ServerShared.AgentCommunication.Tests
{
    partial class KeyExchangeRequestTest
    {
        private static IEnumerable<KeyExchangeRequest> GetInstances()
        {
            yield return new KeyExchangeRequest();
            yield return new KeyExchangeRequest();
        }

        [TestMethod]
        public void EncryptionTest()
        {
            SymmetricEncryptionKey source = SymmetricEncryptionKeyTest.First;

            KeyExchangeRequest request = new KeyExchangeRequest();
            KeyExchangeReply reply = new KeyExchangeReply(request, source);
            SymmetricEncryptionKey result = request.Decrypt(reply);

            Assert.AreEqual(result, source);
        }
    }
}
