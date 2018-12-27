using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Network;

namespace LeakBlocker.ServerShared.AdminViewCommunication
{
    /// <summary>
    /// Интерфейс для настройки отчетов
    /// </summary>
    [NetworkObject]
    public interface IReportTools : IDisposable
    {
        /// <summary>
        /// Загружает настройки отчетов
        /// </summary>
        ReportConfiguration LoadSettings();

        /// <summary>
        /// Сохраняет настройки отчетов
        /// </summary>
        void SaveSettings(ReportConfiguration settings);

        /// <summary>
        /// Отсылает тестовый отчет с информацией за последние сутки.
        /// Если результате null - то оправка была успешной. Иначе - Exception c описанием ошибки
        /// </summary>
        string TrySendTestReport(ReportConfiguration configuration);
    }
}
