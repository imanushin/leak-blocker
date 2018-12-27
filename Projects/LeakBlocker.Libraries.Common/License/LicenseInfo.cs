using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;

namespace LeakBlocker.Libraries.Common.License
{
    /// <summary>
    /// Лицензия продукта. В конструкторе всегда идет проверка лицензионного кода.
    /// </summary>
    [DataContract(IsReference = true)]
    [Serializable]
    public sealed class LicenseInfo : BaseReadOnlyObject
    {
        private static readonly SHA1 hashCreator = new SHA1CryptoServiceProvider();
        
        private static readonly RSACryptoServiceProvider rsaProvider = CreateRsaProvider();

        private LicenseInfo(int count, IReadOnlyCollection<LicenseInfo> suppressedLicenses, string signature, string companyName, Time creationTime, Time expirationTime)
        {
            Check.StringIsMeaningful(signature, "signature");
            Check.ObjectIsNotNull(suppressedLicenses, "suppressedLicenses");
            Check.IntegerIsGreaterThanZero(count, "count");
            Check.StringIsMeaningful(companyName, "companyName");
            Check.ObjectIsNotNull(expirationTime, "expirationTime");
            Check.ObjectIsNotNull(creationTime, "creationTime");

            Signature = signature;
            SuppressedLicenses = suppressedLicenses.ToReadOnlySet();
            Count = count;
            CompanyName = companyName;
            ExpirationDate = expirationTime;
            CreationTime = creationTime;

            CheckLicense();

            if (suppressedLicenses.All(lic => lic.ExpirationDate != Time.Unknown) && suppressedLicenses.Sum(lic => lic.Count) > Count)
                throw new ArgumentException("Count of the current license is less then sum of count in suppressed licenses. Wrong license: {0}".Combine(this));
        }

        /// <summary>
        /// Постоянная лицензия на ограниченное число компьютеров
        /// </summary>
        public LicenseInfo(int count, IReadOnlyCollection<LicenseInfo> suppressedLicenses, string signature, string companyName, Time creationTime)
            : this(count, suppressedLicenses, signature, companyName, creationTime, Time.Unknown)
        {
        }

        /// <summary>
        /// Временная лицензия на неограниченное количество компьютеров
        /// </summary>
        public LicenseInfo(string signature, string companyName, Time creationTime, Time expirationTime)
            : this(int.MaxValue, ReadOnlySet<LicenseInfo>.Empty, signature, companyName, creationTime, expirationTime)
        {
        }

        /// <summary>
        /// Подпись. Нужна только для проверки лицензии и сохранения в базе данных
        /// </summary>
        [DataMember]
        public string Signature
        {
            get;
            private set;
        }

        /// <summary>
        /// Время, до которого работает лицензия. Если == Time.Unknown, то бессрочно
        /// </summary>
        [DataMember]
        public Time ExpirationDate
        {
            get;
            private set;
        }

        /// <summary>
        /// Количество компьютеров, содержащихся в лицензии
        /// </summary>
        [DataMember]
        public int Count
        {
            get;
            private set;
        }

        /// <summary>
        /// Имя компании, получившей лицензию
        /// </summary>
        [DataMember]
        public string CompanyName
        {
            get;
            private set;
        }

        /// <summary>
        /// Дата создания лицензии. Нужна во многом для генерации разных ключей
        /// </summary>
        [DataMember]
        public Time CreationTime
        {
            get;
            private set;
        }

        /// <summary>
        /// Все предыдущие лицензии, которые покрываются данной
        /// </summary>
        [DataMember]
        public ReadOnlySet<LicenseInfo> SuppressedLicenses
        {
            get;
            private set;
        }

        /// <summary>
        /// Объединяет все лицензии, пробегая по всему дереву. В список также НЕ включается текущая лицензия
        /// </summary>
        public IEnumerable<LicenseInfo> UnionAllChildLicenses
        {
            get
            {
                foreach (LicenseInfo license in SuppressedLicenses)
                {
                    yield return license;

                    foreach (LicenseInfo innerLicense in license.UnionAllChildLicenses)
                    {
                        yield return innerLicense;
                    }
                }
            }
        }

        /// <summary>
        /// Выдает строку, которая используется для генерации цифровой подписи
        /// </summary>
        public static byte[] ComputeHash(string companyName, int count, Time creationTime, Time expirationTime, IReadOnlyCollection<LicenseInfo> suppressedLicenses)
        {
            if (expirationTime != Time.Unknown && count != int.MaxValue)
                throw new ArgumentException("Wrong license type because Time={0} and count={1}".Combine(expirationTime, count));

            const string template = "({0};{1};{2};{3};{4})";

            List<LicenseInfo> inner = suppressedLicenses.ToReadOnlySet().ToList();

            inner.Sort((left, right) => String.CompareOrdinal(left.Signature, right.Signature));

            string children = string.Join(",", inner.Select(license => license.ComputeHash()));

            string resultString = template.Combine(count, companyName, expirationTime, creationTime, children);

            byte[] resultBytes = Encoding.Unicode.GetBytes(resultString);

            return hashCreator.ComputeHash(resultBytes);
        }

        private byte[] ComputeHash()
        {
            return ComputeHash(CompanyName, Count, CreationTime, ExpirationDate, SuppressedLicenses);
        }

        /// <summary>
        /// Проверяет лицензию. Этот метод имеет смысл вызывать после десериализации объекта: после получения из БД, из Web UI и пр.
        /// </summary>
        public void CheckLicense()
        {
            byte[] hashedData = ComputeHash();

            byte[] decodedSignature = Convert.FromBase64String(Signature);

            if (!rsaProvider.VerifyHash(hashedData, "SHA1", decodedSignature))
                throw new LicenseException(typeof(LicenseInfo));
        }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]//Статический инициализатор
        private static RSACryptoServiceProvider CreateRsaProvider()
        {
            using (var stream = Assembly.GetCallingAssembly().GetManifestResourceStream("LeakBlocker.Libraries.Common.License.LicensePublicKey.txt"))
            {
                Check.ObjectIsNotNull(stream);

                var length = (int)stream.Length;

                var data = new byte[length];

                stream.Read(data, 0, length);

                var provider = new RSACryptoServiceProvider();

                provider.ImportCspBlob(data);

                return provider;
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        protected override string GetString()
        {
            return "LicenseInfo: count: {0}, company: {1}, valid before: {2}, creation time: {3}, inner licenses: ({4})".Combine(Count, CompanyName, CreationTime, ExpirationDate, SuppressedLicenses);
        }

        /// <summary>
        /// Returns all object that should be involved in hash code calculation and equalirty checks.
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return Signature;
            yield return Count;
            yield return ExpirationDate;
            yield return CompanyName;
            yield return CreationTime;
            yield return SuppressedLicenses;
        }
    }
}
