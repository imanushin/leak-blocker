using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules;
using LeakBlocker.Libraries.Common.Entities.Settings.TemporaryAccess;

namespace LeakBlocker.Libraries.Common.Entities.Settings
{
    /// <summary>
    /// Object that represents product configuration.
    /// Class is immutable, guaranteed to be properly initialized.
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    public sealed class ProgramConfiguration : BaseEntity
    {
        [NonSerialized]
        private ReadOnlyList<Rule> sortedRules;

        /// <summary>
        /// Configuration version. If configuration is edited or a new one is created the version should be incremented.
        /// </summary>
        [DataMember]
        [Required]
        public int ConfigurationVersion
        {
            get;
            private set;
        }

        /// <summary>
        /// Выдает отсортированные правила в порядке возрастания их приоритета
        /// </summary>
        public ReadOnlyList<Rule> SortedRules
        {
            get
            {
                if (sortedRules == null)
                {
                    var rules = Rules.ToList();

                    rules.Sort((left,rigth)=>left.Priority - rigth.Priority);

                    sortedRules = rules.ToReadOnlyList();
                }

                return sortedRules;
            }
        }

        /// <summary>
        /// Collection of rules associated with the current configuration.
        /// </summary>
        [DataMember]
        [Required]
        [NotNull]
        public ReadOnlySet<Rule> Rules
        {
            get;
            private set;
        }

        /// <summary>
        /// Collection of temporary access conditions.
        /// </summary>
        [DataMember]
        [Required]
        [NotNull]
        public ReadOnlySet<BaseTemporaryAccessCondition> TemporaryAccess
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates an instance of Configuration class.
        /// </summary>
        /// <param name="configurationVersion">Configuration version.</param>
        /// <param name="rules">Collection of rules associated with the configuration.</param>
        /// <param name="temporaryAccess">Collection of temporary access conditions.</param>
        public ProgramConfiguration(int configurationVersion, IReadOnlyCollection<Rule> rules, IReadOnlyCollection<BaseTemporaryAccessCondition> temporaryAccess)
        {
            Check.IntegerIsGreaterThanZero(configurationVersion, "configurationVersion");
            Check.CollectionHasOnlyMeaningfulData(rules, "rules");
            Check.CollectionHasNoDefaultItems(temporaryAccess, "temporaryAccess");

            ConfigurationVersion = configurationVersion;
            Rules = rules.ToReadOnlySet();
            TemporaryAccess = temporaryAccess.ToReadOnlySet();
        }

        #region Equality

        /// <summary>
        /// Возвращает все вложенные объекты, которые имеют смысл для вычисления хеш-кода и метода Equals
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return ConfigurationVersion;
        }

        #endregion
    }
}
