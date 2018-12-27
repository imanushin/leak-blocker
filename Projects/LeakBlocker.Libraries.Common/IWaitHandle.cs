using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common
{
    /// <summary>
    /// Интерфейс для ожидания окончания выполнения.
    /// </summary>
    public interface IWaitHandle
    {
        /// <summary>
        /// Ждать окончания выполнения.
        /// Если в процессе работы было брошено исключение, то оно будет проброшено из этого метода
        /// </summary>
        void Wait();

        /// <summary>
        /// Ждать окончания выполнения.
        /// Если в процессе работы было брошено исключение, то оно будет проброшено из этого метода
        /// </summary>
        /// <param name="timeout">Время в течение которого ожидать окончания.</param>
        /// <returns>True если выполнение было завершено.</returns>
        bool Wait(TimeSpan timeout);

        /// <summary>
        /// Останавливает поток, если он работает
        /// </summary>
        void Abort();
    }
}
