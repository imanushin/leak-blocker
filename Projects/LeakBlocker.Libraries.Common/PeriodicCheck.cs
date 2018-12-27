using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common
{
    /// <summary>
    /// Periodically runs an actions.
    /// </summary>
    public static class PeriodicCheck
    {
        /// <summary>
        /// Synchronously runs the specified function until it returns true or the timeout expires.
        /// </summary>
        /// <param name="action">Function.</param>
        /// <param name="interval">Interval between running the action.</param>
        /// <param name="timeout">Maximal wait time.</param>
        /// <returns>True if the condition was fulfilled.</returns>
        public static bool WaitUntilSuccess(Func<bool> action, TimeSpan interval = default(TimeSpan), TimeSpan timeout = default(TimeSpan))
        {
            Check.ObjectIsNotNull(action, "action");

            TimeSpan counter = (timeout == default(TimeSpan)) ? TimeSpan.MaxValue : timeout;

            while (counter >= default(TimeSpan))
            {
                if (action())
                    return true;

                counter = counter - interval;

                Thread.Sleep(interval.ConvertToTimeout());
            }

            return false;
        }
    }
}
