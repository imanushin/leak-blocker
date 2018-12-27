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
    /// Класс для работы с Credentials в базе данных 
    /// </summary>
    public interface ICredentialsManager
    {
        /// <summary>
        /// Выдает Credential'ы для заданного объекта. Если они еще не заданы, то возвращается null
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [CanBeNull]
        Credentials TryGetCredentials(BaseDomainAccount account);

        /// <summary>
        /// Удаляет старые Credentials и устанавливает новые
        /// </summary>
        /// <param name="credentials"></param>
        void UpdateCredentials(Credentials credentials);

        /// <summary>
        /// Retrieves all credentials.
        /// </summary>
        /// <returns>Set of credentials.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        ReadOnlySet<Credentials> GetAllCredentials();
    }
}
