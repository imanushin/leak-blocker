using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common.Entities.Security
{
    /// <summary>
    /// Хранит ключ шифрования между агентом и сервером
    /// </summary>
    public sealed class AgentEncryptionData : BaseEntity
    {
        /// <summary>
        /// Создает объект
        /// </summary>
        public AgentEncryptionData(BaseComputerAccount computer, string key)
        {
            Check.ObjectIsNotNull(computer, "computer");
            Check.StringIsMeaningful(key, "key");

            Computer = computer;
            Key = key;
        }

        /// <summary>
        /// Компьютер, с которым ассоциирован ключ
        /// </summary>
        public BaseComputerAccount Computer
        {
            get;
            private set;
        }

        /// <summary>
        /// Base64 представление ключа
        /// </summary>
        public string Key
        {
            get;
            private set;
        }

        /// <summary>
        /// Выдает все внутренние объекты
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return Computer;
            yield return Key;
        }

        /// <summary>
        /// Текстовое представление структуры. Скрыто поле <see cref="Key"/>
        /// </summary>
        /// <returns></returns>
        protected override string GetString()
        {
            return "AgentEncryptionData: Computer - {0}".Combine(Computer);
        }
    }
}
