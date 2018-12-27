using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules.Conditions;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Libraries.Common.Tests.Entities.Settings.Rules.Conditions
{
    partial class ComputerListRuleConditionTest
    {
        private static IEnumerable<ComputerListRuleCondition> GetInstances()
        {
            foreach (bool condition in new[] { true, false })
            {
                yield return new ComputerListRuleCondition(
                    condition,
                    DomainAccountTest.objects.Take(2).ToReadOnlySet(),
                    OrganizationalUnitTest.objects.Take(2).ToReadOnlySet(),
                    DomainGroupAccountTest.objects.Take(2).ToReadOnlySet(),
                    BaseComputerAccountTest.objects.ToReadOnlySet());

                yield return new ComputerListRuleCondition(
                    condition,
                    DomainAccountTest.objects.Skip(1).ToReadOnlySet(),
                    OrganizationalUnitTest.objects.Skip(1).ToReadOnlySet(),
                    DomainGroupAccountTest.objects.Skip(1).ToReadOnlySet(),
                    BaseComputerAccountTest.objects.Take(0).ToReadOnlySet());

                yield return new ComputerListRuleCondition(
                    condition,
                    DomainAccountTest.objects.Skip(1).ToReadOnlySet(),
                    OrganizationalUnitTest.objects.Skip(1).ToReadOnlySet(),
                    DomainGroupAccountTest.objects.Take(0).ToReadOnlySet(),
                    BaseComputerAccountTest.objects.ToReadOnlySet());

                yield return new ComputerListRuleCondition(
                    condition,
                    DomainAccountTest.objects.Skip(1).ToReadOnlySet(),
                    OrganizationalUnitTest.objects.Take(0).ToReadOnlySet(),
                    DomainGroupAccountTest.objects.Skip(1).ToReadOnlySet(),
                    BaseComputerAccountTest.objects.ToReadOnlySet());

                yield return new ComputerListRuleCondition(
                    condition,
                    DomainAccountTest.objects.Take(0).ToReadOnlySet(),
                    OrganizationalUnitTest.objects.Skip(1).ToReadOnlySet(),
                    DomainGroupAccountTest.objects.Skip(1).ToReadOnlySet(),
                    BaseComputerAccountTest.objects.ToReadOnlySet());

                yield return new ComputerListRuleCondition(
                    condition,
                    DomainAccountTest.objects.Take(0).ToReadOnlySet(),
                    OrganizationalUnitTest.objects.Take(0).ToReadOnlySet(),
                    DomainGroupAccountTest.objects.Take(0).ToReadOnlySet(),
                    BaseComputerAccountTest.objects.Take(0).ToReadOnlySet());
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ComputerListRuleConditionConstructorTest2()
        {
            new ComputerListRuleCondition(false, DomainAccountTest.objects.Take(2).ToReadOnlySet(), OrganizationalUnitTest.objects.Take(2).ToReadOnlySet(),
                DomainGroupAccountTest.objects.Take(2).ToReadOnlySet(), new List<BaseComputerAccount>() { null }.ToReadOnlySet());
        }

        [TestMethod]
        public void CreateFromScopeListTest_CheckForNulls()
        {
            base.CheckSecondForNull<bool, IEnumerable<IScopeObject>>(false, (param1, param2) => ComputerListRuleCondition.CreateFromScopeList(param1, param2.ToReadOnlySet()));
        }

        [TestMethod]
        public void CreateFromScopeListTest_Valid()
        {
            var scopes = new IScopeObject[] { DomainAccountTest.objects.First(), BaseComputerAccountTest.objects.First() };

            ComputerListRuleCondition result = ComputerListRuleCondition.CreateFromScopeList(false, scopes.ToReadOnlySet());

            Assert.IsNotNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateFromScopeListTest_InvalidScopes()
        {
            var scopes = new IScopeObject[] { BaseUserAccountTest.objects.First() };

            ComputerListRuleCondition result = ComputerListRuleCondition.CreateFromScopeList(false, scopes.ToReadOnlySet());
        }
    }
}
