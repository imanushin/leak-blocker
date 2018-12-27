using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.License;
using LeakBlocker.Server.Service.InternalTools.LocalStorages;

namespace LeakBlocker.Server.Service.InternalTools.SingletonImplementation.LocalStorages
{
    internal sealed class LicenseStorage : Disposable, ILicenseStorage
    {
        private const int minimumLicenseCount = 10;
        private const string subfolderName = "Licenses";
        private const string licenseExtension = "dbLicense";

        private static readonly string licenseFolder = Path.Combine(SharedObjects.Constants.UserDataFolder, subfolderName);

        private readonly object syncRoot = new object();

        private ReadOnlySet<LicenseInfo> licenses = ReadOnlySet<LicenseInfo>.Empty;
        private int licenseCount = minimumLicenseCount;

        private ReadOnlySet<LicenseInfo> suppressedLicenses = ReadOnlySet<LicenseInfo>.Empty;

        private readonly IScheduler updater;

        public LicenseStorage()
        {
            updater = SharedObjects.CreateScheduler(UpdateStatus, TimeSpan.FromHours(3));

            UpdateStatus();
        }

        private void UpdateStatus()
        {
            lock (syncRoot)
            {
                if (!Directory.Exists(licenseFolder))
                {
                    licenses = ReadOnlySet<LicenseInfo>.Empty;
                    suppressedLicenses = GetAllSuppressedLicenses(licenses);

                    return;
                }

                Dictionary<LicenseInfo, string> savedLicenses = Directory.EnumerateFiles(licenseFolder, "*." + licenseExtension).ToDictionary(ReadLicense);

                suppressedLicenses = GetAllSuppressedLicenses(savedLicenses.Keys);

                licenses = savedLicenses.Keys.Without(suppressedLicenses).Where(license => !LicenseIsExpired(license)).ToReadOnlySet();

                licenseCount = CalculateLicenseCount(licenses);

                savedLicenses.Keys.Without(licenses).ForEach(license => File.Delete(savedLicenses[license]));//Удаляем устаревшие лицензии
            }
        }

        private static int CalculateLicenseCount(ReadOnlySet<LicenseInfo> actualLicenses )
        {
            IEnumerable<LicenseInfo> actualTemporaryLicenses = actualLicenses.Where(lic => lic.ExpirationDate != Time.Unknown && lic.ExpirationDate > Time.Now);

            if (actualTemporaryLicenses.Any())
                return int.MaxValue;

            IEnumerable<LicenseInfo> countedLicenses = actualLicenses.Where(lic => lic.ExpirationDate == Time.Unknown);

            return 10 + countedLicenses.Sum(lic => lic.Count);
        }

        private static bool LicenseIsExpired(LicenseInfo licenseInfo)
        {
            return licenseInfo.ExpirationDate != Time.Unknown && licenseInfo.ExpirationDate < Time.Now;
        }

        private static ReadOnlySet<LicenseInfo> GetAllSuppressedLicenses(IEnumerable<LicenseInfo> licenses)
        {
            return licenses.SelectMany(license => license.UnionAllChildLicenses).ToReadOnlySet();
        }

        private static LicenseInfo ReadLicense(string fileName)
        {
            using (Stream stream = File.OpenRead(fileName))
            {
                return BaseObjectSerializer.DeserializeFromXml<LicenseInfo>(stream);
            }
        }

        public ReadOnlySet<LicenseInfo> GetAllActualLicenses()
        {
            return licenses;
        }

        public void AddLicense(LicenseInfo licenseInfo)
        {
            Check.ObjectIsNotNull(licenseInfo, "licenseInfo");

            lock (syncRoot)
            {
                if (licenses.Contains(licenseInfo))
                    throw new ArgumentException("License {0} had already been added".Combine(licenseInfo));

                if (suppressedLicenses.Contains(licenseInfo))
                    throw new ArgumentException("License {0} is suppressed by other license".Combine(licenseInfo));

                if (LicenseIsExpired(licenseInfo))
                    throw new ArgumentException("License {0} has already been expired");

                string fileName = "{0}.{1}".Combine(Guid.NewGuid(), licenseExtension);

                string filePath = Path.Combine(licenseFolder, fileName);

                if (!Directory.Exists(licenseFolder))
                    Directory.CreateDirectory(licenseFolder);

                byte[] serialized = licenseInfo.SerializeToXml();

                File.WriteAllBytes(filePath, serialized);

                UpdateStatus();
            }
        }

        public int LicenseCount
        {
            get
            {
                return licenseCount;
            }
        }

        protected override void DisposeManaged()
        {
            DisposeSafe(updater);
            base.DisposeManaged();
        }
    }
}
