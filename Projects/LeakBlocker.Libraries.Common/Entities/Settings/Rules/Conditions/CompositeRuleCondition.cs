using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;

namespace LeakBlocker.Libraries.Common.Entities.Settings.Rules.Conditions
{
    #region CompositeRuleConditionType

    /// <summary>
    /// Composite condition type.
    /// </summary>
    public enum CompositeRuleConditionType
    {
        /// <summary>
        /// Incorrect value.
        /// </summary>
        [ForbiddenToUse]
        [Obsolete("Don't user", true)]
        None = 0,

        /// <summary>
        /// Logical OR. Result will be true if at least one contition is fulfilled.
        /// </summary>
        LogicalOr = 1,

        /// <summary>
        /// Logical AND. Result will be true if all contitions are fulfilled.
        /// </summary>
        LogicalAnd = 2
    }

    #endregion

    /// <summary>
    /// Class for composite conditions. Composite conditions can include other conditions. 
    /// Class is immutable, guaranteed to be properly initialized.
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    public sealed class CompositeRuleCondition : BaseRuleCondition
    {
        /// <summary>
        /// Operation type.
        /// </summary>
        [Required]
        [DataMember]
        public CompositeRuleConditionType OperationType
        {
            get;
            private set;
        }

        /// <summary>
        /// List of nested conditions.
        /// </summary>
        [DataMember]
        [Required]
        [NotNull]
        public ReadOnlySet<BaseRuleCondition> Conditions
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes properties of the current instance. 
        /// </summary>
        /// <param name="not">Logical inversion. Specify true if result of checking the condition should be inverted.</param>
        /// <param name="conditions">List of nested conditions.</param>
        /// <param name="operationType">Operation type.</param>
        public CompositeRuleCondition(bool not, IReadOnlyCollection<BaseRuleCondition> conditions, CompositeRuleConditionType operationType)
            : base(not)
        {
            Check.CollectionHasOnlyMeaningfulData(conditions, "conditions");
            Check.EnumerationValueIsDefined(operationType, "operationType");

            Conditions = conditions.ToReadOnlySet();
            OperationType = operationType;
        }

        #region Equality

        /// <summary>
        /// Возвращает все вложенные объекты, которые имеют смысл для вычисления хеш-кода и метода Equals
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return OperationType;
            yield return Conditions;

            foreach (object innerObject in base.GetInnerObjects())
            {
                yield return innerObject;
            }
        }

        #endregion
    }
}
