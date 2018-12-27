using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Settings.TemporaryAccess;

namespace LeakBlocker.Libraries.Common.Tests.Entities.Settings.TemporaryAccess
{
    partial class DeviceTemporaryAccessConditionTest
    {
        private static IEnumerable<DeviceTemporaryAccessCondition> GetInstances()
        {
            foreach (var device in DeviceDescriptionTest.objects)
            {
                foreach (bool allowWrite in new[] { true, false })
                {
                    foreach (Time testDate in TimeTest.objects)
                    {
                        yield return new DeviceTemporaryAccessCondition(device, testDate, allowWrite);
                    }
                }
            }
        }
    }
}

