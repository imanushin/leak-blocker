using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.SystemTools.Entities.Management;

namespace LeakBlocker.Libraries.SystemTools.Entities
{
    /// <summary>
    /// Container for objects that were collected from one or more domains.
    /// </summary>
    public interface IDomainSnapshot
    {
        /// <summary>
        /// Group membership mapping.
        /// </summary>
        ReadOnlyDictionary<AccountSecurityIdentifier, AccountSecurityIdentifierSet> GroupMembership
        {
            get;
        }

        /// <summary>
        /// Users.
        /// </summary>
        ReadOnlySet<BaseUserAccount> Users
        {
            get;
        }

        /// <summary>
        /// Groups.
        /// </summary>
        ReadOnlySet<BaseGroupAccount> Groups
        {
            get;
        }

        /// <summary>
        /// Computers.
        /// </summary>
        ReadOnlySet<BaseComputerAccount> Computers
        {
            get;
        }

        /// <summary>
        /// Domains.
        /// </summary>
        ReadOnlySet<DomainAccount> Domains
        {
            get;
        }

        /// <summary>
        /// Organizational units.
        /// </summary>
        ReadOnlySet<OrganizationalUnit> OrganizationalUnits
        {
            get;
        }

        /// <summary>
        /// Returns all objects in the specified group.
        /// </summary>
        ReadOnlySet<IGroupMember> GetObjectsInGroup(BaseGroupAccount group);

        /// <summary>
        /// Returns all objects in the specified domain.
        /// </summary>
        ReadOnlySet<IDomainMember> GetObjectsInDomain(DomainAccount domain);

        /// <summary>
        /// Returns all objects in the specified domain.
        /// </summary>
        ReadOnlySet<IBaseDomainMember> GetObjectsInDomain(BaseDomainAccount domain);

        /// <summary>
        /// Returns all objects in the specified organizational unit.
        /// </summary>
        ReadOnlySet<IDomainMember> GetObjectsInOrganizationalUnit(OrganizationalUnit organizationalUnit);

        /// <summary>
        /// Merges the current spapshot with another one.
        /// </summary>
        IDomainSnapshot Merge(IDomainSnapshot snapshot);
    }
}
