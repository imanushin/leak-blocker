using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Audit;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;

namespace LeakBlocker.Libraries.Common.Tests.Entities.Audit
{
    partial class AuditFilterTest
    {
        private static IEnumerable<AuditFilter> GetInstances()
        {
            foreach (var users in new[]
                                             {
                                                 BaseUserAccountTest.objects,
                                                 new BaseUserAccount[0],
                                                 BaseUserAccountTest.objects.Take(1)
                                             })
            {

                foreach (var computers in new[]
                                             {
                                                 BaseComputerAccountTest.objects,
                                                 new BaseComputerAccount[0],
                                                 BaseComputerAccountTest.objects.Take(1)
                                             })
                {
                    foreach (var devices in new[]
                                                {
                                                    DeviceDescriptionTest.objects,
                                                    new DeviceDescription[0],
                                                    DeviceDescriptionTest.objects.Take(1)
                                                })
                    {
                        foreach (Time startTime in TimeTest.objects)
                        {
                            foreach (Time endTime in TimeTest.objects)
                            {
                                foreach (string name in new[] { "name1, name2" })
                                {
                                    foreach (var onlyError in new[] { true, false })
                                    {
                                        foreach (var group in new IEnumerable<AuditItemGroupType>[]
                                                           {
                                                               EnumHelper<AuditItemGroupType>.Values,
                                                               ReadOnlySet<AuditItemGroupType>.Empty
                                                           })
                                        {
                                            yield return new AuditFilter(name, computers.ToReadOnlySet(), users.ToReadOnlySet(), devices.ToReadOnlySet(), startTime, endTime, onlyError, group.ToReadOnlySet());
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


    }
}
