using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;

namespace LeakBlocker.Libraries.Common.CommonInterfaces
{
    /// <summary>
    /// Class simplified checking command line arguments.
    /// </summary>
    public interface ICommandLine
    {
        /// <summary>
        /// Checks if command line arguments list contains specified parameter. Comparison is case-insensitive.
        /// </summary>
        /// <param name="parameter">Parameter name.</param>
        /// <returns>Returns true if arguments list contains specified parameter.</returns>
        bool Contains(string parameter);

        /// <summary>
        /// Returns parameter following after the specified key parameter. For example command line arguments 
        /// are -p "c:\1.txt". In this case specifying -p as a key parameter will lead to returning c:\1.txt.
        /// </summary>
        /// <param name="keyParameter">Key parameter name.</param>
        /// <returns>Returns subsequent parameter.</returns>
        string GetValue(string keyParameter);

        /// <summary>
        /// Returns true if there is another parameter (value) after the specified key parameter. 
        /// </summary>
        /// <param name="keyParameter">Key parameter name.</param>
        /// <returns>Returns true if parameter has value.</returns>
        bool HasValue(string keyParameter);

        /// <summary>
        /// Tries to get parameter value.
        /// </summary>
        /// <param name="keyParameter">Parameter name.</param>
        /// <param name="value">Parameter value.</param>
        /// <returns>Returns true if parameter has value.</returns>
        bool TryGetValue(string keyParameter, out string value);

        /// <summary>
        /// Creates a command line from the specified arguments.
        /// </summary>
        /// <param name="executable">Executable path.</param>
        /// <param name="arguments">Arguments.</param>
        /// <returns>Command line string.</returns>
        string Create(string executable, params string[] arguments);

        /// <summary>
        /// Creates a command line from the specified arguments.
        /// </summary>
        /// <param name="executable">Executable path.</param>
        /// <param name="arguments">Arguments.</param>
        /// <returns>Command line string.</returns>
        string Create(string executable, IList<string> arguments);

        /// <summary>
        /// Creates arguments string (for starting processes).
        /// </summary>
        /// <param name="arguments">Arguments.</param>
        /// <returns>Arguments string.</returns>
        string CreateArguments(params string[] arguments);
        
        /// <summary>
        /// Creates arguments string (for starting processes).
        /// </summary>
        /// <param name="arguments">Arguments.</param>
        /// <returns>Arguments string.</returns>
        string CreateArguments(IList<string> arguments);

        /// <summary>
        /// Splits the command line into argument strings.
        /// </summary>
        /// <param name="commandLine">Full command line string.</param>
        /// <returns>List of arguments.</returns>
        ReadOnlyList<string> Split(string commandLine);
    }
}
