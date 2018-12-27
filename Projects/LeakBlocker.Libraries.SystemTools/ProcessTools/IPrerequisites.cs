using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.SystemTools.ProcessTools
{
    /// <summary>
    /// Provides various checks before running processes.
    /// </summary>
    public interface IPrerequisites
    {
        /// <summary>
        /// Checks if the computer can be accessed remotely.
        /// </summary>
        /// <param name="name">Computer name.</param>
        /// <returns>True if computer is turned on.</returns>
        bool ComputerIsAccessible(string name);

        /// <summary>
        /// Checks if firewall is active on the remote host.
        /// </summary>
        /// <param name="name">Computer name.</param>
        /// <returns>True if firewall blocks required ports.</returns>
        bool FirewallIsActive(string name);

        /// <summary>
        /// Returns the version of the remote operating system.
        /// </summary>
        Version GetRemoteSystemVersion(SystemAccessOptions options);
    }
}
