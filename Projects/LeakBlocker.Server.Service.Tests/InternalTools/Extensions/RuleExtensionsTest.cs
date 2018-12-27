using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules.Conditions;
using LeakBlocker.Libraries.Common.Tests;
using LeakBlocker.Libraries.Common.Tests.Entities.Settings.Rules;
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
    public sealed class RuleExtensionsTest : BaseTest
    {
        private static readonly ActionData unblockDisableAudit = new ActionData(BlockActionType.Unblock, AuditActionType.DisableAudit);
        private static readonly ActionData onlyUnblock = new ActionData(BlockActionType.Unblock, AuditActionType.Skip);

        [TestInitialize]
        public void Init()
        {
            var cache = Mocks.StrictMock<ISecurityObjectCache>();

            InternalObjects.Singletons.SecurityObjectCache.SetTestImplementation(cache);

            cache.Stub(x => x.Data).Return(DomainSnapshotTest.Second);
        }

        [TestMethod]
        public void GetExcludedComputers_Empty()
        {
            Mocks.ReplayAll();

            var rule = new Rule("test", 1, UserListRuleConditionTest.objects.First(), unblockDisableAudit);

            Assert.AreEqual(0, RuleExtensions.GetExcludedComputers(rule).Count());
        }

        [TestMethod]
        public void GetExcludedComputers_WrongActions()
        {
            Mocks.ReplayAll();

            ComputerListRuleCondition condition = ComputerListRuleConditionTest.objects.First();

            var rule = new Rule("test", 1, condition, onlyUnblock);

            Assert.AreEqual(0, RuleExtensions.GetExcludedComputers(rule).ToReadOnlySet().Count);
        }

        [TestMethod]
        public void GetExcludedComputers_Sussess()
        {
            var auditItemHelper = Mocks.StrictMock<IAuditItemHelper>();

            InternalObjects.Singletons.AuditItemHelper.SetTestImplementation(auditItemHelper);

            auditItemHelper.Stub(x => x.DomainMemberIsNotAccessible(null)).IgnoreArguments();
            
            Mocks.ReplayAll();

            var rule = new Rule("test", 1, ComputerListRuleConditionTest.objects.First(), unblockDisableAudit);

            Assert.AreNotEqual(0, RuleExtensions.GetExcludedComputers(rule).Count());
        }

        [TestMethod]
        public void GetDomains()
        {
            Mocks.ReplayAll();

            foreach (Rule rule in RuleTest.objects)
            {
                var domains = rule.GetDomains().ToReadOnlySet();

                Assert.IsNotNull(domains);
            }
        }
    }
}
