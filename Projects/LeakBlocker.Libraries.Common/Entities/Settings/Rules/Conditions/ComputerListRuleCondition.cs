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
    /// Condition for checking computers.
    /// Class is immutable, guaranteed to be properly initialized.
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    [KnownType(typeof(DomainGroupAccount))]
    [KnownType(typeof(LocalGroupAccount))]
    public sealed class ComputerListRuleCondition : BaseRuleCondition
    {
        /// <summary>
        /// Collection of domains. If computer belongs to any of the specified domains then the condition is satisfied.
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
        /// Collection of organizational units. If computer belongs to any of the specified organizational units then the condition is satisfied.
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
        /// Collection of groups. If computer belongs to any of the specified groups then the condition is satisfied.
        /// </summary>
        [DataMember]
        [Required]
        [NotNull]
        public ReadOnlySet<DomainGroupAccount> Groups
        {
            get;
            private set;
        }

        /// <summary>
        /// Collection of computers. If computer is in this list then the condition is satisfied.
        /// </summary>
        [DataMember]
        [Required]
        [NotNull]
        public ReadOnlySet<BaseComputerAccount> Computers
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates an instance of ComputerListRuleCondition class.
        /// </summary>
        /// <param name="not">Logical inversion. Specify true if result of checking the condition should be inverted.</param>
        /// <param name="domains">List of domains.</param>
        /// <param name="organizationalUnits">List of organizational units.</param>
        /// <param name="groups">List of groups.</param>
        /// <param name="computers">List of computers.</param>
        public ComputerListRuleCondition(bool not, IReadOnlyCollection<DomainAccount> domains, IReadOnlyCollection<OrganizationalUnit> organizationalUnits,
            IReadOnlyCollection<DomainGroupAccount> groups, IReadOnlyCollection<BaseComputerAccount> computers)
            : base(not)
        {
            Check.CollectionHasNoDefaultItems(domains, "domains");
            Check.CollectionHasNoDefaultItems(organizationalUnits, "organizationalUnits");
            Check.CollectionHasNoDefaultItems(groups, "groups");
            Check.CollectionHasNoDefaultItems(computers, "computers");

            Domains = domains.ToReadOnlySet();
            OrganizationalUnits = organizationalUnits.ToReadOnlySet();
            Groups = groups.ToReadOnlySet();
            Computers = computers.ToReadOnlySet();
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
            yield return Computers;

            foreach (object innerObject in base.GetInnerObjects())
            {
                yield return innerObject;
            }

        }

        #endregion

        /// <summary/>
        /// Создает объект из списка Scope.
        /// Если ряд Scope не может быть использован в <see cref="ComputerListRuleCondition"/>, то будет брошено исключение со списком проблемных элементов
        public static ComputerListRuleCondition CreateFromScopeList(bool not, IReadOnlyCollection<IScopeObject> scopes)
        {
            Check.ObjectIsNotNull(scopes, "scopes");

            ReadOnlySet<DomainAccount> domains = scopes.OfType<DomainAccount>().ToReadOnlySet();
            ReadOnlySet<OrganizationalUnit> organizationalUnits = scopes.OfType<OrganizationalUnit>().ToReadOnlySet();
            ReadOnlySet<DomainGroupAccount> groups = scopes.OfType<DomainGroupAccount>().ToReadOnlySet();
            ReadOnlySet<BaseComputerAccount> computers = scopes.OfType<BaseComputerAccount>().ToReadOnlySet();

            IEnumerable<IScopeObject> superfluousItems = scopes.Without(domains).Without(organizationalUnits).Without(groups).Without(computers).ToReadOnlySet();

            if (superfluousItems.Any())
            {
                string scopesString = string.Join(", ", scopes.Select(scope => "{0} ({1})".Combine(scope, scopes.GetType().Name)));

                string errorText = "Unable to add these objects to {0}: {1}.".Combine(typeof(ComputerListRuleCondition).Name, scopesString);

                throw new ArgumentException(errorText, "scopes");
            }

            return new ComputerListRuleCondition(not, domains, organizationalUnits, groups, computers);
        }
    }
}
