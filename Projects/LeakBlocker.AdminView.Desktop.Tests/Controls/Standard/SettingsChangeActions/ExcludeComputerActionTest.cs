using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.AdminView.Desktop.Controls.Standard.SettingsChangeActions;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;
using LeakBlocker.ServerShared.AdminViewCommunication;
using LeakBlocker.ServerShared.AdminViewCommunication.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.AdminView.Desktop.Tests.Controls.Standard.SettingsChangeActions
{
    partial class ExcludeComputerActionTest
    {
        private static IEnumerable<ExcludeComputerAction> GetInstances()
        {
            foreach (BaseComputerAccount computer in BaseComputerAccountTest.objects)
            {
                yield return new ExcludeComputerAction(computer);
            }
        }

        protected override bool SkipSerializationTest()
        {
            return true;
        }

        [TestMethod]
        public void PatchConfigTest()
        {
            BaseComputerAccount oldComputer = BaseComputerAccountTest.First;
            BaseComputerAccount newComputer = BaseComputerAccountTest.Second;

            var target = new ExcludeComputerAction(newComputer);

            var config = SimpleConfigurationTest.First.GetFromCurrent(excludedScopes: new[] { new Scope(oldComputer) });

            SimpleConfiguration newConfig = target.AddSettings(config);

            Assert.IsNotNull(newConfig);
            Assert.AreEqual(2, newConfig.ExcludedScopes.Count);
            Assert.IsTrue(newConfig.ExcludedScopes.Select(scope => scope.TargetObject).Contains(oldComputer));
            Assert.IsTrue(newConfig.ExcludedScopes.Select(scope => scope.TargetObject).Contains(newComputer));
        }
    }
}
