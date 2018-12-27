using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Settings;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules;
using LeakBlocker.Libraries.Common.Entities.Settings.TemporaryAccess;
using LeakBlocker.Libraries.Common.Tests.Entities.Settings.Rules;
using LeakBlocker.Libraries.Common.Tests.Entities.Settings.TemporaryAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Libraries.Common.Tests.Entities.Settings
{
    partial class ProgramConfigurationTest
    {
        private static IEnumerable<ProgramConfiguration> GetInstances()
        {
            yield return new ProgramConfiguration(15, RuleTest.objects.Take(50).ToReadOnlySet(), ComputerTemporaryAccessConditionTest.objects.ToReadOnlySet());
            yield return new ProgramConfiguration(20, RuleTest.objects.Take(2).ToReadOnlySet(), ComputerTemporaryAccessConditionTest.objects.ToReadOnlySet());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ProgramConfigurationConstructorTest0()
        {
            new ProgramConfiguration(0, RuleTest.objects.ToReadOnlySet(), ComputerTemporaryAccessConditionTest.objects.ToReadOnlySet());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ProgramConfigurationConstructorTest2()
        {
            new ProgramConfiguration(123, new List<Rule> { null }.ToReadOnlySet(), ComputerTemporaryAccessConditionTest.objects.ToReadOnlySet());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ProgramConfigurationConstructorTest3()
        {
            new ProgramConfiguration(123, new List<Rule>().ToReadOnlySet(), ComputerTemporaryAccessConditionTest.objects.ToReadOnlySet());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ProgramConfigurationConstructorTest4()
        {
            new ProgramConfiguration(123, RuleTest.objects.ToReadOnlySet(), new List<BaseTemporaryAccessCondition> { null }.ToReadOnlySet());
        }
    }
}
