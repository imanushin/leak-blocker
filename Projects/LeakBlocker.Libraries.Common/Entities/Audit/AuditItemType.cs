using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Resources;

namespace LeakBlocker.Libraries.Common.Entities.Audit
{
    /// <summary>
    /// Event type.
    /// </summary>
    [EnumerationDescriptionProvider(typeof(AuditItemTypeDescriptions))]
    public enum AuditItemType
    {
        /// <summary>
        /// Incorrect value.
        /// </summary>
        [ForbiddenToUse]
        [Obsolete("Incorrect value", true)]
        None = 0,

        /// <summary>
        /// Сервер запустил удаленную установку агента
        /// </summary>
        [LinkedEnum(AuditItemGroupType.Installation)]
        [LinkedEnum(AuditItemSeverityType.Information)]
        [LinkedEnum(ReportType.None)]
        AgentSetupStarted = 1,

        /// <summary>
        /// Агент не отвечает, но компьютер включен.
        /// </summary>
        [LinkedEnum(AuditItemGroupType.Installation)]
        [LinkedEnum(AuditItemSeverityType.Warning)]
        [LinkedEnum(ReportType.Warnings)]
        AgentIsNotResponding = 2,

        /// <summary>
        /// Установка агента завершена успешно
        /// </summary>
        [LinkedEnum(AuditItemGroupType.Installation)]
        [LinkedEnum(AuditItemSeverityType.Information)]
        [LinkedEnum(ReportType.None)]
        AgentInstallationSucceeded = 3,

        /// <summary>
        /// Установка агента провалилась
        /// </summary>
        [LinkedEnum(AuditItemGroupType.Installation)]
        [LinkedEnum(AuditItemSeverityType.Warning)]
        [LinkedEnum(ReportType.Warnings)]
        AgentInstallationWasNotRequired = 16,

        /// <summary>
        /// Установка агента провалилась
        /// </summary>
        [LinkedEnum(AuditItemGroupType.Installation)]
        [LinkedEnum(AuditItemSeverityType.Error)]
        [LinkedEnum(ReportType.Errors)]
        AgentInstallationFailed = 4,

        /// <summary>
        /// Агентский компьютер выключается
        /// </summary>
        [LinkedEnum(AuditItemGroupType.System)]
        [LinkedEnum(AuditItemSeverityType.Information)]
        [LinkedEnum(ReportType.None)]
        AgentComputerTurningOff = 5,

        /// <summary>
        /// Агентский компьютер включен
        /// </summary>
        [LinkedEnum(AuditItemGroupType.System)]
        [LinkedEnum(AuditItemSeverityType.Information)]
        [LinkedEnum(ReportType.None)]
        AgentComputerTurnedOn = 6,

        /// <summary>
        /// Администратор изменил конфигурацию
        /// </summary>
        [LinkedEnum(AuditItemGroupType.Other)]
        [LinkedEnum(AuditItemSeverityType.Information)]
        [LinkedEnum(ReportType.ConfigurationChanges)]
        ConfigurationChanged = 7,

        /// <summary>
        /// Конфигурация для отчетов не настроена
        /// </summary>
        [LinkedEnum(AuditItemGroupType.Other)]
        [LinkedEnum(AuditItemSeverityType.Warning)]
        [LinkedEnum(ReportType.Warnings)]
        ReportsAreNotConfigured = 8,

        /// <summary>
        /// При отправке отчета произошла ошибка
        /// </summary>
        [LinkedEnum(AuditItemGroupType.Other)]
        [LinkedEnum(AuditItemSeverityType.Error)]
        [LinkedEnum(ReportType.Errors)]
        SendingReportFailed = 9,

        /// <summary>
        /// При обновлении внутренностей домена произошла ошибка
        /// </summary>
        [LinkedEnum(AuditItemGroupType.Other)]
        [LinkedEnum(AuditItemSeverityType.Error)]
        [LinkedEnum(ReportType.Errors)]
        UnableToRefreshDomainData = 10,

