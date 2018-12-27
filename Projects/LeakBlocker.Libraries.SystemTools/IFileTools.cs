using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;

namespace LeakBlocker.Libraries.SystemTools
{
    /// <summary>
    /// Utilities for filesystem acceess.
    /// </summary>
    public interface IFileTools
    {
        /// <summary>
        /// Removes the current process' directory. Process should exit as soon as possible after this call.
        /// </summary>
        void RemoveCurrentExecutable();

        /// <summary>
        /// Creates a new directory.
        /// </summary>
        /// <param name="folder">Path.</param>
        /// <param name="options">Access options.</param>
        void CreateDirectory(string folder, SystemAccessOptions options = default(SystemAccessOptions));

        /// <summary>
        /// Copies the file.
        /// </summary>
        /// <param name="source">Source path.</param>
        /// <param name="destination">estination path.</param>
        /// <param name="options">Access options.</param>
        void CopyFile(string source, string destination, SystemAccessOptions options = default(SystemAccessOptions));

        /// <summary>
        /// Check if the specified file exists.
        /// </summary>
        /// <param name="path">File path.</param>
        /// <returns>True if the file exists.</returns>
        /// <param name="options">Access options.</param>
        bool Exists(string path, SystemAccessOptions options = default(SystemAccessOptions));

        /// <summary>
        /// Writes the data to the specified file.
        /// </summary>
        /// <param name="path">File path.</param>
        /// <param name="data">Binary data.</param>
        /// <param name="options">Access options.</param>
        void WriteFile(string path, IReadOnlyCollection<byte> data, SystemAccessOptions options = default(SystemAccessOptions));

        /// <summary>
        /// Reads the data to the specified file.
        /// </summary>
        /// <param name="path">File path.</param>
        /// <param name="options">Access options.</param>
        IReadOnlyCollection<byte> ReadFile(string path, SystemAccessOptions options = default(SystemAccessOptions));

        /// <summary>
        /// Removes the specified file.
        /// </summary>
        /// <param name="path">File path.</param>
        /// <param name="options">Access options.</param>
        void Delete(string path, SystemAccessOptions options = default(SystemAccessOptions));
    }
}
