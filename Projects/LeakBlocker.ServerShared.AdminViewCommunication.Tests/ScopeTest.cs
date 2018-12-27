using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.ServerShared.AdminViewCommunication.Tests
{
    partial class ScopeTest
    {
        private static IEnumerable<Scope> GetInstances()
        {
            foreach (IScopeObject target in IScopeObjectTest.objects)
            {
                yield return new Scope(target);
            }
        }

        internal static IEnumerable<Scope> GetComputerContainers()
        {
            var result =
                BaseComputerAccountTest.objects.Cast<IScopeObject>().Union(
                    DomainAccountTest.objects).Union(
                    DomainGroupAccountTest.objects).Union(
                    OrganizationalUnitTest.objects);

            return result.Select(item => new Scope(item));
        }

        internal static IEnumerable<Scope> GetUserContainers()
        {
            var result =
                BaseUserAccountTest.objects.Cast<IScopeObject>().Union(
                    BaseGroupAccountTest.objects).Union(
                    OrganizationalUnitTest.objects);

            return result.Select(item => new Scope(item));
        }

        [TestMethod]
        public void CompareTest()
        {
            var first = new Scope(new LocalComputerAccount("Name1", AccountSecurityIdentifierTest.objects.First()));
            var second = new Scope(new LocalComputerAccount("Name2", AccountSecurityIdentifierTest.objects.First()));
            var third = new Scope(new LocalComputerAccount(" Name2", AccountSecurityIdentifierTest.objects.First()));

            Assert.IsTrue(third > first);
            Assert.IsTrue(first < third);

            Assert.IsTrue((Scope)null == (Scope)null);
            Assert.IsFalse((Scope)null != (Scope)null);

            Assert.IsTrue(null < first);
            Assert.IsTrue(first > null);

            Assert.IsFalse(null > first);
            Assert.IsFalse(first < null);
        }
    }
}
