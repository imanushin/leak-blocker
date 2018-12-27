using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeakBlocker.Libraries.Storage.InternalTools
{
    /// <summary>
    /// Интерфейс для инициализации и смены настроек базы данных
    /// </summary>
    public interface IDatabaseInitializer
    {
        /// <summary>
        /// Пароль базы данных.
        /// Длинный пароль будет обрезан до 40 символов: http://msdn.microsoft.com/en-us/library/aa257373(v=sql.80).aspx
        /// </summary>
        string DatabasePassword
        {
            get;
            set;
        }
    }
}
