using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.Libraries.Common.Entities
{
    /// <summary>
    /// Данные о пользователе, которые могут быть использованы для покупки продукта
    /// </summary>
    [DataContract(IsReference = true)]
    public sealed class UserContactInformation : BaseReadOnlyObject
    {
        private static readonly UserContactInformation empty = new UserContactInformation(string.Empty,string.Empty,string.Empty,string.Empty,string.Empty);

        /// <summary>
        /// Пустые контактные данные
        /// </summary>
        public static UserContactInformation Empty
        {
            get
            {
                return empty;
            }
        }

        /// <summary>
        /// Конструктор. Все строковые поля могут быть пустыми
        /// </summary>
        [StringCanBeEmpty("firstName")]
        [StringCanBeEmpty("lastName")]
        [StringCanBeEmpty("companyName")]
        [StringCanBeEmpty("email")]
        [StringCanBeEmpty("phone")]
        public UserContactInformation(string firstName, string lastName, string companyName, string email, string phone)
        {
            Check.ObjectIsNotNull(firstName, "firstName");
            Check.ObjectIsNotNull(lastName, "lastName");
            Check.ObjectIsNotNull(companyName, "companyName");
            Check.ObjectIsNotNull(email, "email");
            Check.ObjectIsNotNull(phone, "phone");

            FirstName = firstName;
            LastName = lastName;
            CompanyName = companyName;
            Email = email;
            Phone = phone;
        }
        
        /// <summary>
        /// Имя
        /// </summary>
        [DataMember]
        public string FirstName
        {
            get;
            private set;
        }

        /// <summary>
        /// Фамилия
        /// </summary>
        [DataMember]
        public string LastName
        {
            get;
            private set;
        }

        /// <summary>
        /// Имя компании
        /// </summary>
        [DataMember]
        public string CompanyName
        {
            get;
            private set;
        }

        /// <summary>
        /// Почтовый адрес
        /// </summary>
        [DataMember]
        public string Email
        {
            get;
            private set;
        }

        /// <summary>
        /// Телефон
        /// </summary>
        [DataMember]
        public string Phone
        {
            get;
            private set;
        }

        /// <summary>
        /// Выдает все вложенные объекты
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return FirstName;
            yield return LastName;
            yield return CompanyName;
            yield return Email;
            yield return Phone;
        }
    }
}
