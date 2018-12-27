using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;

namespace LeakBlocker.Libraries.Common.Entities.Audit
{
    /// <summary>
    /// Упрощет получаение данных о связанных enum'ах
    /// </summary>
    /// <typeparam name="TTargetEnum"></typeparam>
    /// <typeparam name="TLinkedEnum"></typeparam>
    public static class LinkedEnumHelper<TTargetEnum, TLinkedEnum>
        where TTargetEnum : struct, IComparable, IFormattable, IConvertible
        where TLinkedEnum : struct, IComparable, IFormattable, IConvertible
    {
        private static readonly Dictionary<TTargetEnum, TLinkedEnum> reverseLinkedEnums = CreateReverseLinkedEnums();

        private static readonly ConcurrentDictionary<TLinkedEnum, ReadOnlySet<TTargetEnum>> linkedEnums = new ConcurrentDictionary<TLinkedEnum, ReadOnlySet<TTargetEnum>>();

        /// <summary>
        /// Выдает привязанный Enum, или бросает исключение, если его нет.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static TLinkedEnum GetLinkedEnum(TTargetEnum targetEnum)
        {
            var result = TryGetLinkedEnum(targetEnum);

            if (result.HasValue)
                return result.Value;

            Exceptions.Throw(ErrorMessage.NotFound, "Unable to find linked enum for {0}".Combine(targetEnum));
            return default(TLinkedEnum);
        }

        /// <summary>
        /// Выдает привязанный Enum, или бросает исключение, если его нет.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static TLinkedEnum? TryGetLinkedEnum(TTargetEnum targetEnum)
        {
            Check.EnumerationValueIsDefined(targetEnum, "targetEnum");

            return reverseLinkedEnums.TryGetValue(targetEnum);
        }

        private static Dictionary<TTargetEnum, TLinkedEnum> CreateReverseLinkedEnums()
        {
            var result = new Dictionary<TTargetEnum, TLinkedEnum>();

            foreach (string name in Enum.GetNames(typeof(TTargetEnum)))
            {
                TTargetEnum currentValue;

                if (!Enum.TryParse(name, out currentValue) || !EnumHelper<TTargetEnum>.Values.Contains(currentValue))
                    continue;

                MemberInfo memberInfo = typeof(TTargetEnum).GetMember(name).FirstOrDefault();

                if (memberInfo == null)
                    continue;

                TLinkedEnum severity = memberInfo
                    .GetCustomAttributes(typeof(LinkedEnumAttribute), false)
                    .OfType<LinkedEnumAttribute>()
                    .Where(attribute => attribute.IsMatches<TLinkedEnum>())
                    .Select(attribute => attribute.GetLinkedEnum<TLinkedEnum>())
                    .First();

                result.Add(currentValue, severity);
            }

            return result;
        }

        /// <summary>
        /// Выдает все <typeparamref name="TTargetEnum"/>, которые прикреплены к <paramref name="linkedEnum"/>
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static ReadOnlySet<TTargetEnum> GetAllLinkedEnums(TLinkedEnum linkedEnum)
        {
            var result = linkedEnums.TryGetValue(linkedEnum);

            if (result != null)
                return result;

            result =
                EnumHelper<TTargetEnum>
                    .Values
                    .Where(item => linkedEnum.Equals(GetLinkedEnum(item)))
                    .ToReadOnlySet();

            linkedEnums.TryAdd(linkedEnum, result);

            return result;
        }

    }
}
