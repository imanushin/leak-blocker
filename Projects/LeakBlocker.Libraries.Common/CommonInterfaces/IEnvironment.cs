using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;

namespace LeakBlocker.Libraries.Common.CommonInterfaces
{
    /// <summary>
    /// Wrapper for the System.Environment class.
    /// </summary>
    public interface IEnvironment
    {
        /// <summary>
        /// Returns an array of command line arguments of the current process.
        /// </summary>
        /// <returns>Collection of arguments.</returns>
        ReadOnlyList<string> CommandLineArguments
        {
            get;
        }
    }
}
