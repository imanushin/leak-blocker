using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LicenseCreator
{
    internal static class MainClass
    {
        private static void Main()
        {
            byte[] publicKey;
            byte[] privateKey;

            var parameters = new CspParameters(1);

            RSAParameters privateParameters;
            RSAParameters publicParameters;

            using (var provider = new RSACryptoServiceProvider(2048))
            {
                provider.PersistKeyInCsp = true;

                publicParameters = provider.ExportParameters(false);
                privateParameters = provider.ExportParameters(true);

                publicKey = provider.ExportCspBlob(false);
                privateKey = provider.ExportCspBlob(true);

                provider.ImportCspBlob(publicKey);
                provider.ImportCspBlob(privateKey);

                File.WriteAllBytes("LicensePublicKey.txt", publicKey);
                File.WriteAllBytes("LicensePrivateKey.txt", privateKey);
            }

            var testData = new byte[1];

            byte[] encrypted;

            var hash = new SHA384Managed();
            byte[] hashedData = hash.ComputeHash(testData);

            using (var provider = new RSACryptoServiceProvider())
            {
            //    provider.ImportParameters(privateParameters);
                provider.ImportCspBlob(privateKey);

                encrypted = provider.SignHash(hashedData, CryptoConfig.MapNameToOID("SHA384"));
            }

            using (var provider = new RSACryptoServiceProvider())
            {
                provider.ImportCspBlob(publicKey);
         //       provider.ImportParameters(publicParameters);

               if(! provider.VerifyHash(hashedData, CryptoConfig.MapNameToOID("SHA384"), encrypted))
                   Debugger.Break();
                
            }
        }
    }
}
