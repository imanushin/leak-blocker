using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Resources;

namespace LeakBlocker.ServerShared.AdminViewCommunication
{
    /// <summary>
    /// Тип области
    /// </summary>
    [EnumerationDescriptionProvider(typeof(ScopeTypeResources))]
    public enum ScopeType
    {
        /// <summary>
        /// Домен
        /// </summary>
        Domain,

        /// <summary>
        /// Organizational Unit
        /// </summary>
        OU,

        /// <summary>
        /// Компьютер
        /// </summary>
        Computer,

        /// <summary>
        /// Пользователь. Имеет смысл только для Scope, в котором содержатся пользователи
        /// </summary>
        User,

        /// <summary>
        /// Группа, содержащая компьютеры
        /// </summary>
        Group
    }
}
