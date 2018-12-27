using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common.Entities.Security
{
    /// <summary>
    /// Объект, который может быть показан в UI
    /// </summary>
    public interface IScopeObject
    {
        /// <summary>
        /// Полное имя
        /// </summary>
        string FullName
        {
            get;
        }
    }
}
