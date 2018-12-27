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
    /// Обрабатывает запросы для сохранения и загрузки ключей агентов
    /// </summary>
    public interface IAgentEncryptionDataManager
    {
        /// <summary>
        /// Сохраняет данные об агенте
        /// </summary>
        void SaveAgent(AgentEncryptionData data);

        /// <summary>
        /// Выдает всех сохраненных агентов
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        ReadOnlySet<AgentEncryptionData> GetAllData();
    }
}
