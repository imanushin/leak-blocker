using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common.Entities.Settings.Rules.Conditions
{
    /// <summary>
    /// Base class for all conditions.
    /// Class is immutable, guaranteed to be properly initialized.
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    [KnownType(typeof(CompositeRuleCondition))]
    [KnownType(typeof(ComputerListRuleCondition))]
    [KnownType(typeof(DeviceListRuleCondition))]
    [KnownType(typeof(UserListRuleCondition))]
    [KnownType(typeof(DeviceTypeRuleCondition))]
    public abstract class BaseRuleCondition : BaseEntity
    {
        /// <summary>
        /// Logical inversion. If this property is set to true if result of checking the condition should be inverted.
        /// </summary>
        [DataMember]
        [Required]
        public bool Not
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes properties of the current item.
        /// </summary>
        /// <param name="not">Logical inversion. Specify true if result of checking the condition should be inverted.</param>
        internal BaseRuleCondition(bool not)
        {
            Not = not;
        }

        #region Protected extensions

        //protected static bool IsObjectInGroup(IGroupMember computer, DomainGroupAccount group, bool ignoreCache)
        //{
        //    return computer.CheckMembership(group, ignoreCache: ignoreCache);
        //}

        //protected static bool IsObjectInOrganizationalUnit(IActiveDirectoryMember computer, OrganizationalUnit organizationalUnit)
        //{
        //    return computer.IsInOrganizationalUnit(organizationalUnit);
        //}

        #endregion Protected extensions

        #region Equality

        /// <summary>
        /// Возвращает все вложенные объекты, которые имеют смысл для вычисления хеш-кода и метода Equals
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return Not;
        }

        #endregion
    }
}
