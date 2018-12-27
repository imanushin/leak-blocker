using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common
{
    /// <summary>
    /// Интерфейс для упрощенного запуска задач в фоновом потоке.
    /// </summary>
    public interface IAsyncInvoker
    {
        /// <summary>
        /// Запускает действие без параметров в фоновом потоке.
        /// </summary>
        IWaitHandle Invoke(Action action);

        /// <summary>
        /// Запускает действие с одним параметром в фоновом потоке.
        /// </summary>
        IWaitHandle Invoke<T>(Action<T> action, T argument);

        /// <summary>
        /// Запускает действие с двумя параметрами в фоновом потоке.
        /// </summary>
        IWaitHandle Invoke<T1, T2>(Action<T1, T2> action, T1 argument1, T2 argument2);

        /// <summary>
        /// Запускает действие с тремя параметрами в фоновом потоке.
        /// </summary>
        IWaitHandle Invoke<T1, T2, T3>(Action<T1, T2, T3> action, T1 argument1, T2 argument2, T3 argument3); 
    }
}