        /// <summary>
        /// OU/группа или компьютер участвуют напрямую в конфигурации, но в данный момент недоступны.
        /// То есть домен, в котором они присутствуют, есть, а их в домене нет
        /// </summary>
        [LinkedEnum(AuditItemGroupType.Other)]
        [LinkedEnum(AuditItemSeverityType.Warning)]
        [LinkedEnum(ReportType.Warnings)]
        DomainMemberIsNotAccessible = 11,

        /// <summary>
        /// При сетевом соединении с агентов (т. е. коннект от агента к серверу или обратно) возникла ошибка
        /// </summary>
        [LinkedEnum(AuditItemGroupType.System)]
        [LinkedEnum(AuditItemSeverityType.Error)]
        [LinkedEnum(ReportType.Errors)]
        ErrorInAgentCommunications = 12,

        /// <summary>
        /// При обновлении статусов на сервере произошла ошибка
        /// </summary>
        [LinkedEnum(AuditItemGroupType.System)]
        [LinkedEnum(AuditItemSeverityType.Error)]
        [LinkedEnum(ReportType.Errors)]
        ErrorInStatusUpdate = 13,

        /// <summary>
        /// Сервер включился
        /// </summary>
        [LinkedEnum(AuditItemGroupType.System)]
        [LinkedEnum(AuditItemSeverityType.Information)]
        [LinkedEnum(ReportType.None)]
        MainServiceStarted = 14,

        /// <summary>
        /// Сервер выключился
        /// </summary>
        [LinkedEnum(AuditItemGroupType.System)]
        [LinkedEnum(AuditItemSeverityType.Information)]
        [LinkedEnum(ReportType.None)]
        MainServiceStopped = 15,

        /// <summary>
        /// Read file access was blocked.
        /// </summary>
        [LinkedEnum(AuditItemGroupType.Devices)]
        [LinkedEnum(AuditItemSeverityType.Information)]
        [LinkedEnum(ReportType.FullFileBlock)]
        ReadFileBlocked = 100001,

        /// <summary>
        /// Read file access was allowed.
        /// </summary>
        [LinkedEnum(AuditItemGroupType.Devices)]
        [LinkedEnum(AuditItemSeverityType.Information)]
        [LinkedEnum(ReportType.FileAllow)]
        ReadFileAllowed = 100002,

        /// <summary>
        /// Read file access was temporarily allowed.
        /// </summary>
        [LinkedEnum(AuditItemGroupType.Devices)]
        [LinkedEnum(AuditItemSeverityType.Information)]
        [LinkedEnum(ReportType.FileAllow)]
        ReadFileTemporarilyAllowed = 100003,

        /// <summary>
        /// Write file access was blocked.
        /// </summary>
        [LinkedEnum(AuditItemGroupType.Devices)]
        [LinkedEnum(AuditItemSeverityType.Information)]
        [LinkedEnum(ReportType.FullFileBlock)]
        WriteFileBlocked = 100004,

        /// <summary>
        /// Write file access was allowed.
        /// </summary>
        [LinkedEnum(AuditItemGroupType.Devices)]
        [LinkedEnum(AuditItemSeverityType.Information)]
        [LinkedEnum(ReportType.FileAllow)]
        WriteFileAllowed = 100005,

        /// <summary>
        /// Write file access was temporarily allowed.
        /// </summary>
        [LinkedEnum(AuditItemGroupType.Devices)]
        [LinkedEnum(AuditItemSeverityType.Information)]
        [LinkedEnum(ReportType.FileAllow)]
        WriteFileTemporarilyAllowed = 100006,

        /// <summary>
        /// Read and write file access was blocked.
        /// </summary>
        [LinkedEnum(AuditItemGroupType.Devices)]
        [LinkedEnum(AuditItemSeverityType.Information)]
        [LinkedEnum(ReportType.FullFileBlock)]
        ReadWriteFileBlocked = 100007,

        /// <summary>
        /// Read and write file access was allowed.
        /// </summary>
        [LinkedEnum(AuditItemGroupType.Devices)]
        [LinkedEnum(AuditItemSeverityType.Information)]
        [LinkedEnum(ReportType.FileAllow)]
        ReadWriteFileAllowed = 100008,

