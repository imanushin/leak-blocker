using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Agent.Core.Settings;
using LeakBlocker.Agent.Core.Settings.Implementations;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Entities.Settings;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules.Conditions;
using LeakBlocker.Libraries.Common.Entities.Settings.TemporaryAccess;
using LeakBlocker.Libraries.Common.Tests;
using LeakBlocker.Libraries.Common.Tests.Entities;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;
using LeakBlocker.Libraries.Common.Tests.Entities.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace LeakBlocker.Agent.Core.Tests.Settings.Implementations
{
    [TestClass]
    public sealed class ProgramConfigurationCheckerTest : BaseTest
    {
        private static readonly BaseUserAccount baseUserAccount = BaseUserAccountTest.First;
        private static readonly DeviceDescription device = DeviceDescriptionTest.First;
        private static readonly BaseComputerAccount computer = BaseComputerAccountTest.First;

        private IRuleConditionChecker ruleConditionChecker;
        private ITemporaryAccessConditionsChecker temporaryAccessConditionsChecker;
        private ProgramConfigurationChecker target;
        [TestInitialize]
        public void Initialize()
        {
            ruleConditionChecker = Mocks.StrictMock<IRuleConditionChecker>();
            temporaryAccessConditionsChecker = Mocks.StrictMock<ITemporaryAccessConditionsChecker>();

            AgentObjects.Singletons.RuleConditionChecker.SetTestImplementation(ruleConditionChecker);
            AgentObjects.Singletons.TemporaryAccessConditionsChecker.SetTestImplementation(temporaryAccessConditionsChecker);

            target = new ProgramConfigurationChecker();
        }


        [TestMethod]
        public void CheckAllItems()
        {
            foreach (bool ruleResult in new[] { true, false })
            {
                foreach (bool temporaryAccessResult in new[] { true, false })
                {
                    Mocks.BackToRecordAll();

                    ruleConditionChecker.Stub(x => x.IsMatched(null, null, null, null)).IgnoreArguments().Return(ruleResult);
                    temporaryAccessConditionsChecker.Stub(x => x.IsMatched(null, null, null, null)).IgnoreArguments().Return(temporaryAccessResult);

                    Mocks.ReplayAll();

                    foreach (ProgramConfiguration configuration in ProgramConfigurationTest.objects)
                    {
                        foreach (AgentComputerState computerState in AgentComputerStateTest.objects)
                        {
                            var result = target.GetRequiredActions(computerState, configuration);

                            Assert.IsNotNull(result);
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void SimpleFullBlockTest()
        {
            ruleConditionChecker.Stub(x => x.IsMatched(null, null, null, null)).IgnoreArguments().Return(true);
            temporaryAccessConditionsChecker.Stub(x => x.IsMatched(null, null, null, null)).IgnoreArguments().Return(false);

            Mocks.ReplayAll();

            ProgramConfiguration config = CreateConfig(new ActionData(BlockActionType.Complete, AuditActionType.DeviceAndFiles));

            var state = new AgentComputerState(
                computer,
                ReadOnlySet<AccountSecurityIdentifier>.Empty,
                new Dictionary<BaseUserAccount, ReadOnlySet<AccountSecurityIdentifier>>() { { baseUserAccount, ReadOnlySet<AccountSecurityIdentifier>.Empty } }.ToReadOnlyDictionary(),
                new[] { device }.ToReadOnlySet());


            RuleCheckResult result = target.GetRequiredActions(state, config);

            CommonActionData data = result[baseUserAccount, device];

            Assert.AreEqual(DeviceAccessType.Blocked, data.Access);
            Assert.AreEqual(AuditActionType.DeviceAndFiles, data.Audit);
        }

        [TestMethod]
        public void SimpleTemporaryAccessTest()
        {
            ruleConditionChecker.Stub(x => x.IsMatched(null, null, null, null)).IgnoreArguments().Return(true);
            temporaryAccessConditionsChecker.Stub(x => x.IsMatched(null, null, null, null)).IgnoreArguments().Return(true);

            Mocks.ReplayAll();

            ProgramConfiguration config = CreateConfig(
                new ActionData(BlockActionType.Complete, AuditActionType.DeviceAndFiles),
                new ComputerTemporaryAccessCondition(computer, new Time(DateTime.Now.AddHours(1)), false ));

            var state = new AgentComputerState(
                computer,
                ReadOnlySet<AccountSecurityIdentifier>.Empty,
                new Dictionary<BaseUserAccount, ReadOnlySet<AccountSecurityIdentifier>>() { { baseUserAccount, ReadOnlySet<AccountSecurityIdentifier>.Empty } }.ToReadOnlyDictionary(),
                new[] { device }.ToReadOnlySet());


            RuleCheckResult result = target.GetRequiredActions(state, config);

            CommonActionData data = result[baseUserAccount, device];

            Assert.AreEqual(DeviceAccessType.TemporarilyAllowed, data.Access);
            Assert.AreEqual(AuditActionType.DeviceAndFiles, data.Audit);
        }

        private static ProgramConfiguration CreateConfig(ActionData actionData)
        {
            return CreateConfig(actionData, new UserTemporaryAccessCondition(baseUserAccount, Time.Unknown, false));
        }

        private static ProgramConfiguration CreateConfig(ActionData actionData, BaseTemporaryAccessCondition temporaryAccess)
        {
            return new ProgramConfiguration(
                2,
                new[]{new Rule(
                    "Block Computer", 
                    5, 
                    new ComputerListRuleCondition(
                        false,
                        ReadOnlySet<DomainAccount>.Empty,
                        ReadOnlySet<OrganizationalUnit>.Empty,
                        ReadOnlySet<DomainGroupAccount>.Empty,
                        new[]{computer}.ToReadOnlySet()),
                    actionData) }.ToReadOnlySet(),
                    new[] { temporaryAccess }.ToReadOnlySet());
        }
    }
}
