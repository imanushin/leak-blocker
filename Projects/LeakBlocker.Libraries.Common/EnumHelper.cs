using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;

namespace LeakBlocker.Libraries.Common
{
    /// <summary>
    /// Расширение статического класса Enum для корректной обработки дополнительной логики, такой как игнорирование запрещенных к использованию
    /// значений.
    /// Класс используется в Check'е и тестируется там же
    /// </summary>
    /// <typeparam name="T">Enumeration type.</typeparam>
    public static class EnumHelper<T> where T : struct, IComparable, IFormattable, IConvertible
    {
        private static readonly Lazy<ReadOnlySet<T>> correctValues = new Lazy<ReadOnlySet<T>>(() =>
            ((T[])Enum.GetValues(typeof(T))).Where(IsValueAllowed).ToReadOnlySet());
        
        /// <summary>
        /// Значение enum'а, которые можно использовать. Игнорируются те из них, которые либо отсутствуют, либо если все имена запрещены к 
        /// использованию <seealso cref="ForbiddenToUseAttribute"/>
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static ReadOnlySet<T> Values
        {
            get
            {
                return correctValues.Value;
            }
        }

        private static bool IsValueAllowed(T value)
        {
            Type enumerationType = typeof(T);

            foreach (string name in Enum.GetNames(enumerationType))
            {
                T currentValue;

                if (!Enum.TryParse(name, out currentValue) || !currentValue.Equals(value))
                    continue;

                MemberInfo memberInfo = enumerationType.GetMember(name).FirstOrDefault();

                if (memberInfo == null)
                    return true;

                var attribute = (ForbiddenToUseAttribute)memberInfo.GetCustomAttributes(typeof(ForbiddenToUseAttribute), false).FirstOrDefault();

                if (attribute == null)
                    return true;
            }

            return false;
        }
    }
}
