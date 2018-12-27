using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;

namespace LeakBlocker.Libraries.Common
{
    /// <summary>
    /// Выдает строку, которая используется в <see cref="StringNamedFormatter.ApplyTemplate"/>
    /// </summary>
    public delegate string GetString();

    /// <summary>
    /// Преобразовывает строку вида "adasd {#Name1}" в "adasd value1", если есть конвертер со строкой Name1, а функция выдает value1
    /// </summary>
    public static class StringNamedFormatter
    {
        /// <summary>
        /// Применяет шаблон к строке.
        /// </summary>
        public static string ApplyTemplate(string template, ReadOnlyDictionary<string, GetString> values)
        {
            Check.ObjectIsNotNull(template, "template");
            Check.CollectionIsNotNullOrEmpty(values, "values");

            string result = template;

            foreach (var pair in values)
            {
                string key = "{{{0}}}".Combine(pair.Key);

                if (result.Contains(key))//Сначала if, чтобы не вызывать каждый раз GetValue
                    result = result.Replace(key, pair.Value());
            }

            return result;
        }
    }
}
