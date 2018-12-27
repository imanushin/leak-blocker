using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common
{
    /// <summary>
    /// Позволяет подавлять все исключения в вызываемой функции.
    /// Исключения записываются в лог
    /// </summary>
    public interface IExceptionSuppressor
    {
        /// <summary>
        /// Запуск функции без параметров
        /// </summary>
        void Run(Action action);

        /// <summary>
        /// Запуск функции с параметром
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        void Run<T1>(Action<T1> action, [CanBeNull] T1 argument);

        /// <summary>
        /// Запуск функции с двумя параметрами
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        void Run<T1, T2>(Action<T1, T2> action, [CanBeNull] T1 argument1, [CanBeNull] T2 argument2);

        /// <summary>
        /// Запуск функции с тремя параметрами
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        void Run<T1, T2, T3>(Action<T1, T2, T3> action, [CanBeNull] T1 argument1, [CanBeNull] T2 argument2, [CanBeNull] T3 argument3);

        /// <summary>
        /// Запуск функции с 4 параметрами
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        void Run<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action, [CanBeNull] T1 argument1, [CanBeNull] T2 argument2, [CanBeNull] T3 argument3, [CanBeNull] T4 argument4);

        /// <summary>
        /// Запуск функции с 5 параметрами
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        void Run<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> action, [CanBeNull] T1 argument1, [CanBeNull] T2 argument2, [CanBeNull] T3 argument3, [CanBeNull] T4 argument4, [CanBeNull] T5 argument5);
    }
}
