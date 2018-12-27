using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Cryptography;
using LeakBlocker.Libraries.Common.Equality;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Libraries.Common.Tests.Cryptography
{
    [TestClass]
    public sealed class SymmetricEncryptionProviderTest : BaseTest
    {
        [TestMethod]
        public void EncryptDecryptTest()
        {
            Mocks.ReplayAll();

            for (int arraySize = 1; arraySize < 32 * 2 + 1; arraySize++)
            {
                using (var target = new SymmetricEncryptionProvider(SymmetricEncryptionKeyTest.First))
                {
                    byte[] originalValue = Enumerable.Range(0, arraySize).Select(value => (byte)value).ToArray(); //Используется "плохой" размер

                    byte[] encrypted = target.Encrypt(originalValue);
                    byte[] secondaryEncrypted = target.Encrypt(originalValue);

                    Assert.IsTrue(EnumerableComparer.Compare(encrypted, secondaryEncrypted));//Проверяем, что шифрование и расшифровывание независимы
                    
                    byte[] decrypted = target.Decrypt(encrypted);
                    byte[] secondaryDecrypted = target.Decrypt(encrypted);

                    Assert.IsTrue(EnumerableComparer.Compare(originalValue, decrypted));
                    Assert.IsTrue(EnumerableComparer.Compare(decrypted, secondaryDecrypted));//Проверяем, что шифрование и расшифровывание независимы
                }
            }
        }
    }
}
