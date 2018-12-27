using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules.Conditions;

namespace LeakBlocker.Libraries.Common.Entities.Settings.Rules
{
    /// <summary>
    /// Rule that contains conditions. If one or more conditions are satisfied under some circumstances
    /// then all actions from the corresponding collection should be performed.
    /// Class is immutable, guaranteed to be properly initialized.
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    public sealed class Rule : BaseEntity
    {
        /// <summary>
        /// Actions that should be performed if the condition is satisfied.
        /// </summary>
        [DataMember]
        [Required]
        public ActionData Actions
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Rule name.
        /// </summary>
        [DataMember]
        [Required]
        [NotNull]
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Root condition of the rule.
        /// </summary>
        [DataMember]
        [Required]
        [NotNull]
        public BaseRuleCondition RootCondition
        {
            get;
            private set;
        }

        /// <summary>
        /// Rule priority.
        /// </summary>
        [DataMember]
        [Required]
        public int Priority
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates an instance of Rule class.
        /// </summary>
        /// <param name="name">Rule name.</param>
        /// <param name="priority">Rule priority.</param>
        /// <param name="rootCondition">Root condition.</param>
        /// <param name="actions">Actions that should be performed if the condition is satisfied.</param>
        public Rule(string name, int priority, BaseRuleCondition rootCondition, ActionData actions)
        {
            Check.StringIsMeaningful(name, "name");
            Check.IntegerIsGreaterThanZero(priority, "priority");
            Check.ObjectIsNotNull(rootCondition, "rootCondition");
            Check.ObjectIsNotNull(actions, "actions");

            Name = name;
            Priority = priority;
            RootCondition = rootCondition;
            Actions = actions;
        }

        #region Equality

        /// <summary>
        /// Возвращает все вложенные объекты, которые имеют смысл для вычисления хеш-кода и метода Equals
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return Name.ToUpperInvariant();
            yield return RootCondition;
            yield return Priority;
            yield return Actions;
        }
        
        #endregion

        /// <summary>
        /// Compares rules based on the priority. After sorting rules will be arranged in ascending order of priority.
        /// </summary>
        public static int CompareRulesUsingPriority(Rule first, Rule second)
        {
            Check.ObjectIsNotNull(first, "first");
            Check.ObjectIsNotNull(second, "second");

            return first.Priority - second.Priority;
        }
    }
}