        /// <summary>
        /// Read and write file access was temporarily allowed.
        /// </summary>
        [LinkedEnum(AuditItemGroupType.Devices)]
        [LinkedEnum(AuditItemSeverityType.Information)]
        [LinkedEnum(ReportType.FileAllow)]
        ReadWriteFileTemporarilyAllowed = 100009,

        /// <summary>
        /// Device was blocked.
        /// </summary>
        [LinkedEnum(AuditItemGroupType.Devices)]
        [LinkedEnum(AuditItemSeverityType.Information)]
        [LinkedEnum(ReportType.FullDeviceBlock)]
        DeviceAccessBlocked = 100010,

        /// <summary>
        /// Device was allowed in read-only mode.
        /// </summary>
        [LinkedEnum(AuditItemGroupType.Devices)]
        [LinkedEnum(AuditItemSeverityType.Information)]
        [LinkedEnum(ReportType.FullDeviceAllow)]
        DeviceAccessReadOnly = 100011,

        /// <summary>
        /// Device was allowed.
        /// </summary>
        [LinkedEnum(AuditItemGroupType.Devices)]
        [LinkedEnum(AuditItemSeverityType.Information)]
        [LinkedEnum(ReportType.FullDeviceAllow)]
        DeviceAccessAllowed = 100012,

        /// <summary>
        /// Device was temporarily allowed in read-only mode.
        /// </summary>
        [LinkedEnum(AuditItemGroupType.Devices)]
        [LinkedEnum(AuditItemSeverityType.Information)]
        [LinkedEnum(ReportType.FullDeviceAllow)]
        DeviceAccessTemporarilyReadOnly = 100013,

        /// <summary>
        /// Device was temporarily allowed.
        /// </summary>
        [LinkedEnum(AuditItemGroupType.Devices)]
        [LinkedEnum(AuditItemSeverityType.Information)]
        [LinkedEnum(ReportType.FullDeviceAllow)]
        DeviceAccessTemporarilyAllowed = 100014,

        /// <summary>
        /// Device was allowed because agent is not licensed.
        /// </summary>
        [LinkedEnum(AuditItemGroupType.Devices)]
        [LinkedEnum(AuditItemSeverityType.Warning)]
        [LinkedEnum(ReportType.Warnings)]
        DeviceAccessAllowedNotLicensed = 100025,

        /// <summary>
        /// Agent service was started.
        /// </summary>
        [LinkedEnum(AuditItemGroupType.System)]
        [LinkedEnum(AuditItemSeverityType.Information)]
        [LinkedEnum(ReportType.None)]
        AgentStarted = 100015,

        /// <summary>
        /// Agent service was stopped.
        /// </summary>
        [LinkedEnum(AuditItemGroupType.System)]
        [LinkedEnum(AuditItemSeverityType.Information)]
        [LinkedEnum(ReportType.None)]
        AgentStopped = 100016,

        /// <summary>
        /// User logged on.
        /// </summary>
        [LinkedEnum(AuditItemGroupType.System)]
        [LinkedEnum(AuditItemSeverityType.Information)]
        [LinkedEnum(ReportType.None)]
        UserLoggedOn = 100017,

        /// <summary>
        /// User logged off.
        /// </summary>
        [LinkedEnum(AuditItemGroupType.System)]
        [LinkedEnum(AuditItemSeverityType.Information)]
        [LinkedEnum(ReportType.None)]
        UserLoggedOff = 100018,

        /// <summary>
        /// Computer was turned on.
        /// </summary>
        [LinkedEnum(AuditItemGroupType.System)]
        [LinkedEnum(AuditItemSeverityType.Information)]
        [LinkedEnum(ReportType.None)]
        ComputerTurnedOn = 100019,

        /// <summary>
        /// Computer was turned off or put into sleep state.
        /// </summary>
        [LinkedEnum(AuditItemGroupType.System)]
        [LinkedEnum(AuditItemSeverityType.Information)]
        [LinkedEnum(ReportType.None)]
        ComputerTurnedOff = 100020,

        /// <summary>
        /// Device was connected.
        /// </summary>
        [LinkedEnum(AuditItemGroupType.Devices)]
        [LinkedEnum(AuditItemSeverityType.Information)]
        [LinkedEnum(ReportType.None)]
        DeviceConnected = 100021,

