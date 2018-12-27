using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;

namespace LeakBlocker.Libraries.Common.Entities.Audit
{
    /// <summary>
    /// Фильтр, используемый для отчета
    /// </summary>
    [DataContract(IsReference = true)]
    [Serializable]
    public sealed class ReportFilter : BaseReadOnlyObject
    {
        private static readonly Dictionary<ReportType, Func<ReportFilter, bool>> reportTypesGenerator = CreateReportTypeGenerators();

        private static readonly ReportFilter defaultFilter = new ReportFilter(
            true,
            OperationDetail.OnlyDevices,
            OperationDetail.OnlyDevices,
            OperationDetail.OnlyDevices,
            true,
            true
            );

        /// <summary>
        /// Фильтр по умолчанию
        /// </summary>
        public static ReportFilter Default
        {
            get
            {
                return defaultFilter;
            }
        }

        /// <summary>
        /// Включать ли в отчет ошибки в работе приложения
        /// </summary>
        [DataMember]
        public bool Errors
        {
            get;
            private set;
        }

        /// <summary>
        /// Операции с результатом в виде хоть какого-нибудь ограничения доступа
        /// </summary>
        [DataMember]
        public OperationDetail BlockOperations
        {
            get;
            private set;
        }

        /// <summary>
        /// Операции с результатом "полный доступ разрешен"
        /// </summary>
        [DataMember]
        public OperationDetail AllowOperations
        {
            get;
            private set;
        }

        /// <summary>
        /// Операции, которые были разрешены из-за включенного временного доступа
        /// </summary>
        [DataMember]
        public OperationDetail TemporaryAccessOperations
        {
            get;
            private set;
        }

        /// <summary>
        /// Изменения в конфигурации
        /// </summary>
        [DataMember]
        public bool ConfigurationChanges
        {
            get;
            private set;
        }

        /// <summary>
        /// Некритические ошибки в работе программы
        /// </summary>
        [DataMember]
        public bool Warnings
        {
            get;
            private set;
        }

        /// <summary>
        /// Выдает все типы event'ов, которые могут быть использованы для отчетов
        /// </summary>
        public ReadOnlySet<ReportType> ReportTypes
        {
            get
            {
                return EnumHelper<ReportType>.Values.Where(item =>
                {
                    if (!reportTypesGenerator.ContainsKey(item))
                        Exceptions.Throw(ErrorMessage.NotFound, "ReportType {0} does not registered".Combine(item));

                    return reportTypesGenerator[item](this);
                }).ToReadOnlySet();
            }
        }

        /// <summary>
        /// Создает объект. В дальнейшем менять его нельзя
        /// </summary>
        public ReportFilter(
            bool errors,
            OperationDetail block,
            OperationDetail allow,
            OperationDetail temporaryAccess,
            bool configurationChanges,
            bool warnings)
        {
            Check.EnumerationValueIsDefined(block, "block");
            Check.EnumerationValueIsDefined(allow, "allow");
            Check.EnumerationValueIsDefined(temporaryAccess, "temporaryAccess");

            Errors = errors;
            BlockOperations = block;
            AllowOperations = allow;
            TemporaryAccessOperations = temporaryAccess;
            ConfigurationChanges = configurationChanges;
            Warnings = warnings;
        }


        /// <summary>
        /// Возвращает все вложенные объекты, которые имеют смысл для вычисления хеш-кода и метода Equals
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return Errors;
            yield return BlockOperations;
            yield return AllowOperations;
            yield return TemporaryAccessOperations;
            yield return ConfigurationChanges;
            yield return Warnings;
        }


        private static Dictionary<ReportType, Func<ReportFilter, bool>> CreateReportTypeGenerators()
        {
            return new Dictionary<ReportType, Func<ReportFilter, bool>>
            {
                { ReportType.None, item => false },
                { ReportType.ConfigurationChanges, item => item.ConfigurationChanges },
                { ReportType.DeviceTemporaryAccessAction, item => item.TemporaryAccessOperations != OperationDetail.None },
                { ReportType.Errors, item => item.Errors},
                { ReportType.FileAllow, item => item.AllowOperations == OperationDetail.DevicesAndFiles},
                { ReportType.FileTemporaryAccessAction, item => item.TemporaryAccessOperations == OperationDetail.DevicesAndFiles },
                { ReportType.FullDeviceAllow, item => item.AllowOperations != OperationDetail.None },
                { ReportType.FullDeviceBlock, item => item.BlockOperations != OperationDetail.None },
                { ReportType.FullFileBlock, item => item.BlockOperations == OperationDetail.DevicesAndFiles },
                { ReportType.Warnings, item => item.Warnings }
            };
        }

    }

    /// <summary>
    /// Подробрость отчета об операциях с устройствами
    /// </summary>
    public enum OperationDetail
    {
        /// <summary>
        /// Не включать в отчет ничего
        /// </summary>
        None = 0,

        /// <summary>
        /// Включать в отчет только глобальные операции с устройствами
        /// </summary>
        OnlyDevices,

        /// <summary>
        /// Включать в отчет операции с устройствами и операции с файлами
        /// </summary>
        DevicesAndFiles
    }
}
