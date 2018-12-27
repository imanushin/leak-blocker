using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.ServerShared.AdminViewCommunication
{
    /// <summary>
    /// Область действия агентов. Класс рассчитан на UI взаимодействие
    /// </summary>
    [DataContract(IsReference = true)]
    [Serializable]
    [KnownType(typeof(OrganizationalUnit))]
    [KnownType(typeof(Account))]
    public sealed class Scope : BaseReadOnlyObject
    {
        /// <summary>
        /// Конструктор. Инициализирует все поля, так как класс Read Only
        /// </summary>
        public Scope(IScopeObject parentObject)
        {
            Check.ObjectIsNotNull(parentObject, "parentObject");

            ScopeType = GetScopeType(parentObject);
            TargetObject = parentObject;
        }

        /// <summary>
        /// Тип области
        /// </summary>
        [DataMember]
        public ScopeType ScopeType
        {
            get;
            private set;
        }

        /// <summary>
        /// Полное имя области для UI-окна
        /// </summary>
        public string Name
        {
            get
            {
                return TargetObject.FullName;
            }
        }

        /// <summary>
        /// Объект, который представляет собой реальный Scope (он же используется в базе, высчитывается агентом и т. д.)
        /// </summary>
        [DataMember]
        public IScopeObject TargetObject
        {
            get;
            private set;
        }

        #region Equality

        /// <summary>
        /// Возвращает все вложенные объекты, которые имеют смысл для вычисления хеш-кода и метода Equals
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return ScopeType;
            yield return Name;
        }

        /// <summary>
        /// Сравнение двух Scope'ов.
        /// Первоначально сравниваются длины названий, если они равны, то сравниваются сами названия
        /// </summary>
        protected override int CustomCompare(object target)
        {
            var scope = (Scope)target;

            int result = Name.Length - scope.Name.Length;

            if (result != 0)
                return result;

            return string.CompareOrdinal(Name, scope.Name);
        }

        #endregion

        private static ScopeType GetScopeType(IScopeObject scope)
        {
            if (scope is BaseComputerAccount)
                return ScopeType.Computer;

            if (scope is BaseUserAccount)
                return ScopeType.User;

            if (scope is DomainAccount)
                return ScopeType.Domain;

            if (scope is BaseGroupAccount)
                return ScopeType.Group;

            if (scope is OrganizationalUnit)
                return ScopeType.OU;

            throw new InvalidOperationException("Object {0} is not a proper scope".Combine(scope));
        }

        /// <summary>
        /// Выдаем имя <see cref="Scope"/>. 
        /// </summary>
        protected override string GetString()
        {
            return Name;
        }
    }
}
