using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.ServerShared.AdminViewCommunication
{
    /// <summary>
    /// Настройки почтового сервера
    /// </summary>
    [DataContract(IsReference = true)]
    public sealed class EmailSettings : BaseReadOnlyObject
    {
        /// <summary>
        /// От кого отсылается отчет
        /// </summary>
        [DataMember]
        public string From
        {
            get;
            private set;
        }

        /// <summary>
        /// Кому отсылается отчет
        /// </summary>
        [DataMember]
        public string To
        {
            get;
            private set;
        }

        /// <summary>
        /// Сервер для отправки почты
        /// </summary>
        [DataMember]
        public string SmtpHost
        {
            get;
            private set;
        }

        /// <summary>
        /// Порт на SMTP сервере
        /// </summary>
        [DataMember]
        public int Port
        {
            get;
            private set;
        }

        /// <summary>
        /// Использовать ли SSL при подключении
        /// </summary>
        [DataMember]
        public bool UseSslConnection
        {
            get;
            private set;
        }

        /// <summary>
        /// Включена ил аутентификация
        /// </summary>
        [DataMember]
        public bool IsAuthenticationEnabled
        {
            get;
            private set;
        }

        /// <summary>
        /// Пользователь, который участвует в аутентификации
        /// </summary>
        [DataMember]
        public string UserName
        {
            get;
            private set;
        }

        /// <summary>
        /// Пароль, который участвует в аутентификации
        /// </summary>
        [DataMember]
        public string Password
        {
            get;
            private set;
        }

        /// <summary>
        /// Создает экземпляр. Некоторые свойства могут быть пустыми
        /// </summary>
        [StringCanBeEmpty("from")]
        [StringCanBeEmpty("to")]
        [StringCanBeEmpty("smtpHost")]
        [StringCanBeEmpty("userName")]
        [StringCanBeEmpty("password")]
        public EmailSettings(
            string from,
            string to,
            string smtpHost,
            int smtpPort,
            bool useSslConnection,
            bool isAuthenticationEnabled,
            string userName,
            string password)
        {
            Check.ObjectIsNotNull(from, "from");
            Check.ObjectIsNotNull(to, "to");
            Check.ObjectIsNotNull(smtpHost, "smtpHost");
            Check.ObjectIsNotNull(userName, "userName");
            Check.ObjectIsNotNull(password, "password");

            Port = smtpPort;
            From = from;
            To = to;
            SmtpHost = smtpHost;
            UseSslConnection = useSslConnection;
            IsAuthenticationEnabled = isAuthenticationEnabled;
            UserName = userName;
            Password = password;
        }

        /// <summary>
        /// Возвращает все вложенные объекты, которые имеют смысл для вычисления хеш-кода и метода Equals
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return From;
            yield return To;
            yield return SmtpHost;
            yield return Port;
            yield return UseSslConnection;
            yield return IsAuthenticationEnabled;
            yield return UserName;
            yield return Password;
        }

        /// <summary>
        /// Текстовое представление. Используется только для отладки
        /// </summary>
        protected override string GetString()
        {
            return "EmailSettings: From - {0}; To - {1}; Server - {2}; IsAuthEnabled - {3}; User: {4}"
                .Combine(From, To, SmtpHost, IsAuthenticationEnabled, UserName ?? "none");
        }
    }
}
