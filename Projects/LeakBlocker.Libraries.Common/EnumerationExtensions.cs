using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common
{
    /// <summary>
    /// Extension methods for enumerations.
    /// </summary>
    public static class EnumerationExtensions 
    {
        private static readonly ConcurrentDictionary<Enum, string> cache = new ConcurrentDictionary<Enum, string>();

        /// <summary>
        /// Returns a localized description for the specified value. Enumeration must be marked with EnumerationDescriptionProvider
        /// attribute, and the specified resource must contain the corresponding string.
        /// </summary>
        /// <param name="value">Enumeration value.</param>
        /// <returns>Enumeration description.</returns>
        public static string GetValueDescription(this Enum value)
        {
            string result;
            if (cache.TryGetValue(value, out result))
                return result;

            Type type = value.GetType();

            EnumerationDescriptionProviderAttribute attribute = type.GetCustomAttributes(true).OfType<EnumerationDescriptionProviderAttribute>().FirstOrDefault();
            if (attribute == null)
                Exceptions.Throw(ErrorMessage.NotFound, "Description provider was not found for type {0}.".Combine(value.GetType().Name));

            result = attribute.Provider.GetString(value.ToString());

            if (result == null)
                Exceptions.Throw(ErrorMessage.NotFound, "Description was not found for value {0}.".Combine(value));

            cache[value] = result;

            return result;
        }

        /// <summary>
        /// Checks if enumeration value has the specified flag.
        /// </summary>
        /// <typeparam name="T">Enumeration type.</typeparam>
        /// <param name="value">Enumeration value.</param>
        /// <param name="flag">Flag that should be checked.</param>
        /// <returns>True if value has the specified flag.</returns>
        public static bool HasFlag<T>(this T value, T flag) where T : struct, IComparable, IFormattable, IConvertible
        {
            var checkedValue = value as Enum;
            var checkedFlag = flag as Enum;

            if (checkedValue == null)
                Exceptions.Throw(ErrorMessage.UnsupportedType, "Type " + typeof(T).Name + " is not an enumeration.");

            return checkedValue.HasFlag(checkedFlag);
        }        
    }
}
