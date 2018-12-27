using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Agent.Core.Settings.Implementations;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Tests;
using LeakBlocker.Libraries.Common.Tests.Entities.Settings.Rules.Conditions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Agent.Core.Tests.Settings.Implementations
{
    [TestClass]
    public sealed class RuleConditionCheckerTest : BaseTest
    {
        [TestMethod]
        public void AllConditionsTest()
        {
            Mocks.ReplayAll();

            var target = new RuleConditionChecker();

            foreach (var ruleCondition in BaseRuleConditionTest.objects)
            {
                foreach (var state in AgentComputerStateTest.objects)
                {
                    foreach (var device in state.ConnectedDevices)
                    {
                        foreach (var user in state.LoggedOnUsers.Keys)
                        {
                            target.IsMatched(state, device, user, ruleCondition);
                        }
                    }

                }
            }
        }
    }
}
