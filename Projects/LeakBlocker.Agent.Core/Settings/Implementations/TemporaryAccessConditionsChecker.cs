using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Entities.Settings.TemporaryAccess;

namespace LeakBlocker.Agent.Core.Settings.Implementations
{
    internal sealed class TemporaryAccessConditionsChecker : ITemporaryAccessConditionsChecker
    {
        private delegate bool Matcher(DeviceDescription device, BaseComputerAccount computer, BaseUserAccount user, BaseTemporaryAccessCondition condition);

        private static readonly ReadOnlyDictionary<Type, Matcher> matchers = new Dictionary<Type, Matcher>
        {
            { typeof(ComputerTemporaryAccessCondition), IsMatchedComputerCondition },
            { typeof(UserTemporaryAccessCondition), IsMatchedUserCondition },
            { typeof(DeviceTemporaryAccessCondition), IsMatchedDeviceCondition },
        }.ToReadOnlyDictionary();
        
        public bool IsMatched(DeviceDescription device, BaseComputerAccount computer, BaseUserAccount user, BaseTemporaryAccessCondition condition)
        {
            if (condition.EndTime <= Time.Now)
                return false;

            Type conditionType = condition.GetType();

            if (!matchers.ContainsKey(conditionType))
                Exceptions.Throw(ErrorMessage.NotFound, "Unable to check the condition of type {0}".Combine(condition));

            return matchers[conditionType](device, computer, user, condition);
        }

        private static bool IsMatchedDeviceCondition(DeviceDescription device, BaseComputerAccount computer, BaseUserAccount user, BaseTemporaryAccessCondition condition)
        {
            var currentCondition = (DeviceTemporaryAccessCondition)condition;

            return Equals(currentCondition.Device, device);
        }

        private static bool IsMatchedUserCondition(DeviceDescription device, BaseComputerAccount computer, BaseUserAccount user, BaseTemporaryAccessCondition condition)
        {
            var currentCondition = (UserTemporaryAccessCondition)condition;

            return Equals(currentCondition.User, user);
        }

        private static bool IsMatchedComputerCondition(DeviceDescription device, BaseComputerAccount computer, BaseUserAccount user, BaseTemporaryAccessCondition condition)
        {
            var currentCondition = (ComputerTemporaryAccessCondition)condition;

            return Equals(currentCondition.Computer, computer);
        }
    }
}
