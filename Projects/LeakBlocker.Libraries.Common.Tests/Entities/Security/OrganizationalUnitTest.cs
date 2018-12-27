using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Libraries.Common.Tests.Entities.Security
{
    partial class OrganizationalUnitTest
    {
        private static IEnumerable<OrganizationalUnit> GetInstances()
        {
            int unitNumber = 652;

            foreach (DomainAccount domain in DomainAccountTest.objects)
            {
                unitNumber++;
                string name = "test" + unitNumber;
                string distinguishedName = "CN=test" + unitNumber;

                yield return new OrganizationalUnit(name, distinguishedName, domain);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void OrganizationalUnitConstructorTest1()
        {
            new OrganizationalUnit(string.Empty, "CN=Test", DomainAccountTest.objects.First());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void OrganizationalUnitConstructorTest2()
        {
            new OrganizationalUnit("   ", "CN=Test", DomainAccountTest.objects.First());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void OrganizationalUnitConstructorTest4()
        {
            new OrganizationalUnit("test", string.Empty, DomainAccountTest.objects.First());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void OrganizationalUnitConstructorTest5()
        {
            new OrganizationalUnit("test", "   ", DomainAccountTest.objects.First());
        }
    }
}
