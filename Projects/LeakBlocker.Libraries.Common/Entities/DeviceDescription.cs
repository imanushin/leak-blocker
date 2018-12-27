using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common.Entities
{
    /// <summary>
    /// Represents logical device in operational system.
    /// Class is immutable, guaranteed to be properly initialized.
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    public sealed class DeviceDescription : BaseEntity
    {
        /// <summary>
        /// Device displayed name.
        /// </summary>
        [DataMember]
        [Required]
        [NotNull]
        public string FriendlyName
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Unique identifier that allows to uniquely identify the device across the domain.  
        /// </summary>
        [DataMember]
        [Required]
        [NotNull]
        public string Identifier
        {
            get;
            private set;
        }

        /// <summary>
        /// Device class.
        /// </summary>
        [DataMember]
        [Required]
        public DeviceCategory Category
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates an instance of LogicalDevice class.
        /// </summary>
        /// <param name="friendlyName">Device displayed name.</param>
        /// <param name="identifier">Device unique identifier.</param>
        /// <param name="category">Класс устройства</param>
        public DeviceDescription(string friendlyName, string identifier, DeviceCategory category)
        {
            Check.StringIsMeaningful(friendlyName, "friendlyName");
            Check.StringIsMeaningful(identifier, "identifier");
            Check.EnumerationValueIsDefined(category, "category");

            FriendlyName = friendlyName;
            Identifier = identifier;
            Category = category;
        }
        
        /// <summary>
        /// Текстовая информация об устройстве. Локализована, может отображаться в UI
        /// </summary>
        protected override string GetString()
        {
            return FriendlyName;
        }

        #region Equality

        /// <summary>
        /// Возвращает все вложенные объекты, которые имеют смысл для вычисления хеш-кода и метода Equals
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return Identifier;
        }
        
        #endregion
    }
}
