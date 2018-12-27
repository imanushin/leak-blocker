using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Audit;

namespace LeakBlocker.ServerShared.AdminViewCommunication
{
    /// <summary>
    /// Настройки отчета
    /// </summary>
    [DataContract(IsReference = true)]
    public sealed class ReportConfiguration : BaseReadOnlyObject
    {

        /// <summary>
        /// Включены ли отчеты
        /// </summary>
        [DataMember]
        public bool AreEnabled
        {
            get;
            private set;
        }

        /// <summary>
        /// Настройки электронной почты
        /// </summary>
        [DataMember]
        public EmailSettings Email
        {
            get;
            private set;
        }

        /// <summary>
        /// Фильтр событий
        /// </summary>
        [DataMember]
        public ReportFilter Filter
        {
            get;
            private set;
        }

        /// <summary>
        /// Создает объект. В дальнейшем менять его нельзя
        /// </summary>
        public ReportConfiguration(bool areEnabled, ReportFilter filter, EmailSettings email)
        {
            Check.ObjectIsNotNull(email, "email");
            Check.ObjectIsNotNull(filter, "filter");

            AreEnabled = areEnabled;
            Filter = filter;
            Email = email;
        }


        /// <summary>
        /// Возвращает все вложенные объекты, которые имеют смысл для вычисления хеш-кода и метода Equals
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return AreEnabled;
            yield return Filter;
            yield return Email;
        }
    }
}
