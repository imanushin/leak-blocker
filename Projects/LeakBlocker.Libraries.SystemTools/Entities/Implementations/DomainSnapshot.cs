using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.SystemTools.Entities.Management;

namespace LeakBlocker.Libraries.SystemTools.Entities.Implementations
{
    [Serializable]
    [DataContract(IsReference = true)]
    internal sealed class DomainSnapshot : BaseReadOnlyObject, IDomainSnapshot
    {
        [DataMember]
        public ReadOnlyDictionary<AccountSecurityIdentifier, AccountSecurityIdentifierSet> GroupMembership
        {
            get;
            private set;
        }

        [DataMember]
        public ReadOnlySet<BaseUserAccount> Users
        {
            get;
            private set;
        }

        [DataMember]
        public ReadOnlySet<BaseGroupAccount> Groups
        {
            get;
            private set;
        }

        [DataMember]
        public ReadOnlySet<BaseComputerAccount> Computers
        {
            get;
            private set;
        }

        [DataMember]
        public ReadOnlySet<DomainAccount> Domains
        {
            get;
            private set;
        }

        [DataMember]
        public ReadOnlySet<OrganizationalUnit> OrganizationalUnits
        {
            get;
            private set;
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return GroupMembership;
            yield return Users;
            yield return Groups;
            yield return Computers;
            yield return Domains;
            yield return OrganizationalUnits;
        }

        internal DomainSnapshot()
        {
            GroupMembership = ReadOnlyDictionary<AccountSecurityIdentifier, AccountSecurityIdentifierSet>.Empty;
            Users = ReadOnlySet<BaseUserAccount>.Empty;
            Groups = ReadOnlySet<BaseGroupAccount>.Empty;
            Computers = ReadOnlySet<BaseComputerAccount>.Empty;
            Domains = ReadOnlySet<DomainAccount>.Empty;
            OrganizationalUnits = ReadOnlySet<OrganizationalUnit>.Empty;
        }

        internal DomainSnapshot(Credentials credentials)
        {
            Check.ObjectIsNotNull(credentials, "credentials");

            using (new ExceptionNotifier("Collecting domain snapshot. {0}.", credentials))
            {
                var domain = credentials.Domain as DomainAccount;
                var computer = credentials.Domain as BaseComputerAccount;
                if (!((domain == null) ^ (computer == null)))
                    Exceptions.Throw(ErrorMessage.UnsupportedType, "Domain type {0} is not supported.".Combine(credentials.Domain.GetType()));

                var users = new HashSet<BaseUserAccount>();
                var groups = new HashSet<BaseGroupAccount>();
                var computers = new HashSet<BaseComputerAccount>();
                var organizationalUnits = new HashSet<OrganizationalUnit>();
                var domains = new HashSet<DomainAccount>();
                var objectGroups = new Dictionary<AccountSecurityIdentifier, AccountSecurityIdentifierSet>();

                if (domain != null)
                {
                    ReadOnlyDictionary<IDomainMember, AccountSecurityIdentifierSet> domainMembers = DirectoryServicesProvider.FindDomainEntriesByIdentity(domain, credentials);
                    ReadOnlySet<DomainAccount> availableDomains = DirectoryServicesProvider.FindDomains(domain, credentials);
                    
                    users.UnionWith(domainMembers.Keys.OfType<BaseUserAccount>());
                    groups.UnionWith(domainMembers.Keys.OfType<BaseGroupAccount>());
                    computers.UnionWith(domainMembers.Keys.OfType<BaseComputerAccount>());

                    organizationalUnits.UnionWith(domainMembers.Keys.OfType<OrganizationalUnit>());
                    domains.UnionWith(availableDomains);

                    foreach (KeyValuePair<IDomainMember, AccountSecurityIdentifierSet> currentItem in domainMembers)
                    {
                        var account = currentItem.Key as Account;
                        if (account != null)
                            objectGroups[account.Identifier] = currentItem.Value;
                    }
                }

                if (computer != null)
                {
                    ReadOnlyDictionary<Account, AccountSecurityIdentifierSet> localMembers = DirectoryServicesProvider.FindLocalEntriesByIdentity(computer, credentials);
                    
                    users.UnionWith(localMembers.Keys.OfType<BaseUserAccount>());
                    groups.UnionWith(localMembers.Keys.OfType<BaseGroupAccount>());
                    computers.UnionWith(localMembers.Keys.OfType<BaseComputerAccount>());

                    foreach (KeyValuePair<Account, AccountSecurityIdentifierSet> currentItem in localMembers)
                    {
                        var account = currentItem.Key;
                        if (account != null)
                            objectGroups[account.Identifier] = currentItem.Value;
                    }
                }

                var groupMembership = new Dictionary<AccountSecurityIdentifier, AccountSecurityIdentifierSet>();
                foreach (BaseGroupAccount group in groups)
                {
                    var members = objectGroups.Where(item => item.Value.Identifiers.Contains(group.Identifier)).Select(item => item.Key).ToReadOnlySet();
                    groupMembership[group.Identifier] = new AccountSecurityIdentifierSet(members.ToReadOnlySet());
                }

                Users = users.ToReadOnlySet();
                Groups = groups.ToReadOnlySet();
                Computers = computers.ToReadOnlySet();
                OrganizationalUnits = organizationalUnits.ToReadOnlySet();
                Domains = domains.ToReadOnlySet();
                GroupMembership = groupMembership.ToReadOnlyDictionary();
            }
        }

