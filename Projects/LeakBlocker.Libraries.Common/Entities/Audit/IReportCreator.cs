using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;

namespace LeakBlocker.Libraries.Common.Entities.Audit
{
    /// <summary>
    /// Создает отчеты в формате html
    /// </summary>
    public interface IReportCreator
    {
        /// <summary>
        /// Создает ежедневный отчет
        /// </summary>
        string CreateReportHtml(IReadOnlyCollection<AuditItem> auditItems, Time startTime, Time endTime);

        /// <summary>
        /// Создает html документ по списку item'ов
        /// </summary>
        string ExportAuditToHtml(IReadOnlyCollection<AuditItem> auditItems, AuditFilter filter);
    }
}
