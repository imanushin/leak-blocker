using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules.Conditions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Libraries.Common.Tests.Entities.Settings.Rules.Conditions
{
    partial class DeviceListRuleConditionTest
    {
        private static IEnumerable<DeviceListRuleCondition> GetInstances()
        {
            foreach (bool condition in new[] { true, false })
            {
                yield return new DeviceListRuleCondition(condition, DeviceDescriptionTest.objects.Objects);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DeviceListRuleConditionConstructorTest1()
        {
            new DeviceListRuleCondition(false, new List<DeviceDescription> { null }.ToReadOnlySet());
        }
    }
}
