using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.SystemTools
{
    /// <summary>
    /// Secret storage that allows storing small amounts of data.
    /// </summary>
    public interface IPrivateRegistryStorage
    {
        /// <summary>
        /// Time when the storage was created.
        /// </summary>
        Time InstallDate
        {
            get;
        }

        /// <summary>
        /// Gets the value from the storage.
        /// </summary>
        /// <param name="key">Value key.</param>
        /// <returns>Value or empty string if the value does not exist.</returns>
        string GetValue(string key);

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="key">Value key.</param>
        /// <param name="value">Data.</param>
        void SetValue(string key, string value);

        /// <summary>
        /// Removes the value from the storage.
        /// </summary>
        /// <param name="key">Value key.</param>
        void DeleteValue(string key);
    }
}