        private DomainSnapshot(IDomainSnapshot first, IDomainSnapshot second)
        {
            var objectGroups = new Dictionary<AccountSecurityIdentifier, AccountSecurityIdentifierSet>(first.GroupMembership);
            foreach(KeyValuePair<AccountSecurityIdentifier, AccountSecurityIdentifierSet> currentItem in second.GroupMembership)
            {
                if (!objectGroups.ContainsKey(currentItem.Key))
                    objectGroups[currentItem.Key] = currentItem.Value;
                else
                    objectGroups[currentItem.Key] = new AccountSecurityIdentifierSet(objectGroups[currentItem.Key].Identifiers.Union(currentItem.Value.Identifiers).ToReadOnlySet());
            }

            Users = first.Users.Union(second.Users).ToReadOnlySet();
            Groups = first.Groups.Union(second.Groups).ToReadOnlySet();
            Computers = first.Computers.Union(second.Computers).ToReadOnlySet();
            OrganizationalUnits = first.OrganizationalUnits.Union(second.OrganizationalUnits).ToReadOnlySet();
            Domains = first.Domains.Union(second.Domains).ToReadOnlySet();
            GroupMembership = objectGroups.ToReadOnlyDictionary();
        }

        public ReadOnlySet<IGroupMember> GetObjectsInGroup(BaseGroupAccount group)
        {
            Check.ObjectIsNotNull(group, "group");

            var entities = new HashSet<IGroupMember>();
            entities.UnionWith(Users);
            entities.UnionWith(Groups);
            entities.UnionWith(Computers.OfType<IGroupMember>());

            AccountSecurityIdentifierSet members = GroupMembership.TryGetValue(group.Identifier);
            return (members == null) ? ReadOnlySet<IGroupMember>.Empty : entities.Where(entity => members.Identifiers.Contains(entity.Identifier)).ToReadOnlySet();
        }

        public ReadOnlySet<IDomainMember> GetObjectsInDomain(DomainAccount domain)
        {
            Check.ObjectIsNotNull(domain, "domain");

            return GetObjectsInDomain((BaseDomainAccount)domain).OfType<IDomainMember>().ToReadOnlySet();
        }

        public ReadOnlySet<IBaseDomainMember> GetObjectsInDomain(BaseDomainAccount domain)
        {
            Check.ObjectIsNotNull(domain, "domain");

            var entities = new HashSet<IBaseDomainMember>();
            entities.UnionWith(Users);
            entities.UnionWith(Groups);
            entities.UnionWith(Computers.OfType<IBaseDomainMember>());
            entities.UnionWith(OrganizationalUnits);

            return entities.Where(entity => entity.Parent.Identifier.Equals(domain.Identifier)).ToReadOnlySet();
        }

        public ReadOnlySet<IDomainMember> GetObjectsInOrganizationalUnit(OrganizationalUnit organizationalUnit)
        {
            Check.ObjectIsNotNull(organizationalUnit, "organizationalUnit");

            ReadOnlySet<IDomainMember> domainEntities = GetObjectsInDomain(organizationalUnit.Parent);

            Func<IDomainMember, bool> isInOrganizationalUnit = entry =>
                entry.CanonicalName.StartsWith(organizationalUnit.CanonicalName, StringComparison.OrdinalIgnoreCase);

            return domainEntities.Where(isInOrganizationalUnit).ToReadOnlySet();
        }

        public IDomainSnapshot Merge(IDomainSnapshot snapshot)
        {
            Check.ObjectIsNotNull(snapshot, "snapshot");

            return new DomainSnapshot(this, snapshot);
        }
    }
}
              