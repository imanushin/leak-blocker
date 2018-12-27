using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Agent.Core.Settings.Implementations;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Entities.Settings.TemporaryAccess;
using LeakBlocker.Libraries.Common.Tests;
using LeakBlocker.Libraries.Common.Tests.Entities;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;
using LeakBlocker.Libraries.Common.Tests.Entities.Settings.TemporaryAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Agent.Core.Tests.Settings.Implementations
{
    [TestClass]
    public sealed class TemporaryAccessConditionsCheckerTest : BaseTest
    {
        [TestMethod]
        public void AllConditionsTest()
        {
            Mocks.ReplayAll();

            var target = new TemporaryAccessConditionsChecker();

            foreach (DeviceDescription device in DeviceDescriptionTest.objects)
            {
                foreach (BaseUserAccount user in BaseUserAccountTest.objects)
                {
                    foreach (BaseComputerAccount computer in BaseComputerAccountTest.objects)
                    {
                        foreach (BaseTemporaryAccessCondition condition in BaseTemporaryAccessConditionTest.objects)
                        {
                            bool result = target.IsMatched(device, computer, user, condition);

                            if (condition.EndTime <= Time.Now)
                                Assert.IsFalse(result);
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void DeviceAccess()
        {
            Mocks.ReplayAll();

            var target = new TemporaryAccessConditionsChecker();

            foreach (DeviceDescription device in DeviceDescriptionTest.objects)
            {
                foreach (var condition in DeviceTemporaryAccessConditionTest.objects.Where(cond => cond.EndTime > Time.Now))
                {
                    bool result = target.IsMatched(device, BaseComputerAccountTest.First, BaseUserAccountTest.First, condition);

                    Assert.AreEqual(Equals(condition.Device, device), result);
                }
            }
        }

        [TestMethod]
        public void ComputerAccess()
        {
            Mocks.ReplayAll();

            var target = new TemporaryAccessConditionsChecker();

            foreach (BaseComputerAccount computer in BaseComputerAccountTest.objects)
            {
                foreach (var condition in ComputerTemporaryAccessConditionTest.objects.Where(cond => cond.EndTime > Time.Now))
                {
                    bool result = target.IsMatched(DeviceDescriptionTest.First, computer, BaseUserAccountTest.First, condition);

                    Assert.AreEqual(Equals(condition.Computer, computer), result);
                }
            }
        }

        [TestMethod]
        public void UserAccess()
        {
            Mocks.ReplayAll();

            var target = new TemporaryAccessConditionsChecker();

            foreach (BaseUserAccount user in BaseUserAccountTest.objects)
            {
                foreach (var condition in UserTemporaryAccessConditionTest.objects.Where(cond => cond.EndTime > Time.Now))
                {
                    bool result = target.IsMatched(DeviceDescriptionTest.First, BaseComputerAccountTest.First, user, condition);

                    Assert.AreEqual(Equals(condition.User, user), result);
                }
            }
        }
    }
}
