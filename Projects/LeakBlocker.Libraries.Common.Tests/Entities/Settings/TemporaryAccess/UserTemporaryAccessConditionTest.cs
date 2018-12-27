using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Settings.TemporaryAccess;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;

namespace LeakBlocker.Libraries.Common.Tests.Entities.Settings.TemporaryAccess
{
    partial class UserTemporaryAccessConditionTest
    {
        private static IEnumerable<UserTemporaryAccessCondition> GetInstances()
        {
            foreach (var user in BaseUserAccountTest.objects)
            {
                foreach (bool allowWrite in new[] { true, false })
                {
                    foreach (Time testDate in TimeTest.objects)
                    {
                        yield return new UserTemporaryAccessCondition(user, testDate, allowWrite);
                    }
                }
            }
        }
    }
}

