using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;

namespace LeakBlocker.Libraries.Common.Tests.Entities
{
    partial class DeviceAccessMapTest
    {
        private static IEnumerable<DeviceAccessMap> GetInstances()
        {
            yield return DeviceAccessMap.Empty;

            yield return new DeviceAccessMap(
                    BaseUserAccountTest.objects.Take(1).ToReadOnlySet(),
                    DeviceDescriptionTest.objects.Take(1).ToReadOnlySet(),
                    (user,device)=> DeviceAccessType.Blocked) ;

            int item = 0;

            yield return new DeviceAccessMap(
                BaseUserAccountTest.objects.Objects, 
                DeviceDescriptionTest.objects.Objects,
                (user, device) => EnumHelper<DeviceAccessType>.Values.ToList()[(item++) % EnumHelper<DeviceAccessType>.Values.Count]);
        }

        protected override void CheckArgumentExceptionParameter(string expectedParameterName, string actualParameterName)
        {
            return;
        }
    }
}
