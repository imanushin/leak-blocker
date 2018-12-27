using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.SystemTools
{
    /// <summary>
    /// Represents global flag that can be used from multiple processes. Flag is persistent until systeem reboot.
    /// </summary>
    public interface IGlobalFlag
    {
        /// <summary>
        /// True if the flag exists.
        /// </summary>
        bool Exists
        {
            get;
        }

        /// <summary>
        /// Creates the flag.
        /// </summary>
        void Create();

        /// <summary>
        /// Removes the flag.
        /// </summary>
        void Delete();
    }
}
