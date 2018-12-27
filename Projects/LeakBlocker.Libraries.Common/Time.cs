using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common
{
    /// <summary>
    /// Более корректное время, которое может храниться в базе данных и всегда имеет корректную локаль.
    /// Также оно более точное, по сравнению с <see cref="DateTime"/>
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    public sealed class Time : BaseReadOnlyObject
    {
        #region Unknown

        private static readonly Time unknown = new Time(0);

        /// <summary>
        /// Default value. Means undetermined time.
        /// </summary>
        public static Time Unknown
        {
            get
            {
                return unknown;
            }
        }

        #endregion

        /// <summary>
        /// Конструктор. Принимает количество 100-наносекундных интервалов от 1 января 0001 года.
        /// Отрицательные значения недопустимы
        /// </summary>
        public Time(long ticks)
        {
            Ticks = ticks;
        }

        /// <summary>
        /// Берет на вход <see cref="DateTime"/>. Далее всегда идет преобразование ко времени UTC.
        /// </summary>
        public Time(DateTime time)
            : this(time.ToUniversalTime().Ticks)
        {
        }

        /// <summary>
        /// Количество 100-наносекундных интервалов от 1 января 0001 года.
        /// </summary>
        [DataMember]
        public long Ticks
        {
            get;
            private set;
        }

        /// <summary>
        /// Преобразует структуру в UTC DateTime
        /// </summary>
        public DateTime ToUtcDateTime()
        {
            return new DateTime(Ticks, DateTimeKind.Utc);
        }

        /// <summary>
        /// Преобразует структуру в локальное время
        /// </summary>
        public DateTime ToLocalTime()
        {
            return ToUtcDateTime().ToLocalTime();
        }

        /// <summary>
        /// Текущее время (+ огрубление из-за неточности объекта <see cref="DateTime"/>)
        /// </summary>
        public static Time Now
        {
            get
            {
                return new Time(DateTime.UtcNow);
            }
        }

        #region Equality

        /// <summary>
        /// Сравнение с другим таким же объектом
        /// </summary>
        protected override int CustomCompare(object target)
        {
            return Ticks.CompareTo(((Time)target).Ticks);
        }

        /// <summary>
        /// Возвращает все вложенные объекты, которые имеют смысл для вычисления хеш-кода и метода Equals
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return Ticks;
        }

        #endregion

        /// <summary>
        /// Converts current instance to string.
        /// </summary>
        protected override string GetString()
        {
            return new DateTime(Ticks, DateTimeKind.Utc).ToString(CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Добавляет <paramref name="shift"/> к текущему времени и выдает полученный объект
        /// </summary>
        public Time Add(TimeSpan shift)
        {
            return new Time(new DateTime(Ticks, DateTimeKind.Utc).Add(shift));
        }
    }
}
