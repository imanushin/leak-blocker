using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules.Conditions;
using LeakBlocker.Libraries.Common.Tests;
using LeakBlocker.Libraries.Common.Tests.Entities.Settings.Rules.Conditions;
using LeakBlocker.Libraries.SystemTools.Tests.Entities;
using LeakBlocker.Libraries.SystemTools.Tests.Entities.Implementations;
using LeakBlocker.Server.Service.InternalTools;
using LeakBlocker.Server.Service.InternalTools.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace LeakBlocker.Server.Service.Tests.InternalTools.Extensions
{
    [TestClass]
    public sealed class RuleConditionExtensionsTest : BaseTest
    {
        private ISecurityObjectCache cache;
        private IAuditItemHelper auditItemHelper;

        [TestInitialize]
        public void Init()
        {
            cache = Mocks.StrictMock<ISecurityObjectCache>();
            auditItemHelper = Mocks.StrictMock<IAuditItemHelper>();

            InternalObjects.Singletons.SecurityObjectCache.SetTestImplementation(cache);
            InternalObjects.Singletons.AuditItemHelper.SetTestImplementation(auditItemHelper);
        }

        [TestMethod]
        public void GetAllComputersUsedInCondition_TestExisting()
        {
            var container = DomainSnapshotTest.Second;

            auditItemHelper.Stub(x => x.DomainMemberIsNotAccessible(null)).IgnoreArguments();
            cache.Stub(x => x.Data).Return(container);

            Mocks.ReplayAll();

            foreach (BaseRuleCondition condition in BaseRuleConditionTest.objects)
            {
                var result = condition.GetAllComputersUsedInCondition();

                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void GetAllComputersUsedInCondition_StrongRequest()
        {
            var realData = DomainSnapshotTest.Third;

            cache.Stub(x => x.Data).Return(realData);

            Mocks.ReplayAll();

            var group1 = realData.Groups.OfType<DomainGroupAccount>().First(group => string.Equals(group.ShortName, "test group 1", StringComparison.OrdinalIgnoreCase));
            var group2 = realData.Groups.OfType<DomainGroupAccount>().First(group => string.Equals(group.ShortName, "test group 2", StringComparison.OrdinalIgnoreCase));

            var firstCondition = new ComputerListRuleCondition(false, new DomainAccount[0].ToReadOnlySet(), new OrganizationalUnit[0].ToReadOnlySet(),
                                           new[] { group1 }.ToReadOnlySet(), new BaseComputerAccount[0].ToReadOnlySet());

            var secondCondition = new ComputerListRuleCondition(false, new DomainAccount[0].ToReadOnlySet(), new OrganizationalUnit[0].ToReadOnlySet(),
                                           new[] { group2 }.ToReadOnlySet(), new BaseComputerAccount[0].ToReadOnlySet());

            var compositeCondition = new CompositeRuleCondition(false, new[] { firstCondition, secondCondition }.ToReadOnlySet(),
                                                                CompositeRuleConditionType.LogicalOr);

            var expectedResult =
                realData.GetObjectsInGroup(group1).Union(realData.GetObjectsInGroup(group2)).OfType
                    <BaseComputerAccount>().ToReadOnlySet();

            var result = compositeCondition.GetAllComputersUsedInCondition();

            Assert.AreEqual(expectedResult, result.ToReadOnlySet());
        }

        [TestMethod]
        public void GetAllComputersUsedInCondition_SkipOtherConditions()
        {
            var container = DomainSnapshotTest.Second;

            cache.Stub(x => x.Data).Return(container);

            Mocks.ReplayAll();

            var condition = new UserListRuleCondition(false, container.Domains, container.OrganizationalUnits,
                                                      container.Groups, container.Users);

            var result = condition.GetAllComputersUsedInCondition();

            Assert.AreEqual(new BaseComputerAccount[0].ToReadOnlySet(), result.ToReadOnlySet());
        }

        [TestMethod]
        public void GetDomains()
        {
            Mocks.ReplayAll();

            foreach (var condition in BaseRuleConditionTest.objects)
            {
                Assert.IsNotNull(condition.GetDomains().ToReadOnlySet());
            }
        }
    }
}
