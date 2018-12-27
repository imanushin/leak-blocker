using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.Libraries.Common.Entities.Settings.Rules.Conditions
{
    /// <summary>
    /// Condition for checking users.
    /// Class is immutable, guaranteed to be properly initialized.
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    [KnownType(typeof(DomainGroupAccount))]
    [KnownType(typeof(LocalGroupAccount))]
    [KnownType(typeof(DomainUserAccount))]
    [KnownType(typeof(LocalUserAccount))]
    public sealed class UserListRuleCondition : BaseRuleCondition
    {
        /// <summary>
        /// Collection of domains. If user belongs to any of the specified domains then the condition is satisfied.
        /// </summary>
        [DataMember]
        [Required]
        [NotNull]
        public ReadOnlySet<DomainAccount> Domains
        {
            get;
            private set;
        }

        /// <summary>
        /// Collection of organizational units. If user belongs to any of the specified organizational units then the condition is satisfied.
        /// </summary>
        [DataMember]
        [Required]
        [NotNull]
        public ReadOnlySet<OrganizationalUnit> OrganizationalUnits
        {
            get;
            private set;
        }

        /// <summary>
        /// Collection of groups. If user belongs to any of the specified groups then the condition is satisfied.
        /// </summary>
        [DataMember]
        [Required]
        [NotNull]
        public ReadOnlySet<BaseGroupAccount> Groups
        {
            get;
            private set;
        }

        /// <summary>
        /// Collection of users. If user is in this list then the condition is satisfied.
        /// </summary>
        [DataMember]
        [Required]
        [NotNull]
        public ReadOnlySet<BaseUserAccount> Users
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates an instance of UserListRuleCondition class.
        /// </summary>
        /// <param name="not">Logical inversion. Specify true if result of checking the condition should be inverted.</param>
        /// <param name="domains">List of domains.</param>
        /// <param name="organizationalUnits">List of organizational units.</param>
        /// <param name="groups">List of groups.</param>
        /// <param name="users">List of users.</param>
        public UserListRuleCondition(bool not, IReadOnlyCollection<DomainAccount> domains, IReadOnlyCollection<OrganizationalUnit> organizationalUnits,
            IReadOnlyCollection<BaseGroupAccount> groups, IReadOnlyCollection<BaseUserAccount> users)
            : base(not)
        {
            Check.CollectionHasNoDefaultItems(domains, "domains");
            Check.CollectionHasNoDefaultItems(organizationalUnits, "organizationalUnits");
            Check.CollectionHasNoDefaultItems(groups, "groups");
            Check.CollectionHasNoDefaultItems(users, "users");

            Domains = domains.ToReadOnlySet();
            OrganizationalUnits = organizationalUnits.ToReadOnlySet();
            Groups = groups.ToReadOnlySet();
            Users = users.ToReadOnlySet();
        }

        #region Equality

        /// <summary>
        /// Возвращает все вложенные объекты, которые имеют смысл для вычисления хеш-кода и метода Equals
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return Domains;
            yield return Groups;
            yield return OrganizationalUnits;
            yield return Users;

            foreach (object innerObject in base.GetInnerObjects())
            {
                yield return innerObject;
            }

        }
        
        #endregion
        
        /// <summary>
        /// Создает объект из списка Scope.
        /// Если ряд Scope не может быть использован в <see cref="UserListRuleCondition"/>, то будет брошено исключение со списком проблемных элементов
        /// </summary>
        public static UserListRuleCondition CreateFromScopeList(bool not, IReadOnlyCollection<IScopeObject> scopes)
        {
            Check.ObjectIsNotNull(scopes, "scopes");

            ReadOnlySet<DomainAccount> domains = scopes.OfType<DomainAccount>().ToReadOnlySet();
            ReadOnlySet<OrganizationalUnit> organizationalUnits = scopes.OfType<OrganizationalUnit>().ToReadOnlySet();
            ReadOnlySet<BaseGroupAccount> groups = scopes.OfType<BaseGroupAccount>().ToReadOnlySet();
            ReadOnlySet<BaseUserAccount> users = scopes.OfType<BaseUserAccount>().ToReadOnlySet();

            IEnumerable<IScopeObject> superfluousItems = scopes.Without(domains).Without(organizationalUnits).Without(groups).Without(users).ToReadOnlySet();

            if (superfluousItems.Any())
            {
                string scopesString = string.Join(", ", scopes.Select(scope => "{0} ({1})".Combine(scope, scopes.GetType().Name)));

                string errorText = "Unable to add these objects to {0}: {1}.".Combine(typeof(UserListRuleCondition).Name, scopesString);

                throw new ArgumentException(errorText, "scopes");
            }

            return new UserListRuleCondition(not, domains, organizationalUnits, groups, users);
        }

    }
}
