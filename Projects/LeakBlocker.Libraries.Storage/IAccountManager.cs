using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.Libraries.Storage
{
    /// <summary>
    /// Отвечает за массовую выдачу аккаунтов
    /// </summary>
    public interface IAccountManager
    {
        /// <summary>
        /// Выдает все компьютеры, которые когда-либо были в базе
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        ReadOnlySet<BaseComputerAccount> GetSavedComputers();


        /// <summary>
        /// Выдает всех пользователей, которые когда-либо были в базе
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        ReadOnlySet<BaseUserAccount> GetSavedUsers();
    }
}
