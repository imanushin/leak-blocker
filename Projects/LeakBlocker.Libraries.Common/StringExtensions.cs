using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common
{
    /// <summary>
    /// Расширеник для строк
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Аналог string.Format.
        /// </summary>
        /// <param name="template">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns>A copy of format in which the format items have been replaced by the string representation of the corresponding objects in args.</returns>
        [StringFormatMethod("template")]
        public static string Combine([NotNull] this string template, params object[] args)
        {
            Check.ObjectIsNotNull(template, "template");

            return string.Format(CultureInfo.InvariantCulture, template, args);
        }

        /// <summary>
        /// Returns a value indicating whether the specified System.String object occurs within this string. 
        /// </summary>
        /// <param name="target">Current string.</param>
        /// <param name="value">The string to seek.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <returns>True if the value parameter occurs within this string, or if value is the empty string, otherwise, false.</returns>
        public static bool Contains(this string target, string value, StringComparison comparisonType)
        {
            Check.ObjectIsNotNull(target, "target");
            Check.ObjectIsNotNull(value, "value");

            return (target.IndexOf(value, comparisonType) >= 0);
        }
    }
}
