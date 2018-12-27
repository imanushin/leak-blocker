using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.SystemTools.Drivers;
using LeakBlocker.Libraries.SystemTools.Win32;
using LeakBlocker.Libraries.SystemTools.Win32.ResourceWrappers;
using LeakBlocker.Libraries.SystemTools.Win32.UnmanagedTypes;

namespace LeakBlocker.Libraries.SystemTools.Devices
{
    /// <summary>
    /// File system volume name.
    /// </summary>
    public sealed class VolumeName
    {
        private readonly string name;

        internal VolumeName(UnmanagedStructure<NativeMethods.INSTANCE_FULL_INFORMATION> data)
        {
            Check.ObjectIsNotNull(data, "data");

            name = StringTools.FromPointer(+data + data.Value.VolumeNameBufferOffset, data.Value.VolumeNameLength / 2);
        }

        internal VolumeName(UnmanagedStructure<DriverStructures.VOLUME_IDENTIFIER_NOTIFICATION> data)
        {
            Check.ObjectIsNotNull(data, "data");

            using (var nameString = new UnmanagedUnicodeString(+data + data.SSize, data.Value.VolumeName.StringLength))
            {
                name = nameString.Value;
            }
        }

        internal VolumeName(UnmanagedStructure<DriverStructures.FILE_ACCESS_NOTIFICATION> data)
        {
            Check.ObjectIsNotNull(data, "data");

            using (var nameString = new UnmanagedUnicodeString(+data + data.SSize, data.Value.FileName.VolumeNameLength))
            {
                name = nameString.Value.TrimEnd('\\');
            }
        }

        private VolumeName(string name)
        {
            this.name = name ?? string.Empty;
        }

        internal static ReadOnlySet<VolumeName> Enumerate()
        {
            var volumes = new List<string>();

            using (var requiredSize = new UnmanagedInteger())
            using (var searchHandle = new UnmanagedPointer())
            {
                uint error = NativeMethods.FilterVolumeFindFirst(NativeMethods.FILTER_VOLUME_INFORMATION_CLASS.FilterVolumeBasicInformation, IntPtr.Zero, 0, +requiredSize, +searchHandle);
                if (error == NativeMethods.HRESULT_FROM_WIN32(NativeMethods.ERROR_NO_MORE_ITEMS))
                    return ReadOnlySet<VolumeName>.Empty;
                if (error != NativeMethods.HRESULT_FROM_WIN32(NativeMethods.ERROR_INSUFFICIENT_BUFFER))
                    NativeErrors.ThrowException("FilterVolumeFindFirst", error);

                using (var firstEntryBuffer = new UnmanagedMemory(requiredSize))
                using (var data = new UnmanagedStructure<NativeMethods.FILTER_VOLUME_BASIC_INFORMATION>(+firstEntryBuffer))
                {
                    error = NativeMethods.FilterVolumeFindFirst(NativeMethods.FILTER_VOLUME_INFORMATION_CLASS.FilterVolumeBasicInformation, +firstEntryBuffer, firstEntryBuffer.USize, +requiredSize, +searchHandle);
                    if (error != NativeMethods.S_OK)
                        NativeErrors.ThrowException("FilterVolumeFindFirst", error);

                    string volume = StringTools.FromPointer(data.GetFieldAddress("FilterVolumeName"), data.Value.FilterVolumeNameLength / 2);
                    volumes.Add(volume);
                }

                using (new FilterVolumeSearchHandleWrapper(searchHandle))
                {
                    while (true)
                    {
                        error = NativeMethods.FilterVolumeFindNext(searchHandle, NativeMethods.FILTER_VOLUME_INFORMATION_CLASS.FilterVolumeBasicInformation, IntPtr.Zero, 0, +requiredSize);
                        if (error == NativeMethods.HRESULT_FROM_WIN32(NativeMethods.ERROR_NO_MORE_ITEMS))
                            break;

                        if (error != NativeMethods.HRESULT_FROM_WIN32(NativeMethods.ERROR_INSUFFICIENT_BUFFER))
                            NativeErrors.ThrowException("FilterVolumeFindNext", error);

                        using (var nextEntryBuffer = new UnmanagedMemory(requiredSize))
                        using (var data = new UnmanagedStructure<NativeMethods.FILTER_VOLUME_BASIC_INFORMATION>(+nextEntryBuffer))
                        {
                            error = NativeMethods.FilterVolumeFindNext(searchHandle, NativeMethods.FILTER_VOLUME_INFORMATION_CLASS.FilterVolumeBasicInformation, +nextEntryBuffer, nextEntryBuffer.USize, +requiredSize);
                            if (error != NativeMethods.S_OK)
                                NativeErrors.ThrowException("FilterVolumeFindNext", error);

                            string volume = StringTools.FromPointer(data.GetFieldAddress("FilterVolumeName"), data.Value.FilterVolumeNameLength / 2);
                            volumes.Add(volume);
                        }
                    }
                }
            }

            return volumes.Select(name => new VolumeName(name)).ToReadOnlySet();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <param name="value">Class instance.</param>
        /// <returns>A string that represents the current object.</returns>
        public static implicit operator string(VolumeName value)
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
            var volumeName = obj as VolumeName;
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
