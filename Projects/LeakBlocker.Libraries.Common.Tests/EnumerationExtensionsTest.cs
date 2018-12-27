using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Audit;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Libraries.Common.Tests
{
    [TestClass]
    public sealed class EnumerationExtensionsTest
    {
        [TestMethod]
        public void GetEnumerationDescriptionTest0()
        {
            CheckEnumerationDescriptions<AuditItemType>();
        }

        private static void CheckEnumerationDescriptions<T>() where T : struct, IComparable, IFormattable, IConvertible
        {
            Type type = typeof(T);

            IEnumerable<EnumerationDescriptionProviderAttribute> attributes = type.GetCustomAttributes(false).OfType<EnumerationDescriptionProviderAttribute>();

            if (attributes.Count() > 1)
                Assert.Fail("Only one description provider can be defined.");
            if (!attributes.Any())
                Assert.Fail("Description provider was not specified.");

            EnumerationDescriptionProviderAttribute descriptionProvider = attributes.FirstOrDefault();

            Assert.IsNotNull(descriptionProvider.Provider, "Resource manager is null.");
            Assert.IsNotNull(descriptionProvider.Resource, "Resource type is null.");

            int enumerationValuesCount = Enum.GetValues(typeof(T)).Length;

            foreach (T currentItem in Enum.GetValues(typeof(T)))
            {
                MemberInfo memberInfo = typeof(T).GetMember(currentItem.ToString()).FirstOrDefault();
                ForbiddenToUseAttribute attribute = memberInfo.GetCustomAttributes(false).OfType<ForbiddenToUseAttribute>().FirstOrDefault();

                if (attribute != null)
                {
                    enumerationValuesCount--;
                    continue;
                }

                EnumerationExtensions.GetValueDescription(currentItem as Enum);

                Assert.IsFalse(string.IsNullOrWhiteSpace((currentItem as Enum).GetValueDescription()), "Description is empty.");
            }

            int resourceStringsCount = 0;
            foreach (object currentItem in descriptionProvider.Provider.GetResourceSet(CultureInfo.CurrentUICulture, true, true))
                resourceStringsCount++;

            Assert.AreEqual(enumerationValuesCount, resourceStringsCount, "Description count and valid values count are not equal.");
        }
    }
}
