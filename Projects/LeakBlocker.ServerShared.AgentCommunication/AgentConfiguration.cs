using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Settings;

namespace LeakBlocker.ServerShared.AgentCommunication
{
    /// <summary>
    /// Актуальная конфигурация сервера для текущего агента
    /// </summary>
    [DataContract(IsReference = true)]
    public sealed class AgentConfiguration : BaseReadOnlyObject
    {
        /// <summary>
        /// Хватает ли у пользователя лицензий на этого агента
        /// </summary>
        [DataMember]
        public bool Licensed
        {
            get;
            private set;
        }

        /// <summary>
        /// Добавлен ли соответствующий компьютер в scope.
        /// </summary>
        [DataMember]
        public bool Managed
        {
            get;
            private set;
        }

        /// <summary>
        /// Текущие настройки программы
        /// </summary>
        [DataMember]
        public ProgramConfiguration Settings
        {
            get;
            private set;
        }

        /// <summary>
        /// Конструктор ReadOnly объекта
        /// </summary>
        public AgentConfiguration(ProgramConfiguration settings, bool licensed, bool managed)
        {
            Check.ObjectIsNotNull(settings, "settings");

            Settings = settings;
            Licensed = licensed;
            Managed = managed;
        }

        /// <summary>
        /// Перечисляет все вложенные объекты
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return Licensed;
            yield return Settings;
        }
    }
}
