using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common.Entities.Audit
{
    /// <summary>
    /// Тип event'а, с точки зрения отчета.
    /// </summary>
    public enum ReportType
    {
        /// <summary>
        /// Event не будет включен ни в какой отчет
        /// </summary>
        None = 0,

        /// <summary>
        /// События с ошибками
        /// </summary>
        Errors = 1,

        /// <summary>
        /// События с блокировкой подключенного устройства
        /// </summary>
        FullDeviceBlock,

        /// <summary>
        /// События с блокировкой отдельных файлов
        /// </summary>
        FullFileBlock,

        /// <summary>
        /// События с разрешением доступа к отдельным устройству (то есть не создается никаких препятствий)
        /// </summary>
        FullDeviceAllow,

        /// <summary>
        /// События с разрешением доступа к отдельным файлам
        /// </summary>
        FileAllow,

        /// <summary>
        /// События с разрешением устройств, в которых оказывает влияние временный доступ
        /// </summary>
        DeviceTemporaryAccessAction,

        /// <summary>
        /// События с разрешением файлов, если оказывается влияние временного доступа
        /// </summary>
        FileTemporaryAccessAction,

        /// <summary>
        /// Изменения в конфигурации
        /// </summary>
        ConfigurationChanges,

        /// <summary>
        /// Некорректная работа сервиса, не ведущая к фатальным ошибкам
        /// </summary>
        Warnings,
    }
}
