using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.ServerShared.AdminViewCommunication
{
    /// <summary>
    /// Сущность, отображающая текущий статус компьютера
    /// </summary>
    [DataContract(IsReference = true)]
    public sealed class ManagedComputer : BaseReadOnlyObject
    {
        /// <summary>
        /// Поля, инициализированные здесь, нельзя менять в дальнейшем
        /// </summary>
        public ManagedComputer(BaseComputerAccount targetComputer, ManagedComputerData data)
        {
            Check.ObjectIsNotNull(data, "data");
            Check.ObjectIsNotNull(targetComputer, "targetComputer");

            TargetComputer = targetComputer;
            Data = data;
        }

        /// <summary>
        /// Пользователи, залогиненные на компьютер
        /// </summary>
        [DataMember]
        public ManagedComputerData Data
        {
            get;
            private set;
        }

        /// <summary>
        /// Полное DNS имя компьютера (например, myComputer.example.com)
        /// </summary>
        [UsedImplicitly]//WPF binding
        public string DnsName
        {
            get
            {
                return TargetComputer.FullName;
            }
        }

        /// <summary>
        /// Непосредственно, компьютер
        /// </summary>
        [DataMember]
        public BaseComputerAccount TargetComputer
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает все вложенные объекты, которые имеют смысл для вычисления хеш-кода и метода Equals
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return TargetComputer;
        }

        /// <summary>
        /// DNS имя компьютера
        /// </summary>
        protected override string GetString()
        {
            return DnsName;
        }
    }
}
