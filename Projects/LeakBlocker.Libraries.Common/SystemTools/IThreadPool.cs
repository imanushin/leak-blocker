using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;

namespace LeakBlocker.Libraries.Common.SystemTools
{
    /// <summary>
    /// Пул потоков
    /// </summary>
    public interface IThreadPool : IDisposable
    {
        /// <summary>
        /// Добавляет функцию в очередь.
        /// </summary>
        void EnqueueAction(Action action);

        /// <summary>
        /// Запускает функции и ожидает их завершения. Если были исключения, то они будут проброшены с помощью <see cref="AggregateException"/>
        /// </summary>
        void RunAndWait(IReadOnlyCollection<Action> actions);
    }
}
