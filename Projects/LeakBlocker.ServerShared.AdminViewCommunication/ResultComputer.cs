using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.ServerShared.AdminViewCommunication
{
    /// <summary>
    /// Объект для показа элементов списка результатов обработки Scope данных.
    /// То есть основная задача объекта: хранить данные: какой компьютер и почему будет добавлен в Scope
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    public sealed class ResultComputer : BaseReadOnlyObject
    {
        /// <summary>
        /// Конструктор. Иницилизирует параметры, в дальнейшем объект не изменить
        /// </summary>
        public ResultComputer(Scope parentScope, BaseComputerAccount targetAccount)
        {
            Check.ObjectIsNotNull(parentScope, "parentScope");
            Check.ObjectIsNotNull(targetAccount, "targetAccount");

            Scope = parentScope;
            TargetAccount = targetAccount;
        }

        /// <summary>
        /// Имя компьютера
        /// </summary>
        [UsedImplicitly]//WPF binding
        public string Name
        {
            get
            {
                return TargetAccount.ShortName;
            }
        }

        /// <summary>
        /// Полное DNS имя компьютера (например, myComputer.example.com)
        /// </summary>
        [UsedImplicitly]//WPF binding
        public string DnsName
        {
            get
            {
                return TargetAccount.FullName;
            }
        }

        /// <summary>
        /// Scope, в которой содержится этот компьютер
        /// </summary>
        [DataMember]
        public Scope Scope
        {
            get;
            private set;
        }

        /// <summary>
        /// Непосредственно, компьютер
        /// </summary>
        [DataMember]
        public BaseComputerAccount TargetAccount
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает все вложенные объекты, которые имеют смысл для вычисления хеш-кода и метода Equals
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return TargetAccount;
        }

        /// <summary>
        /// Converts the current instance to string.
        /// </summary>
        /// <returns>String that describes the current instance.</returns>
        protected override string GetString()
        {
            return Name;
        }
    }
}
