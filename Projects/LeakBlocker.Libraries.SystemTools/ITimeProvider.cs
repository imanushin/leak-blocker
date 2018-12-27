using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.SystemTools
{
    /// <summary>
    /// Low-level time functions.
    /// </summary>
    public interface ITimeProvider
    {
        /// <summary>
        /// Current time wwith higher precision than Time.Now.
        /// </summary>
        Time CurrentTime
        {
            get;
        }

        /// <summary>
        /// How long system is working. This counter is valid only first 48 days.
        /// </summary>
        TimeSpan SystemUptime
        {
            get;
        }
    }
}
