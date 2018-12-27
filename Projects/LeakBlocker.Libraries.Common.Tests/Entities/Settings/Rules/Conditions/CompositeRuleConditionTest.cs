using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules.Conditions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Libraries.Common.Tests.Entities.Settings.Rules.Conditions
{
    partial class CompositeRuleConditionTest
    {
        private static IEnumerable<CompositeRuleCondition> GetInstances()
        {
            foreach (CompositeRuleConditionType conditionType in EnumHelper<CompositeRuleConditionType>.Values)
            {
                yield return new CompositeRuleCondition(
                    true, 
                    new List<BaseRuleCondition> { 
                        UserListRuleConditionTest.objects.First(),
                        DeviceListRuleConditionTest.objects.First(),
                        ComputerListRuleConditionTest.objects.First()}.ToReadOnlySet(), 
                    conditionType);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CompositeRuleConditionConstructorTest1()
        {
            new CompositeRuleCondition(false, new List<BaseRuleCondition>().ToReadOnlySet(), CompositeRuleConditionType.LogicalAnd);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CompositeRuleConditionConstructorTest2()
        {
            new CompositeRuleCondition(false, new List<BaseRuleCondition> { null }.ToReadOnlySet(), CompositeRuleConditionType.LogicalAnd);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CompositeRuleConditionConstructorTest4()
        {
            new CompositeRuleCondition(false, new List<BaseRuleCondition>().ToReadOnlySet(), CompositeRuleConditionType.LogicalOr);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CompositeRuleConditionConstructorTest5()
        {
            new CompositeRuleCondition(false, new List<BaseRuleCondition> { null }.ToReadOnlySet(), CompositeRuleConditionType.LogicalOr);
        }
    }
}
