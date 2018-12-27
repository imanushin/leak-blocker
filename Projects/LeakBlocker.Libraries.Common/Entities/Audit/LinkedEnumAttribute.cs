using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common.Entities.Audit
{
    /// <summary>
    /// Связывает значения enum`ов
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    internal sealed class LinkedEnumAttribute : Attribute
    {
        private readonly object linkedEnum;

        /// <summary>
        /// <paramref name="otherEnum"/> должен быть enum'ом.
        /// </summary>
        /// <param name="otherEnum"></param>
        public LinkedEnumAttribute(object otherEnum)
        {
            Check.ObjectIsNotNull(otherEnum);

            if (!otherEnum.GetType().IsEnum)
                Exceptions.Throw(ErrorMessage.UnsupportedType, "The {0} should be enum".Combine(otherEnum));
            
            linkedEnum = otherEnum;
        }

        /// <summary>
        /// Является ли вложенный объект типом <typeparamref name="TEnum"/>
        /// </summary>
        /// <typeparam name="TEnum"/>
        internal bool IsMatches<TEnum>()
        {
            return linkedEnum is TEnum;
        }

        /// <summary>
        /// Выдает прикрепленый enum'ы
        /// </summary>
        /// <typeparam name="TEnum"/>
        public TEnum GetLinkedEnum<TEnum>()
        {
            if (!IsMatches<TEnum>())
                Exceptions.Throw(ErrorMessage.UnsupportedType, "The type {0} is not derived from {1}".Combine(typeof(TEnum), linkedEnum.GetType()));

            return (TEnum)linkedEnum;
        }
    }
}
