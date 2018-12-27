using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.SystemTools.Drivers;
using LeakBlocker.Libraries.SystemTools.Win32;
using LeakBlocker.Libraries.SystemTools.Win32.ResourceWrappers;
using LeakBlocker.Libraries.SystemTools.Win32.UnmanagedTypes;

namespace LeakBlocker.Libraries.SystemTools.Entities
{
    /// <summary>
    /// Network name that represents either computer or domain. By default it is meant to be a DNS name.
    /// </summary>
    public sealed class NetworkName
    {
        private readonly string name;

        /// <summary>
        /// Creates an instance.
        /// </summary>
        public NetworkName(string name)
        {
            Check.StringIsMeaningful(name, "name");

            this.name = name;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <param name="value">Class instance.</param>
        /// <returns>A string that represents the current object.</returns>
        public static implicit operator string(NetworkName value)
        {
            return (value != null) ? value.name : null;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current System.Object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified System.Object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            var volumeName = obj as NetworkName;
            return (volumeName != null) && (name.ToUpperInvariant() == volumeName.name.ToUpperInvariant());
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return name.ToUpperInvariant().GetHashCode();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return name;
        }
    }
}
