using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Security.Principal;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.Libraries.SystemTools.Entities.Management
{
    internal static class DirectoryServicesProvider
    {
        private static readonly string localSystemName = NameConversion.SimplifyUserName(new SecurityIdentifier("S-1-5-18").Translate(typeof(NTAccount)).Value);
        private static readonly AccountSecurityIdentifier localSystemIdentifier = new AccountSecurityIdentifier("S-1-5-18");

        private static readonly string localServiceName = NameConversion.SimplifyUserName(new SecurityIdentifier("S-1-5-19").Translate(typeof(NTAccount)).Value);
        private static readonly AccountSecurityIdentifier localServiceIdentifier = new AccountSecurityIdentifier("S-1-5-19");

        private static readonly string networkServiceName = NameConversion.SimplifyUserName(new SecurityIdentifier("S-1-5-20").Translate(typeof(NTAccount)).Value);
        private static readonly AccountSecurityIdentifier networkServiceIdentifier = new AccountSecurityIdentifier("S-1-5-20");
        
        internal static DomainAccount GetDomainByName(string name = null, SystemAccessOptions options = default(SystemAccessOptions))
        {
            if (name == null)
            {
                using (Domain currentDomain = Domain.GetComputerDomain())
                {
                    name = currentDomain.Name;
                }
            }

            using (var root = new DirectoryEntry("LDAP://" + name, options.UserName, options.Password))
            {
                return InitializeDomainAccount(root);
            }
        }

        internal static ReadOnlySet<DomainAccount> FindDomains(DomainAccount baseDomain, SystemAccessOptions options = default(SystemAccessOptions))
        {
            Check.ObjectIsNotNull(baseDomain, "baseDomain");

            var domainEntries = new HashSet<DomainAccount>();
            
            using (Domain domain = Domain.GetDomain(new DirectoryContext(DirectoryContextType.Domain, baseDomain.FullName, options.UserName, options.Password)))
            using (Forest forest = Forest.GetForest(new DirectoryContext(DirectoryContextType.Forest, domain.Forest.Name, options.UserName, options.Password)))
            {
                foreach (Domain forestDomain in forest.Domains)
                {
                    try
                    {
                        using (forestDomain)
                        using (DirectoryEntry domainEntry = forestDomain.GetDirectoryEntry())
                        {
                            DomainAccount domainAccount = InitializeDomainAccount(domainEntry);
                            domainEntries.Add(domainAccount);
                        }
                    }
                    catch (Exception exception)
                    {
                        Log.Write(exception);
                    }
                }
            }

            return domainEntries.ToReadOnlySet();
        }

        private static DomainAccount InitializeDomainAccount(DirectoryEntry directoryEntry)
        {
            var securityIdentifier = (byte[])directoryEntry.Properties["objectSid"].Value;
            
            var distinguishedName = (string)directoryEntry.Properties["distinguishedName"].Value;
            string canonicalName = NameConversion.ConvertToCanonicalName(distinguishedName).TrimEnd('/');

            return new DomainAccount(canonicalName, new AccountSecurityIdentifier(securityIdentifier), canonicalName);
        }

        internal static ReadOnlyDictionary<Account, AccountSecurityIdentifierSet> FindLocalEntriesByIdentity(BaseComputerAccount computer, SystemAccessOptions options = default(SystemAccessOptions))
        {
            Check.ObjectIsNotNull(computer, "computer");

            var localComputer = computer as LocalComputerAccount;
            var domainComputer = computer as DomainComputerAccount;

            if(!((localComputer == null) ^ (domainComputer == null)))
                Exceptions.Throw(ErrorMessage.UnsupportedType, "Unknown computer type {0}.".Combine(computer.GetType()));

            var result = new Dictionary<Account, AccountSecurityIdentifierSet>();

            KeyValuePair<BaseComputerAccount, AccountSecurityIdentifierSet> targetComputer = GetComputerByName(computer.FullName, options);
            result.Add(targetComputer.Key, targetComputer.Value);

            if (ComputerInformation.IsDomainController(computer.FullName, options))
                return result.ToReadOnlyDictionary();

            using (var root = new DirectoryEntry("WinNT://" + computer.FullName + ",Computer", options.UserName, options.Password))
            {
                foreach (DirectoryEntry currentEntry in root.Children)
                {
                    using (currentEntry)
                    {
                        bool user = currentEntry.SchemaClassName.Equals("User", StringComparison.OrdinalIgnoreCase);
                        bool group = currentEntry.SchemaClassName.Equals("Group", StringComparison.OrdinalIgnoreCase);

                        if (!(user ^ group))
                            continue;

                        var securityIdentifier = (byte[])currentEntry.Properties["objectSid"].Value;
                        var name = (string)currentEntry.Properties["name"].Value; // or fullName (but in can be null) ?

                        var identifier = new AccountSecurityIdentifier(securityIdentifier);
                        name = NameConversion.SimplifyUserName(name);

                        var groups = new HashSet<AccountSecurityIdentifier>();
                        if (user)
                        {
                            var groupList = (IEnumerable)currentEntry.Invoke("groups", null);
                            foreach (object groupObject in groupList)
                            {
                                using (var groupEntry = new DirectoryEntry(groupObject))
                                {
                                    groups.Add(new AccountSecurityIdentifier((byte[])groupEntry.Properties["objectSid"].Value));
                                }
                            }
                        }

                        if (user && (localComputer != null))
                            result.Add(new LocalUserAccount(name, identifier, localComputer), new AccountSecurityIdentifierSet(groups.ToReadOnlySet()));

                        if (user && (domainComputer != null))
                            result.Add(new DomainComputerUserAccount(name, identifier, domainComputer), new AccountSecurityIdentifierSet(groups.ToReadOnlySet()));

                        if (group && (localComputer != null))
                            result.Add(new LocalGroupAccount(name, identifier, localComputer), new AccountSecurityIdentifierSet(groups.ToReadOnlySet()));

                        if (group && (domainComputer != null))
                            result.Add(new DomainComputerGroupAccount(name, identifier, domainComputer), new AccountSecurityIdentifierSet(groups.ToReadOnlySet()));
                    }
                }

                if (localComputer != null)
                {
                    result.Add(new LocalUserAccount(localSystemName, localSystemIdentifier, localComputer), new AccountSecurityIdentifierSet(ReadOnlySet<AccountSecurityIdentifier>.Empty));
                    result.Add(new LocalUserAccount(localServiceName, localServiceIdentifier, localComputer), new AccountSecurityIdentifierSet(ReadOnlySet<AccountSecurityIdentifier>.Empty));
                    result.Add(new LocalUserAccount(networkServiceName, networkServiceIdentifier, localComputer), new AccountSecurityIdentifierSet(ReadOnlySet<AccountSecurityIdentifier>.Empty));
                }
                if (domainComputer != null)
                {
                    result.Add(new DomainComputerUserAccount(localSystemName, localSystemIdentifier, domainComputer), new AccountSecurityIdentifierSet(ReadOnlySet<AccountSecurityIdentifier>.Empty));
                    result.Add(new DomainComputerUserAccount(localServiceName, localServiceIdentifier, domainComputer), new AccountSecurityIdentifierSet(ReadOnlySet<AccountSecurityIdentifier>.Empty));
                    result.Add(new DomainComputerUserAccount(networkServiceName, networkServiceIdentifier, domainComputer), new AccountSecurityIdentifierSet(ReadOnlySet<AccountSecurityIdentifier>.Empty));
                }
            }

            return result.ToReadOnlyDictionary();
        }
        
        // Domain computer requires network connection, local does not.
        internal static KeyValuePair<BaseComputerAccount, AccountSecurityIdentifierSet> GetComputerByName(string name = null, SystemAccessOptions options = default(SystemAccessOptions))
        {
            name = name ?? ComputerInformation.GetCurrentComputerName();

            string computerShortName = ComputerInformation.GetComputerShortName(name, options);
            string domainName = ComputerInformation.GetComputerDomainName(name, options);
            if (domainName != null)
            {
                string domainShortName = ComputerInformation.GetComputerShortDomainName(name, options);
                string domainIdentifier = ComputerInformation.GetComputerParentDomainIdentifier(name, options);
                var domain = new DomainAccount(domainShortName, new AccountSecurityIdentifier(domainIdentifier), domainName);

                string fullName = computerShortName + "." + domainName;

                IEnumerable<KeyValuePair<IDomainMember, AccountSecurityIdentifierSet>> entries = FindDomainEntriesByIdentity(domain, options);
                IEnumerable<KeyValuePair<IDomainMember, AccountSecurityIdentifierSet>> computerEntries = entries.Where(item =>
                    item.Key.FullName.Equals(fullName, StringComparison.OrdinalIgnoreCase)).Where(item => item.Key is BaseComputerAccount);

                if (entries.Any())
                {
                    KeyValuePair<IDomainMember, AccountSecurityIdentifierSet> entry = computerEntries.First();
                    return new KeyValuePair<BaseComputerAccount, AccountSecurityIdentifierSet>((BaseComputerAccount)entry.Key, entry.Value);
                }
            }

            string computerIdentifier = ComputerInformation.GetComputerIdentifier(name, options);

            return new KeyValuePair<BaseComputerAccount, AccountSecurityIdentifierSet>(
                new LocalComputerAccount(computerShortName, new AccountSecurityIdentifier(computerIdentifier)),
                new AccountSecurityIdentifierSet(ReadOnlySet<AccountSecurityIdentifier>.Empty));
        }

        internal static ReadOnlyDictionary<IDomainMember, AccountSecurityIdentifierSet> FindDomainEntriesByIdentity(DomainAccount domain, SystemAccessOptions options = default(SystemAccessOptions))
        {
            Check.ObjectIsNotNull(domain, "domain");

            const string filter = "(|(objectCategory=computer)(objectCategory=group)(objectCategory=user)(objectCategory=organizationalUnit)(objectCategory=container))";

            var result = new Dictionary<IDomainMember, AccountSecurityIdentifierSet>();
            var hiddenContainers = new HashSet<OrganizationalUnit>();

            using (var root = new DirectoryEntry("LDAP://" + domain.FullName, options.UserName, options.Password))
            using (var searcher = new DirectorySearcher(root, filter))
            {
                searcher.PageSize = 1000;

                using (SearchResultCollection searchResult = searcher.FindAll())
                {
                    foreach (SearchResult currentResult in searchResult)
                    {
                        using (DirectoryEntry currentEntry = currentResult.GetDirectoryEntry())
                        {
                            string name = (string)currentEntry.Properties["name"].Value; // user: "userPrincipalName" computer: "dNSHostName" user/computer/group: "sAMAccountName" 
                            string distinguishedName = (string)currentEntry.Properties["distinguishedName"].Value;
                            string canonicalName = NameConversion.ConvertToCanonicalName(distinguishedName);
                            name = NameConversion.SimplifyUserName(name);

                            if (currentEntry.SchemaClassName.Equals("computer", StringComparison.OrdinalIgnoreCase))
                            {
                                var securityIdentifier = (byte[])currentEntry.Properties["objectSid"].Value;
                                var identifier = new AccountSecurityIdentifier(securityIdentifier);

                                var groups = new HashSet<AccountSecurityIdentifier>();
                                currentEntry.RefreshCache(new[] { "tokenGroups" });
                                foreach (byte[] bytes in currentEntry.Properties["tokenGroups"])
                                    groups.Add(new AccountSecurityIdentifier(bytes));

                                result.Add(new DomainComputerAccount(name, identifier, domain, canonicalName), new AccountSecurityIdentifierSet(groups.ToReadOnlySet()));
                            }

                            if (currentEntry.SchemaClassName.Equals("group", StringComparison.OrdinalIgnoreCase))
                            {
                                var securityIdentifier = (byte[])currentEntry.Properties["objectSid"].Value;
                                var identifier = new AccountSecurityIdentifier(securityIdentifier);

                                var groups = new HashSet<AccountSecurityIdentifier>();
                                currentEntry.RefreshCache(new[] { "tokenGroups" });
                                foreach (byte[] bytes in currentEntry.Properties["tokenGroups"])
                                    groups.Add(new AccountSecurityIdentifier(bytes));

                                result.Add(new DomainGroupAccount(name, identifier, domain, canonicalName), new AccountSecurityIdentifierSet(groups.ToReadOnlySet()));
                            }

                            if (currentEntry.SchemaClassName.Equals("user", StringComparison.OrdinalIgnoreCase))
                            {
                                var securityIdentifier = (byte[])currentEntry.Properties["objectSid"].Value;
                                var identifier = new AccountSecurityIdentifier(securityIdentifier);

                                var groups = new HashSet<AccountSecurityIdentifier>();
                                currentEntry.RefreshCache(new[] { "tokenGroups" });
                                foreach (byte[] bytes in currentEntry.Properties["tokenGroups"])
                                    groups.Add(new AccountSecurityIdentifier(bytes));

                                result.Add(new DomainUserAccount(name, identifier, domain, canonicalName), new AccountSecurityIdentifierSet(groups.ToReadOnlySet()));
                            }

                            if (currentEntry.SchemaClassName.Equals("organizationalUnit", StringComparison.OrdinalIgnoreCase) ||
                                currentEntry.SchemaClassName.Equals("container", StringComparison.OrdinalIgnoreCase))
                            {
                                var organizationalUnit = new OrganizationalUnit(name, canonicalName, domain);
                                result.Add(organizationalUnit, new AccountSecurityIdentifierSet(ReadOnlySet<AccountSecurityIdentifier>.Empty));

                                object hiddenAttribute = currentEntry.Properties["showInAdvancedViewOnly"].Value;
                                if ((hiddenAttribute is bool) && (bool)hiddenAttribute)
                                    hiddenContainers.Add(organizationalUnit);
                            }
                        }
                    }
                }

                result.Add(new DomainUserAccount(localSystemName, localSystemIdentifier, domain, localSystemName), new AccountSecurityIdentifierSet(ReadOnlySet<AccountSecurityIdentifier>.Empty));
                result.Add(new DomainUserAccount(localServiceName, localServiceIdentifier, domain, localServiceName), new AccountSecurityIdentifierSet(ReadOnlySet<AccountSecurityIdentifier>.Empty));
                result.Add(new DomainUserAccount(networkServiceName, networkServiceIdentifier, domain, networkServiceName), new AccountSecurityIdentifierSet(ReadOnlySet<AccountSecurityIdentifier>.Empty));
            }

            IEnumerable<IDomainMember> itemsToCheck = result.Keys.Without(hiddenContainers);
            IEnumerable<OrganizationalUnit> emptyHiddenItems = hiddenContainers.Where(organizationalUnit =>
                !itemsToCheck.Any(item => item.CanonicalName.StartsWith(organizationalUnit.CanonicalName, StringComparison.OrdinalIgnoreCase)));

            foreach (OrganizationalUnit currentItem in emptyHiddenItems)
                result.Remove(currentItem);

            return result.ToReadOnlyDictionary();
        }
    }
}
