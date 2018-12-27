using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common
{
    /// <summary>
    /// Extension methods for default TimeSpan class.
    /// </summary>
    public static class TimeSpanExtensions
    {
        /// <summary>
        /// Converts the TimeSpan instance to interval in milliseconds that can be safely used in wait methods (by default too
        /// large TimeSpan values can cause exceptions).
        /// </summary>
        /// <param name="timeSpan">The currrent instance.</param>
        /// <returns>Number of milliseconds.</returns>
        public static int ConvertToTimeout(this TimeSpan timeSpan)
        {
            int result = (int)Math.Min(int.MaxValue, (long)timeSpan.TotalMilliseconds);
            return (result == int.MaxValue) ? -1 : result;
        }
    }
}
