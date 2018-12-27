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
    partial class UserListRuleConditionTest
    {
        private static IEnumerable<UserListRuleCondition> GetInstances()
        {
            foreach (bool condition in new[] { true, false })
            {
                yield return new UserListRuleCondition(condition, DomainAccountTest.objects.Take(2).ToReadOnlySet(), OrganizationalUnitTest.objects.Take(2).ToReadOnlySet(),
                    DomainGroupAccountTest.objects.Take(2).ToReadOnlySet(), DomainUserAccountTest.objects.Objects);

                yield return new UserListRuleCondition(condition, DomainAccountTest.objects.Skip(1).ToReadOnlySet(), OrganizationalUnitTest.objects.Skip(1).ToReadOnlySet(),
                    DomainGroupAccountTest.objects.Skip(1).ToReadOnlySet(), DomainUserAccountTest.objects.Objects);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UserListRuleConditionConstructorTest2()
        {
            new UserListRuleCondition(false, DomainAccountTest.objects.Take(2).ToReadOnlySet(), OrganizationalUnitTest.objects.Take(2).ToReadOnlySet(),
                DomainGroupAccountTest.objects.Take(2).ToReadOnlySet(), new List<DomainUserAccount>() { null }.ToReadOnlySet());
        }


        [TestMethod]
        public void CreateFromScopeListTest_CheckForNulls()
        {
            base.CheckSecondForNull<bool, IEnumerable<IScopeObject>>(false, (param1, param2) => UserListRuleCondition.CreateFromScopeList(param1, param2.ToReadOnlySet()));
        }

        [TestMethod]
        public void CreateFromScopeListTest_Valid()
        {
            var scopes = new IScopeObject[] { DomainAccountTest.objects.First(), BaseUserAccountTest.objects.First() };

            UserListRuleCondition result = UserListRuleCondition.CreateFromScopeList(false, scopes.ToReadOnlySet());

            Assert.IsNotNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateFromScopeListTest_InvalidScopes()
        {
            var scopes = new IScopeObject[] { BaseComputerAccountTest.objects.First() };

            UserListRuleCondition.CreateFromScopeList(false, scopes.ToReadOnlySet().ToReadOnlySet());
        }

    }
}
