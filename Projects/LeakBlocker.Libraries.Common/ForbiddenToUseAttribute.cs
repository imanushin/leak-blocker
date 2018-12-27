using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common
{
    /// <summary>
    /// Аттрибут для запрета определенных значений в enum'ах.
    /// Им следует помечать значения, которые запрещены к использованию в параметрах и пр.
    /// Например, значение по-умолчанию (==0) может не иметь корректного названия (кроме как None).
    /// Следовательно, его можно использовать внутри методов, но нельзя передавать в функции, конструкторы, свойства и т. д.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class ForbiddenToUseAttribute : Attribute
    {
    }
}