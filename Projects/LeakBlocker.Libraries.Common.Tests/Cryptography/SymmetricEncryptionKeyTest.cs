using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Libraries.Common.Tests.Cryptography
{
    partial class SymmetricEncryptionKeyTest
    {
        private static IEnumerable<SymmetricEncryptionKey> GetInstances()
        {
            foreach (byte[] key in new[] {
                new byte[32], 
                Enumerable.Repeat((byte)1, 32).ToArray(),
                Enumerable.Repeat((byte)3, 32).ToArray() })
            {
                yield return new SymmetricEncryptionKey(key.ToReadOnlyList());
            }
        }

        [TestMethod]
        public void RandomGeneratorTest()
        {
            Assert.IsNotNull(SymmetricEncryptionKey.GenerateRandomKey());
        }

        protected override void CheckArgumentExceptionParameter(string expectedParameterName, string actualParameterName)
        {
        }
    }
}
