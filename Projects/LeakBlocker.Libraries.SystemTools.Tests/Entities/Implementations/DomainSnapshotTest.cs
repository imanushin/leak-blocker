using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;
using LeakBlocker.Libraries.SystemTools.Entities.Implementations;
using LeakBlocker.Libraries.SystemTools.Entities.Management;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Libraries.SystemTools.Tests.Entities.Implementations
{
    partial class DomainSnapshotTest 
    {
        internal static IEnumerable<DomainSnapshot> GetInstances()
        {
            var instance1 = (DomainSnapshot)FormatterServices.GetUninitializedObject(typeof(DomainSnapshot));
            var instance2 = (DomainSnapshot)FormatterServices.GetUninitializedObject(typeof(DomainSnapshot));

            typeof(DomainSnapshot).GetProperty("GroupMembership").SetValue(instance1, ReadOnlyDictionary<AccountSecurityIdentifier, AccountSecurityIdentifierSet>.Empty, null);
            typeof(DomainSnapshot).GetProperty("Users").SetValue(instance1, ReadOnlySet<BaseUserAccount>.Empty, null);
            typeof(DomainSnapshot).GetProperty("Groups").SetValue(instance1, ReadOnlySet<BaseGroupAccount>.Empty, null);
            typeof(DomainSnapshot).GetProperty("Computers").SetValue(instance1, ReadOnlySet<BaseComputerAccount>.Empty, null);
            typeof(DomainSnapshot).GetProperty("Domains").SetValue(instance1, ReadOnlySet<DomainAccount>.Empty, null);
            typeof(DomainSnapshot).GetProperty("OrganizationalUnits").SetValue(instance1, ReadOnlySet<OrganizationalUnit>.Empty, null);

            typeof(DomainSnapshot).GetProperty("GroupMembership").SetValue(instance2, ReadOnlyDictionary<AccountSecurityIdentifier, AccountSecurityIdentifierSet>.Empty, null);
            typeof(DomainSnapshot).GetProperty("Users").SetValue(instance2, BaseUserAccountTest.objects.ToReadOnlySet(), null);
            typeof(DomainSnapshot).GetProperty("Groups").SetValue(instance2, BaseGroupAccountTest.objects.ToReadOnlySet(), null);
            typeof(DomainSnapshot).GetProperty("Computers").SetValue(instance2, BaseComputerAccountTest.objects.ToReadOnlySet(), null);
            typeof(DomainSnapshot).GetProperty("Domains").SetValue(instance2, DomainAccountTest.objects.ToReadOnlySet(), null);
            typeof(DomainSnapshot).GetProperty("OrganizationalUnits").SetValue(instance2, OrganizationalUnitTest.objects.ToReadOnlySet(), null);

            yield return instance1;
            yield return instance2;

            var instance3 = (DomainSnapshot)FormatterServices.GetUninitializedObject(typeof(DomainSnapshot));
            
            var users = new HashSet<BaseUserAccount>
            {
                DomainUserAccountTest.First,
                DomainUserAccountTest.Second,
            };

            var groups = new HashSet<BaseGroupAccount>
            {
                new DomainGroupAccount("test group 1", DomainGroupAccountTest.First.Identifier, DomainGroupAccountTest.First.Parent, DomainGroupAccountTest.First.CanonicalName),
                new DomainGroupAccount("test group 2", DomainGroupAccountTest.Second.Identifier, DomainGroupAccountTest.Second.Parent, DomainGroupAccountTest.Second.CanonicalName),
            };

            var computers = new HashSet<BaseComputerAccount>
            {
                DomainComputerAccountTest.First,
                DomainComputerAccountTest.Second,
            };

            var domains = new HashSet<DomainAccount>
            {
                DomainAccountTest.First,
            };

            var containers = new HashSet<OrganizationalUnit>
            {
                OrganizationalUnitTest.First,
                OrganizationalUnitTest.Second,
            };

            var groupMembership = new Dictionary<AccountSecurityIdentifier, AccountSecurityIdentifierSet>
            {
                { groups.First().Identifier, new AccountSecurityIdentifierSet(new HashSet<AccountSecurityIdentifier> { computers.First().Identifier, users.First().Identifier }.ToReadOnlySet()) },
                { groups.Skip(1).First().Identifier, new AccountSecurityIdentifierSet(new HashSet<AccountSecurityIdentifier> { computers.Skip(1).First().Identifier, computers.Skip(1).First().Identifier }.ToReadOnlySet()) },
            };

            typeof(DomainSnapshot).GetProperty("GroupMembership").SetValue(instance3, groupMembership.ToReadOnlyDictionary(), null);
            typeof(DomainSnapshot).GetProperty("Users").SetValue(instance3, users.ToReadOnlySet(), null);
            typeof(DomainSnapshot).GetProperty("Groups").SetValue(instance3, groups.ToReadOnlySet(), null);
            typeof(DomainSnapshot).GetProperty("Computers").SetValue(instance3, computers.ToReadOnlySet(), null);
            typeof(DomainSnapshot).GetProperty("Domains").SetValue(instance3, domains.ToReadOnlySet(), null);
            typeof(DomainSnapshot).GetProperty("OrganizationalUnits").SetValue(instance3, containers.ToReadOnlySet(), null);

            yield return instance3;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestMethod1()
        {
            new DomainSnapshot(null);
        }

        private readonly HashSet<string> domainObjectNames = new HashSet<string>
        {
            #region Items
            @"env.tests.com",
            @"env.tests.com/Users/Administrator",
            @"env.tests.com/Users/Guest",
            @"env.tests.com/Users/krbtgt",
            @"env.tests.com/Users/Tester",
            @"env.tests.com/Users/ExternalBuilder",
            @"env.tests.com/Users/SUBDOMAIN$",
            @"subdomain.env.tests.com",
            //@"CREATOR OWNER",
            //@"CREATOR OWNER SERVER",
            //@"NT AUTHORITY\SELF",
            @"SYSTEM",
            @"LOCAL SERVICE",
            @"NETWORK SERVICE",
            //@"NT AUTHORITY\NTLM Authentication",
            //@"NT AUTHORITY\Digest Authentication",
            //@"NT AUTHORITY\SChannel Authentication",
            @"env.tests.com/Builtin/Administrators",
            @"env.tests.com/Builtin/Users",
            @"env.tests.com/Builtin/Guests",
            @"env.tests.com/Builtin/Print Operators",
            @"env.tests.com/Builtin/Backup Operators",
            @"env.tests.com/Builtin/Replicator",
            @"env.tests.com/Builtin/Remote Desktop Users",
            @"env.tests.com/Builtin/Network Configuration Operators",
            @"env.tests.com/Builtin/Performance Monitor Users",
            @"env.tests.com/Builtin/Performance Log Users",
            @"env.tests.com/Builtin/Distributed COM Users",
            @"env.tests.com/Builtin/IIS_IUSRS",
            @"env.tests.com/Builtin/Cryptographic Operators",
            @"env.tests.com/Builtin/Event Log Readers",
            @"env.tests.com/Builtin/Certificate Service DCOM Access",
            @"env.tests.com/Users/Domain Computers",
            @"env.tests.com/Users/Domain Controllers",
            @"env.tests.com/Users/Schema Admins",
            @"env.tests.com/Users/Enterprise Admins",
            @"env.tests.com/Users/Cert Publishers",
            @"env.tests.com/Users/Domain Admins",
            @"env.tests.com/Users/Domain Users",
            @"env.tests.com/Users/Domain Guests",
            @"env.tests.com/Users/Group Policy Creator Owners",
            @"env.tests.com/Users/RAS and IAS Servers",
            @"env.tests.com/Builtin/Server Operators",
            @"env.tests.com/Builtin/Account Operators",
            @"env.tests.com/Builtin/Pre-Windows 2000 Compatible Access",
            @"env.tests.com/Builtin/Incoming Forest Trust Builders",
            @"env.tests.com/Builtin/Windows Authorization Access Group",
           // @"env.tests.com/Builtin/Terminal Server LicenseInfo Servers",
            @"env.tests.com/Users/Allowed RODC Password Replication Group",
            @"env.tests.com/Users/Denied RODC Password Replication Group",
            @"env.tests.com/Users/Read-only Domain Controllers",
            @"env.tests.com/Users/Enterprise Read-only Domain Controllers",
            @"env.tests.com/Users/DnsAdmins",
            @"env.tests.com/Users/DnsUpdateProxy",
            @"env.tests.com/Users/Testers",
            @"env.tests.com/Users/TeamTestAgentService",
            @"env.tests.com/Users/TeamTestControllerUsers",
            @"env.tests.com/Users/TeamTestControllerAdmins",
            @"env.tests.com/Users/Test Group 1",
            @"env.tests.com/Users/Test Group 2",
            //@"NULL SID",
            //@"Everyone",
            //@"LOCAL",
            //@"CREATOR GROUP",
            //@"CREATOR GROUP SERVER",
            //@"NT AUTHORITY\DIALUP",
            //@"NT AUTHORITY\NETWORK",
            //@"NT AUTHORITY\BATCH",
            //@"NT AUTHORITY\INTERACTIVE",
            //@"NT AUTHORITY\SERVICE",
            //@"NT AUTHORITY\ANONYMOUS LOGON",
            //@"NT AUTHORITY\PROXY",
            //@"NT AUTHORITY\ENTERPRISE DOMAIN CONTROLLERS",
            //@"NT AUTHORITY\Authenticated Users",
            //@"NT AUTHORITY\RESTRICTED",
            //@"NT AUTHORITY\TERMINAL SERVER USER",
            //@"NT AUTHORITY\REMOTE INTERACTIVE LOGON",
            //@"BUILTIN\Power Users",
            //@"NT AUTHORITY\This Organization",
            //@"NT AUTHORITY\Other Organization",
            @"env.tests.com/Domain Controllers/WIN-45I9EEK2REL",
            @"env.tests.com/Computers/XP86EN",
            @"env.tests.com/Computers/2003SERVER64EN",
            @"env.tests.com/Computers/VISTA86EN",
            @"env.tests.com/Domain Controllers",

            @"env.tests.com/Users",
            @"env.tests.com/Computers",
            @"env.tests.com/ForeignSecurityPrincipals",

            @"ENV.TESTS.COM/BUILTIN/TERMINAL SERVER LICENSE SERVERS",
            //@"ENV.TESTS.COM/BUILTIN/TERMINAL SERVER LICENSEINFO SERVERS",

            #endregion
        };

        private readonly HashSet<string> objectNames_2003server64en = new HashSet<string>
        {
            #region Items

            @"2003SERVER64EN",
            @"Administrator",
            @"ASPNET",
            @"Guest",
            @"SUPPORT_388945a0",
            //@"CREATOR OWNER",
            //@"CREATOR OWNER SERVER",
            //@"NT AUTHORITY\SELF",
            @"SYSTEM",
            @"LOCAL SERVICE",
            @"NETWORK SERVICE",
            //@"NT AUTHORITY\NTLM Authentication",
            //@"NT AUTHORITY\Digest Authentication",
            //@"NT AUTHORITY\SChannel Authentication",
            @"Administrators",
            @"Backup Operators",
            @"Distributed COM Users",
            @"Guests",
            @"Network Configuration Operators",
            @"Performance Log Users",
            @"Performance Monitor Users",
            @"Power Users",
            @"Print Operators",
            @"Remote Desktop Users",
            @"Replicator",
            @"Users",
            @"HelpServicesGroup",
            @"TelnetClients",
            //@"NULL SID",
            //@"Everyone",
            //@"LOCAL",
            //@"CREATOR GROUP",
            //@"CREATOR GROUP SERVER",
            //@"NT AUTHORITY\DIALUP",
            //@"NT AUTHORITY\NETWORK",
            //@"NT AUTHORITY\BATCH",
            //@"NT AUTHORITY\INTERACTIVE",
            //@"NT AUTHORITY\SERVICE",
            //@"NT AUTHORITY\ANONYMOUS LOGON",
            //@"NT AUTHORITY\PROXY",
            //@"NT AUTHORITY\ENTERPRISE DOMAIN CONTROLLERS",
            //@"NT AUTHORITY\Authenticated Users",
            //@"NT AUTHORITY\RESTRICTED",
            //@"NT AUTHORITY\TERMINAL SERVER USER",
            //@"NT AUTHORITY\REMOTE INTERACTIVE LOGON",
            //@"NT AUTHORITY\This Organization",
            //@"NT AUTHORITY\Other Organization"

            #endregion
        };

        private readonly HashSet<string> objectNames_xp86en = new HashSet<string>
        {
            #region Items

            @"XP86EN",
            @"Admin",
            @"Administrator",
            @"ASPNET",
            @"Guest",
            @"HelpAssistant",
            @"SUPPORT_388945a0",
            //@"CREATOR OWNER",
            //@"CREATOR OWNER SERVER",
            //@"NT AUTHORITY\SELF",
            @"SYSTEM",
            @"LOCAL SERVICE",
            @"NETWORK SERVICE",
            //@"NT AUTHORITY\NTLM Authentication",
            //@"NT AUTHORITY\Digest Authentication",
            //@"NT AUTHORITY\SChannel Authentication",
            @"Administrators",
            @"Backup Operators",
            @"Guests",
            @"Network Configuration Operators",
            @"Power Users",
            @"Remote Desktop Users",
            @"Replicator",
            @"Users",
            @"HelpServicesGroup",
            //@"NULL SID",
            //@"Everyone",
            //@"LOCAL",
            //@"CREATOR GROUP",
            //@"CREATOR GROUP SERVER",
            //@"NT AUTHORITY\DIALUP",
            //@"NT AUTHORITY\NETWORK",
            //@"NT AUTHORITY\BATCH",
            //@"NT AUTHORITY\INTERACTIVE",
            //@"NT AUTHORITY\SERVICE",
            //@"NT AUTHORITY\ANONYMOUS LOGON",
            //@"NT AUTHORITY\PROXY",
            //@"NT AUTHORITY\ENTERPRISE DOMAIN CONTROLLERS",
            //@"NT AUTHORITY\Authenticated Users",
            //@"NT AUTHORITY\RESTRICTED",
            //@"NT AUTHORITY\TERMINAL SERVER USER",
            //@"NT AUTHORITY\REMOTE INTERACTIVE LOGON",
            //@"BUILTIN\Print Operators",
            //@"NT AUTHORITY\This Organization",
            //@"NT AUTHORITY\Other Organization",
            //@"BUILTIN\Performance Monitor Users",
            //@"BUILTIN\Performance Log Users"

            #endregion
        };

        private readonly HashSet<string> objectNames_WIN_45I9EEK2REL = new HashSet<string>
        {
            #region Items

            "WIN-45I9EEK2REL"

            #endregion
        };

        private readonly HashSet<string> objectNames_8x64 = new HashSet<string>
        {
            #region Items

            @"8x64",
            @"Administrator",
            @"Guest",
            @"HomeGroupUser$",
            @"Tester",
            //@"CREATOR OWNER",
            //@"CREATOR OWNER SERVER",
            //@"NT AUTHORITY\SELF",
            @"SYSTEM",
            @"LOCAL SERVICE",
            @"NETWORK SERVICE",
            //@"NT AUTHORITY\NTLM Authentication",
            //@"NT AUTHORITY\Digest Authentication",
            //@"NT AUTHORITY\SChannel Authentication",
            @"Access Control Assistance Operators",
            @"Administrators",
            @"Backup Operators",
            @"Cryptographic Operators",
            @"Distributed COM Users",
            @"Event Log Readers",
            @"Guests",
            @"Hyper-V Administrators",
            @"IIS_IUSRS",
            @"Network Configuration Operators",
            @"Performance Log Users",
            @"Performance Monitor Users",
            @"Power Users",
            @"Remote Desktop Users",
            @"Remote Management Users",
            @"Replicator",
            @"Users",
            @"HomeUsers",
            @"WinRMRemoteWMIUsers__",
            //@"NULL SID",
            //@"Everyone",
            //@"LOCAL",
            //@"CREATOR GROUP",
            //@"CREATOR GROUP SERVER",
            //@"NT AUTHORITY\DIALUP",
            //@"NT AUTHORITY\NETWORK",
            //@"NT AUTHORITY\BATCH",
            //@"NT AUTHORITY\INTERACTIVE",
            //@"NT AUTHORITY\SERVICE",
            //@"NT AUTHORITY\ANONYMOUS LOGON",
            //@"NT AUTHORITY\PROXY",
            //@"NT AUTHORITY\ENTERPRISE DOMAIN CONTROLLERS",
            //@"NT AUTHORITY\Authenticated Users",
            //@"NT AUTHORITY\RESTRICTED",
            //@"NT AUTHORITY\TERMINAL SERVER USER",
            //@"NT AUTHORITY\REMOTE INTERACTIVE LOGON",
            //@"BUILTIN\Print Operators",
            //@"NT AUTHORITY\This Organization",
            //@"NT AUTHORITY\Other Organization",

            #endregion
        };

        private readonly HashSet<string> subdomainObjectNames = new HashSet<string>
        {
            #region Items

            @"subdomain.env.tests.com",
            @"subdomain.env.tests.com/Users/Administrator",
            @"subdomain.env.tests.com/Users/Guest",
            @"subdomain.env.tests.com/Users/SUPPORT_388945a0",
            @"subdomain.env.tests.com/Users/krbtgt",
            @"subdomain.env.tests.com/Users/TESTENV$",
            //@"CREATOR OWNER",
            //@"CREATOR OWNER SERVER",
            //@"NT AUTHORITY\SELF",
            @"SYSTEM",
            @"LOCAL SERVICE",
            @"NETWORK SERVICE",
            //@"NT AUTHORITY\NTLM Authentication",
            //@"NT AUTHORITY\Digest Authentication",
            //@"NT AUTHORITY\SChannel Authentication",
            @"subdomain.env.tests.com/Users/HelpServicesGroup",
            @"subdomain.env.tests.com/Users/TelnetClients",
            @"subdomain.env.tests.com/Builtin/Administrators",
            @"subdomain.env.tests.com/Builtin/Users",
            @"subdomain.env.tests.com/Builtin/Guests",
            @"subdomain.env.tests.com/Builtin/Print Operators",
            @"subdomain.env.tests.com/Builtin/Backup Operators",
            @"subdomain.env.tests.com/Builtin/Replicator",
            @"subdomain.env.tests.com/Builtin/Remote Desktop Users",
            @"subdomain.env.tests.com/Builtin/Network Configuration Operators",
            @"subdomain.env.tests.com/Builtin/Performance Monitor Users",
            @"subdomain.env.tests.com/Builtin/Performance Log Users",
            @"subdomain.env.tests.com/Builtin/Distributed COM Users",
            @"subdomain.env.tests.com/Users/Domain Computers",
            @"subdomain.env.tests.com/Users/Domain Controllers",
            @"subdomain.env.tests.com/Users/Cert Publishers",
            @"subdomain.env.tests.com/Users/Domain Admins",
            @"subdomain.env.tests.com/Users/Domain Users",
            @"subdomain.env.tests.com/Users/Domain Guests",
            @"subdomain.env.tests.com/Users/Group Policy Creator Owners",
            @"subdomain.env.tests.com/Users/RAS and IAS Servers",
            @"subdomain.env.tests.com/Builtin/Server Operators",
            @"subdomain.env.tests.com/Builtin/Account Operators",
            @"subdomain.env.tests.com/Builtin/Pre-Windows 2000 Compatible Access",
            @"subdomain.env.tests.com/Builtin/Windows Authorization Access Group",
            //@"subdomain.env.tests.com/Builtin/Terminal Server LicenseInfo Servers",
            //@"NULL SID",
            //@"Everyone",
            //@"LOCAL",
            //@"CREATOR GROUP",
            //@"CREATOR GROUP SERVER",
            //@"NT AUTHORITY\DIALUP",
            //@"NT AUTHORITY\NETWORK",
            //@"NT AUTHORITY\BATCH",
            //@"NT AUTHORITY\INTERACTIVE",
            //@"NT AUTHORITY\SERVICE",
            //@"NT AUTHORITY\ANONYMOUS LOGON",
            //@"NT AUTHORITY\PROXY",
            //@"NT AUTHORITY\ENTERPRISE DOMAIN CONTROLLERS",
            //@"NT AUTHORITY\Authenticated Users",
            //@"NT AUTHORITY\RESTRICTED",
            //@"NT AUTHORITY\TERMINAL SERVER USER",
            //@"NT AUTHORITY\REMOTE INTERACTIVE LOGON",
            //@"BUILTIN\Power Users",
            //@"NT AUTHORITY\This Organization",
            //@"NT AUTHORITY\Other Organization",
            @"subdomain.env.tests.com/Domain Controllers/2003SERVER86EN",
            @"subdomain.env.tests.com/Domain Controllers",
            @"env.tests.com",
            
            @"subdomain.env.tests.com/Users",
            @"subdomain.env.tests.com/Computers",
            @"subdomain.env.tests.com/ForeignSecurityPrincipals",

            
            @"SUBDOMAIN.ENV.TESTS.COM/BUILTIN/TERMINAL SERVER LICENSE SERVERS",
            //@"SUBDOMAIN.ENV.TESTS.COM/BUILTIN/TERMINAL SERVER LICENSEINFO SERVERS",

            #endregion
        };

        private readonly HashSet<string> objectNames_2003server86en = new HashSet<string>
        {
            #region Items

            @"2003server86en",
            //@"subdomain.env.tests.com/Domain Controllers/WIN-45I9EEK2REL"
              //SUBDOMAIN.ENV.TESTS.COM/DOMAIN CONTROLLERS/WIN-45I9EEK2REL

            #endregion
        };

        [TestMethod]
        public void TestSubdomainSnapshot1()
        {
            foreach (string domainName in new[] { @"subdomain.env.tests.com", @"subdomain" })
            {
                foreach (string userName in new[] { @"subdomain\Administrator", @"Administrator" })
                {
                    TestDomainSnapshot(
                        subdomainObjectNames,
                        domainName,
                        userName);
                }
            }
        }

        [TestMethod]
        public void TestDomainSnapshot1()
        {
            foreach (string domainName in new[] { @"env.tests.com", @"testenv" })
            {
                foreach (string userName in new[] { @"testenv\Administrator", @"Administrator" })
                {
                    TestDomainSnapshot(
                        domainObjectNames,
                        domainName,
                        userName);
                }
            }
        }

        [TestMethod]
        public void TestComputerSnapshot1_2003server86en()
        {
            foreach (string domainName in new[] { @"2003server86en.subdomain.env.tests.com", @"2003server86en" })
            {
                foreach (string userName in new[] { @"subdomain\Administrator", @"Administrator" })
                {
                    TestComputerSnapshot(
                        objectNames_2003server86en,
                        domainName,
                        userName);
                }
            }
        }

        [TestMethod]
        public void TestComputerSnapshot1_2003server64en()
        {
            foreach (string domainName in new[] { @"2003server64en.env.tests.com", @"2003server64en" })
            {
                foreach (string userName in new[] { @"testenv\Administrator", @"Administrator" })
                {
                    TestComputerSnapshot(
                        objectNames_2003server64en,
                        domainName,
                        userName);
                }
            }
        }

        [TestMethod]
        public void TestComputerSnapshot1_xp86en()
        {
            var names = new[] { @"xp86en.env.tests.com", @"xp86en" };

            foreach (string domainName in names)
            {
                foreach (string userName in new[] {@"testenv\Administrator", @"Administrator"})
                {
                    TestComputerSnapshot(
                        objectNames_xp86en,
                        domainName,
                        userName);
                }
            }
        }

        [TestMethod]
        public void TestComputerSnapshot1_WIN_45I9EEK2REL()
        {
            var names = new[] { @"WIN-45I9EEK2REL", @"WIN-45I9EEK2REL.env.tests.com" };

            foreach (string domainName in names)
            {
                foreach (string userName in new[] { @"testenv\Administrator", @"Administrator" })
                {
                    TestComputerSnapshot(
                        objectNames_WIN_45I9EEK2REL,
                        domainName,
                        userName);
                }
            }
        }

        [TestMethod]
        public void TestComputerSnapshot1_8x64()
        {
            foreach (string userName in new[] { @"8x64\Administrator", @"Administrator" })
            {
                TestComputerSnapshot(
                    objectNames_8x64,
                    @"8x64",
                    userName);
            }
        }

        private static void TestComputerSnapshot(IEnumerable<string> objectNames, string domainName, string loginName)
        {
            var credentials = new Credentials(new LocalComputerAccount(domainName, new AccountSecurityIdentifier("S-1-5-0")), loginName, @"Qwerty12");

            CheckComputerSnapshot(credentials, objectNames);
        }

        private static void TestDomainSnapshot(IEnumerable<string> objectNames, string domainName, string loginName)
        {
            var credentials = new Credentials(new DomainAccount(domainName, new AccountSecurityIdentifier("S-1-5-0"), domainName), loginName, @"Qwerty12");

            CheckDomainSnapshot(credentials, objectNames);
        }

        private static void CheckDomainSnapshot(Credentials credentials, IEnumerable<string> expected)
        {
            if (!string.Equals(Environment.MachineName, "tfs", StringComparison.OrdinalIgnoreCase))
                return;

            var snapshot = new DomainSnapshot(credentials);
            
            var names = new HashSet<string>();
            names.UnionWith(snapshot.Computers.Select(item => item.CanonicalName));
            names.UnionWith(snapshot.Domains.Select(item => item.CanonicalName));
            names.UnionWith(snapshot.Groups.Select(item => item.CanonicalName));
            names.UnionWith(snapshot.OrganizationalUnits.Select(item => item.CanonicalName));
            names.UnionWith(snapshot.Users.Select(item => item.CanonicalName));

            var unknownItems = new HashSet<string>(names.Select(item => item.ToUpperInvariant()));
            unknownItems.SymmetricExceptWith(expected.Select(item => item.ToUpperInvariant()));

            string unknownItemsString = string.Join(", ", unknownItems);
            string searchResultError = "Incorrect search result. There are additional objects in the domain {0} : {1}"
                .Combine(credentials.Domain, unknownItemsString);

            Assert.IsTrue(!unknownItems.Any(), searchResultError);
        }

        private static void CheckComputerSnapshot(Credentials credentials, IEnumerable<string> expected)
        {
            if (!string.Equals(Environment.MachineName, "tfs", StringComparison.OrdinalIgnoreCase))
                return;

            var snapshot = new DomainSnapshot(credentials);

            HashSet<string> names = new HashSet<string>();
            names.UnionWith(snapshot.Computers.Select(item => item.ShortName));
            names.UnionWith(snapshot.Domains.Select(item => item.ShortName));
            names.UnionWith(snapshot.Groups.Select(item => item.ShortName));
            names.UnionWith(snapshot.OrganizationalUnits.Select(item => item.ShortName));
            names.UnionWith(snapshot.Users.Select(item => item.ShortName));

            var unknownItems = new HashSet<string>(names.Select(item => item.ToUpperInvariant()));
            unknownItems.SymmetricExceptWith(expected.Select(item => item.ToUpperInvariant()));

            string unknownItemsString = string.Join(", ", unknownItems);
            string searchResultError = "Incorrect search result. There are additional objects in the domain {0} : {1}"
                .Combine(credentials.Domain, unknownItemsString);

            Assert.IsTrue(!unknownItems.Any(), searchResultError);
        }
    }
}
