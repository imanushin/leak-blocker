using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Agent.Core.Settings;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Tests.Entities;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;

namespace LeakBlocker.Agent.Core.Tests.Settings
{
    partial class AgentComputerStateTest
    {
        private static IEnumerable<AgentComputerState> GetInstances()
        {
            foreach (var computer in BaseComputerAccountTest.objects)
            {
                BaseGroupAccount localAdministerGroup;

                BaseUserAccount localSystem;

                BaseUserAccount someLoggedOnUser;

                if (computer is DomainComputerAccount)
                {
                    localAdministerGroup = new DomainComputerGroupAccount("Administrators", AccountSecurityIdentifierTest.First, (DomainComputerAccount)computer);
                    localSystem = new DomainComputerUserAccount("SYSTEM", AccountSecurityIdentifierTest.First, (DomainComputerAccount)computer);
                    someLoggedOnUser = new DomainUserAccount("SomeUser", AccountSecurityIdentifierTest.Second, ((DomainComputerAccount)computer).Parent, "SomeUser");
                }
                else
                {
                    localAdministerGroup = new LocalGroupAccount("Administrators", AccountSecurityIdentifierTest.First, (LocalComputerAccount)computer);

                    localSystem = new LocalUserAccount("SYSTEM", AccountSecurityIdentifierTest.First, (LocalComputerAccount)computer);
                    someLoggedOnUser = new LocalUserAccount("SomeUser", AccountSecurityIdentifierTest.Second, ((LocalComputerAccount)computer));
                }

                var standardUsers = new Dictionary<BaseUserAccount, ReadOnlySet<AccountSecurityIdentifier>>()
                                    {
                                        {localSystem, new[] {localAdministerGroup.Identifier}.ToReadOnlySet()},
                                        {someLoggedOnUser, new[] {localAdministerGroup.Identifier}.ToReadOnlySet()},
                                    };

                foreach (var computerGroups in new[] { DomainGroupAccountTest.objects.Objects, ReadOnlySet<DomainGroupAccount>.Empty })
                {
                    foreach (var devices in new[] { DeviceDescriptionTest.objects.Objects, ReadOnlySet<DeviceDescription>.Empty })
                    {
                        foreach (var userGroups in new[]
                        {
                            new Dictionary<BaseUserAccount,ReadOnlySet<AccountSecurityIdentifier>>(standardUsers)
                                {
                                },
                        new Dictionary<BaseUserAccount,ReadOnlySet<AccountSecurityIdentifier>>(standardUsers)
                                {
                                    {
                                        new LocalUserAccount("SomeAnotherUser", AccountSecurityIdentifierTest.Third, LocalComputerAccountTest.First),
                                        new []{ AccountSecurityIdentifierTest.Third}.ToReadOnlySet()
                                    }
                                }
                            }
                        )
                        {
                            yield return new AgentComputerState(
                                computer,
                                computerGroups.Select(group => group.Identifier).ToReadOnlySet(),
                                userGroups.ToReadOnlyDictionary(),
                                devices);
                        }

                    }
                }
            }
        }

        protected override bool SkipSerializationTest()
        {
            return true;
        }
    }
}