        /// <summary>
        /// Device was removed.
        /// </summary>
        [LinkedEnum(AuditItemGroupType.Devices)]
        [LinkedEnum(AuditItemSeverityType.Information)]
        [LinkedEnum(ReportType.None)]
        DeviceDisconnected = 100022,

        /// <summary>
        /// Server is inaccessible.
        /// </summary>
        [LinkedEnum(AuditItemGroupType.Other)]
        [LinkedEnum(AuditItemSeverityType.Warning)]
        [LinkedEnum(ReportType.Warnings)]
        ServerInaccessible = 100023,

        /// <summary>
        /// The last service shutdown was unplanned.
        /// </summary>
        [LinkedEnum(AuditItemGroupType.Other)]
        [LinkedEnum(AuditItemSeverityType.Error)]
        [LinkedEnum(ReportType.Errors)]
        UnplannedServiceShutdown = 100024,

        /// <summary>
        /// Agent was removed from the computer.
        /// </summary>
        [LinkedEnum(AuditItemGroupType.Installation)]
        [LinkedEnum(AuditItemSeverityType.Information)]
        [LinkedEnum(ReportType.ConfigurationChanges)]
        AgentUninstalled = 100026,

        /// <summary>
        /// Read file access was blocked.
        /// </summary>
        [LinkedEnum(AuditItemGroupType.Devices)]
        [LinkedEnum(AuditItemSeverityType.Information)]
        [LinkedEnum(ReportType.FullFileBlock)]
        ReadFileBlockedMultiple = 100027,

        /// <summary>
        /// Read file access was allowed.
        /// </summary>
        [LinkedEnum(AuditItemGroupType.Devices)]
        [LinkedEnum(AuditItemSeverityType.Information)]
        [LinkedEnum(ReportType.FileAllow)]
        ReadFileAllowedMultiple = 100028,

        /// <summary>
        /// Read file access was temporarily allowed.
        /// </summary>
        [LinkedEnum(AuditItemGroupType.Devices)]
        [LinkedEnum(AuditItemSeverityType.Information)]
        [LinkedEnum(ReportType.FileAllow)]
        ReadFileTemporarilyAllowedMultiple = 100029,

        /// <summary>
        /// Write file access was blocked.
        /// </summary>
        [LinkedEnum(AuditItemGroupType.Devices)]
        [LinkedEnum(AuditItemSeverityType.Information)]
        [LinkedEnum(ReportType.FullFileBlock)]
        WriteFileBlockedMultiple = 100030,

        /// <summary>
        /// Write file access was allowed.
        /// </summary>
        [LinkedEnum(AuditItemGroupType.Devices)]
        [LinkedEnum(AuditItemSeverityType.Information)]
        [LinkedEnum(ReportType.FileAllow)]
        WriteFileAllowedMultiple = 100031,

        /// <summary>
        /// Write file access was temporarily allowed.
        /// </summary>
        [LinkedEnum(AuditItemGroupType.Devices)]
        [LinkedEnum(AuditItemSeverityType.Information)]
        [LinkedEnum(ReportType.FileAllow)]
        WriteFileTemporarilyAllowedMultiple = 100032,

        /// <summary>
        /// Read and write file access was blocked.
        /// </summary>
        [LinkedEnum(AuditItemGroupType.Devices)]
        [LinkedEnum(AuditItemSeverityType.Information)]
        [LinkedEnum(ReportType.FullFileBlock)]
        ReadWriteFileBlockedMultiple = 100033,

        /// <summary>
        /// Read and write file access was allowed.
        /// </summary>
        [LinkedEnum(AuditItemGroupType.Devices)]
        [LinkedEnum(AuditItemSeverityType.Information)]
        [LinkedEnum(ReportType.FileAllow)]
        ReadWriteFileAllowedMultiple = 100034,

        /// <summary>
        /// Read and write file access was temporarily allowed.
        /// </summary>
        [LinkedEnum(AuditItemGroupType.Devices)]
        [LinkedEnum(AuditItemSeverityType.Information)]
        [LinkedEnum(ReportType.FileAllow)]
        ReadWriteFileTemporarilyAllowedMultiple = 100035,
    }
}