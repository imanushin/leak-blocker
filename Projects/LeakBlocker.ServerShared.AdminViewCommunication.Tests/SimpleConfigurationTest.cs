using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Settings.TemporaryAccess;
using LeakBlocker.Libraries.Common.Tests.Entities;
using LeakBlocker.Libraries.Common.Tests.Entities.Settings.TemporaryAccess;

namespace LeakBlocker.ServerShared.AdminViewCommunication.Tests
{
    partial class SimpleConfigurationTest
    {
        private static IEnumerable<SimpleConfiguration> GetInstances()
        {
            foreach (bool isBlockEnabled in new[] { true, false })
            {
                foreach (bool isReadonlyAccessEnabled in new[] { true, false })
                {
                    foreach (bool areInputDevicesAllowed in new[] { true, false })
                    {
                        foreach (bool isFileAuditEnabled in new[] { true, false })
                        {
                            foreach (var devices in new IEnumerable<DeviceDescription>[] { new DeviceDescription[0], DeviceDescriptionTest.objects })
                            {
                                foreach (var users in new[] { new Scope[0], ScopeTest.GetUserContainers() })
                                {
                                    foreach (var blockedArea in new[] { new Scope[0], ScopeTest.GetComputerContainers() })
                                    {
                                        foreach (var excludedArea in new[] { new Scope[0], ScopeTest.GetComputerContainers() })
                                        {
                                            foreach (
                                                var temporaryAccess in
                                                    new IEnumerable<BaseTemporaryAccessCondition>[] { new BaseTemporaryAccessCondition[0], BaseTemporaryAccessConditionTest.objects })
                                            {
                                                yield return new SimpleConfiguration(
                                                    isBlockEnabled,
                                                    isReadonlyAccessEnabled,
                                                    areInputDevicesAllowed,
                                                    isFileAuditEnabled,
                                                    blockedArea,
                                                    excludedArea,
                                                    devices,
                                                    users,
                                                    temporaryAccess
                                                    );
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
}
