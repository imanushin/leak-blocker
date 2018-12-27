using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common
{
    /// <summary>
    /// Executes task in loop in a separate thread.
    /// </summary>
    public interface IScheduler : IDisposable
    {
        /// <summary>
        /// Starts executing the task immediately. Interval countdown will also be reset.
        /// </summary>
        void RunNow();
    }
}