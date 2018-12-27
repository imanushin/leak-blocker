using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Audit;

namespace LeakBlocker.Libraries.Storage
{
    /// <summary>
    /// Менеджер по работе с аудитом в базе
    /// </summary>
    public interface IAuditItemsManager
    {
        /// <summary>
        /// Добавляет набор AuditItem'ов в базу
        /// </summary>
        void AddItems(IEnumerable<AuditItem> items);

        /// <summary>
        /// Аналог методы <see cref="AddItems"/>, но добавляется только один элемент
        /// </summary>
        void AddItem(AuditItem item);

        /// <summary>
        /// Выдает данные из базы в соответствии с фильтром. Выдаются максимум первые <paramref name="topCount"/> элементов, сортировка идет от старых event'ов к новым
        /// Если <paramref name="topCount"/> == -1, то будут возвращены все записи
        /// </summary>
        ReadOnlyList<AuditItem> GetItems(AuditFilter filter, int topCount);

        /// <summary>
        /// Выдает данные из базы в соответствии с фильтров для отчета. Выдает все результаты, сортировка идет от старых event'ов к новым
        /// </summary>
        ReadOnlyList<AuditItem> GetItems(ReportFilter filter, Time startTime, Time endTime);
    }
}