using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Audit;
using LeakBlocker.Libraries.Common.Network;

namespace LeakBlocker.ServerShared.AdminViewCommunication
{
    /// <summary>
    /// Web-сервисы для поддержки аудита
    /// </summary>
    [NetworkObject]
    public interface IAuditTools : IDisposable
    {
        /// <summary>
        /// Сохраняет список открытых вкладок с фильтрами на диск
        /// </summary>
        void SaveFilterSet(ReadOnlyList<AuditFilter> filters);

        /// <summary>
        /// Выдает последний сохраненный список вкладок
        /// </summary>
        ReadOnlyList<AuditFilter> LoadFilters();

        /// <summary>
        /// Создает новый фильтр
        /// </summary>
        void CreateFilter(AuditFilter filter);
        
        /// <summary>
        /// Удаляет фильтр
        /// </summary>
        void DeleteFilter(AuditFilter filter);

        /// <summary>
        /// Заменяет один фильтр на другой.
        /// На входе во fromFilter или toFilter может быть null, но запрещены null`ы в обоих аргументах сразу
        /// </summary>
        void ChangeFilter(AuditFilter fromFilter, AuditFilter toFilter);

        /// <summary>
        /// Выдает все устройства, когда-либо добавленные в аудит
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        ReadOnlySet<DeviceDescription> GetAuditDevices();

        /// <summary>
        /// Выдает список <see cref="AuditItem"/>, соответствующий заданному фильтру. Выдается не более <paramref name="topCount"/> записей.
        /// Если <paramref name="topCount"/> == -1, то будут возвращены все записи
        /// </summary>
        ReadOnlyList<AuditItem> GetItemsForFilter(AuditFilter filter, int topCount);
    }
}
