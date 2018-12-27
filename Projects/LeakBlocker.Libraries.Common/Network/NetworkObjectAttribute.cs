using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common.Network
{
    /// <summary>
    /// Аттрибут для генерации объектов сетевых соединений
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public sealed class NetworkObjectAttribute : Attribute
    {
    }
}
