using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules.Conditions;
using LeakBlocker.Libraries.Common.Tests.Entities.Settings.Rules.Conditions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Libraries.Common.Tests.Entities.Settings.Rules
{
    partial class RuleTest
    {
        internal static IEnumerable<Rule> GetInstances()
        {
            int conditionsCount = BaseRuleConditionTest.objects.Count();
            int actionsCount = ActionDataTest.objects.Count();

            for (int index = 0; index < Math.Max(conditionsCount, actionsCount); index++)
            {
                BaseRuleCondition condition = BaseRuleConditionTest.objects.Skip(index % conditionsCount).First();
                ActionData actions = ActionDataTest.objects.Skip(index % actionsCount).First();

                yield return new Rule("rule " + index, index + 128, condition, actions);
            }
        }


        [TestMethod]
        public void RuleCompare()
        {
            var first = new Rule("rule", 1, BaseRuleConditionTest.objects.First(), ActionDataTest.objects.First());
            var second = new Rule("rule", 2, BaseRuleConditionTest.objects.First(), ActionDataTest.objects.First());

            Assert.IsTrue(Rule.CompareRulesUsingPriority(first, second) < 0);

            var rules = new List<Rule>() { first, second };//Чтобы проверить, что в списке они будут правильно отсортированы

            rules.Sort(Rule.CompareRulesUsingPriority);

            Assert.AreEqual(first, rules[0]);
            Assert.AreEqual(second, rules[1]);
        }
    }
}
