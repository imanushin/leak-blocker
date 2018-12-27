using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.License;

namespace LeakBlocker.InternalLicenseManager
{
    public static class LicenseCreator
    {
        private static readonly RSACryptoServiceProvider rsaProvider = CreateRsaProvider();

        public static LicenseInfo CreateLicense(string companyName, int count, IReadOnlyCollection<LicenseInfo> innerLicenses)
        {
            Time creationTime = Time.Now;

            var signature = GetSignature(companyName, count, creationTime, Time.Unknown, innerLicenses);

            return new LicenseInfo(count, innerLicenses, signature, companyName, creationTime);
        }

        public static LicenseInfo CreateLicense(string companyName, Time expirationTime)
        {
            Time creationTime = Time.Now;

            var signature = GetSignature(companyName, int.MaxValue, creationTime, expirationTime, ReadOnlySet<LicenseInfo>.Empty);

            return new LicenseInfo(signature, companyName, creationTime, expirationTime);
        }

        private static string GetSignature(string companyName, int count, Time creationTime, Time expirationTime, IReadOnlyCollection<LicenseInfo> innerLicenses)
        {
            byte[] hash = LicenseInfo.ComputeHash(companyName, count, creationTime, expirationTime, innerLicenses);

            byte[] encryptedHash = rsaProvider.SignHash(hash, "SHA1");

            return Convert.ToBase64String(encryptedHash);
        }

        private static RSACryptoServiceProvider CreateRsaProvider()
        {
            var provider = new RSACryptoServiceProvider();
            using (var stream = Assembly.GetCallingAssembly().GetManifestResourceStream("LeakBlocker.InternalLicenseManager.LicensePrivateKey.txt"))
            {
                Check.ObjectIsNotNull(stream);

                var length = (int)stream.Length;

                var data = new byte[length];

                stream.Read(data, 0, length);

                provider.ImportCspBlob(data);
            }

            return provider;
        }
    }
}
