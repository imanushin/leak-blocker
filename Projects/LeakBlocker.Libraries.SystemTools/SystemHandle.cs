using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.SystemTools.Win32;
using LeakBlocker.Libraries.SystemTools.Win32.ResourceWrappers;
using LeakBlocker.Libraries.SystemTools.Win32.UnmanagedTypes;

namespace LeakBlocker.Libraries.SystemTools
{
    /// <summary>
    /// Describes windows object handle.
    /// </summary>
    public sealed class SystemHandle
    {
        private readonly NativeMethods.SYSTEM_HANDLE_TABLE_ENTRY_INFO_EX data;
        private readonly Lazy<string> objectName;

        /// <summary>
        /// Target object name or empty string if name cannot be retreived.
        /// </summary>
        public string ObjectName
        {
            get
            {
                return objectName.Value;
            }
        }

        /// <summary>
        /// Identifier of a process that owns this handle.
        /// </summary>
        public int ProcessIdentifier
        {
            get
            {
                return unchecked((int)data.UniqueProcessId.ToInt64());
            }
        }

        /// <summary>
        /// Closes the current handle. Owner process will not be able to access it anymore.
        /// </summary>
        public void Close()
        {
            CloseHandle(data);
        }

        /// <summary>
        /// Enumerates almost all handles in the system. Some handles cannot be retreived.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SystemHandle> EnumerateSystemHandles()
        {
            return EnumerateHandles().Select(data => new SystemHandle(data));
        }

        #region Type cast

        /// <summary>
        /// Returns handle value.
        /// </summary>
        /// <param name="value">Handle.</param>
        /// <returns>Native handle value.</returns>
        public static implicit operator IntPtr(SystemHandle value)
        {
            Check.ObjectIsNotNull(value, "value");
            return value.ToIntPtr();
        }

        /// <summary>
        /// Converts to native handle value.
        /// </summary>
        /// <returns>Native handle value.</returns>
        public IntPtr ToIntPtr()
        {
            return data.HandleValue;
        }

        #endregion

        private SystemHandle(NativeMethods.SYSTEM_HANDLE_TABLE_ENTRY_INFO_EX data)
        {
            this.data = data;
            objectName = new Lazy<string>(() => QueryObjectName(data) ?? string.Empty);
        }

        private static IEnumerable<NativeMethods.SYSTEM_HANDLE_TABLE_ENTRY_INFO_EX> EnumerateHandles()
        {
            uint allocationSize = 128 * 1024;
            while (true)
            {
                using (var data = new UnmanagedMemory(allocationSize))
                using (var size = new UnmanagedInteger())
                {
                    uint error = NativeMethods.NtQuerySystemInformation(NativeMethods.SYSTEM_INFORMATION_CLASS.SystemExtendedHandleInformation, +data, data.USize, +size);
                    if (error == NativeMethods.STATUS_INFO_LENGTH_MISMATCH)
                    {
                        allocationSize *= 2;
                        if (allocationSize >= 32 * 1024 * 1024)
                            Exceptions.Throw(ErrorMessage.NotSupported, "Allocation size too much. Method is not supported.");

                        continue;
                    }
                    if (error != NativeMethods.STATUS_SUCCESS)
                        NativeErrors.ThrowException("NtQuerySystemInformation", error);

                    using (var handleInformation = new UnmanagedStructure<NativeMethods.SYSTEM_EXTENDED_HANDLE_INFORMATION>(+data))
                    {
                        int offset = UnmanagedStructure<NativeMethods.SYSTEM_EXTENDED_HANDLE_INFORMATION>.GetFieldOffset("Handles");
                        int calculatedSize = (int)handleInformation.Value.NumberOfHandles * (int)UnmanagedStructure<NativeMethods.SYSTEM_HANDLE_TABLE_ENTRY_INFO_EX>.GetSize() + offset;

                        if (calculatedSize != size)
                            Exceptions.Throw(ErrorMessage.NotSupported, "Calculated size mismatch. Method is not supported.");

                        using (var handles = new UnmanagedArray<NativeMethods.SYSTEM_HANDLE_TABLE_ENTRY_INFO_EX>(+data + offset, (uint)handleInformation.Value.NumberOfHandles))
                        {
                            return handles.ToList();
                        }
                    }
                }
            }
        }

        private static string QueryObjectName(NativeMethods.SYSTEM_HANDLE_TABLE_ENTRY_INFO_EX handleInformation)
        {
            if ((handleInformation.GrantedAccess == 0x0012019f) || (handleInformation.GrantedAccess == 0x001a019f) ||
                (handleInformation.GrantedAccess == 0x00120189) || (handleInformation.GrantedAccess == 0x00100000))
            {
                return string.Empty;
            }

            IntPtr processHandle = NativeMethods.OpenProcess(NativeMethods.PROCESS_DUP_HANDLE, false, unchecked((uint)handleInformation.UniqueProcessId.ToInt64()));
            if (processHandle == IntPtr.Zero)
            {
                uint error = NativeMethods.GetLastError();
                if (error == NativeMethods.ERROR_ACCESS_DENIED)
                    return string.Empty;
                if (error == NativeMethods.ERROR_INVALID_PARAMETER)
                    return string.Empty;

                NativeErrors.ThrowLastErrorException("OpenProcess", handleInformation.UniqueProcessId);
            }

            using (new NativeHandleWrapper(processHandle))
            using (var duplicatedHandle = new UnmanagedPointer())
            {
                if (!NativeMethods.DuplicateHandle(processHandle, handleInformation.HandleValue, NativeMethods.GetCurrentProcess(), +duplicatedHandle, 0, false, NativeMethods.DUPLICATE_SAME_ACCESS))
                {
                    uint error = NativeMethods.GetLastError();
                    if ((error == NativeMethods.ERROR_INVALID_PARAMETER) || (error == NativeMethods.ERROR_INVALID_HANDLE) ||
                        (error == NativeMethods.ERROR_NOT_SUPPORTED) || (error == NativeMethods.ERROR_ACCESS_DENIED))
                    {
                        return string.Empty;
                    }

                    NativeErrors.ThrowLastErrorException("DuplicateHandle");
                }

                using (new NativeHandleWrapper(duplicatedHandle))
                {
                    uint allocationSize = 1024;
                    while (true)
                    {
                        using (var data = new UnmanagedMemory(allocationSize))
                        using (var size = new UnmanagedInteger())
                        {
                            uint error = NativeMethods.NtQueryObject(duplicatedHandle, NativeMethods.OBJECT_INFORMATION_CLASS.ObjectNameInformation, +data, data.USize, +size);
                            if (error == NativeMethods.STATUS_INFO_LENGTH_MISMATCH)
                            {
                                allocationSize *= 2;
                                if (allocationSize >= 1024 * 1024)
                                    Exceptions.Throw(ErrorMessage.NotSupported, "Allocation size too much. Method is not supported.");

                                continue;
                            }
                            if ((error == NativeMethods.STATUS_NOT_SUPPORTED) || (error == NativeMethods.STATUS_OBJECT_PATH_INVALID))
                                return string.Empty;

                            if (error != NativeMethods.STATUS_SUCCESS)
                                NativeErrors.ThrowException("NtQuerySystemInformation", error);

                            using (var nameData = new UnmanagedStructure<NativeMethods.OBJECT_NAME_INFORMATION>(+data))
                            {
                                return StringTools.FromUnicodeString(nameData.Value.Name, true) ?? string.Empty;
                            }
                        }
                    }
                }
            }
        }

        private static void CloseHandle(NativeMethods.SYSTEM_HANDLE_TABLE_ENTRY_INFO_EX handleInformation)
        {
            IntPtr processHandle = NativeMethods.OpenProcess(NativeMethods.PROCESS_DUP_HANDLE, false, unchecked((uint)handleInformation.UniqueProcessId.ToInt64()));
            if (processHandle == IntPtr.Zero)
            {
                uint error = NativeMethods.GetLastError();
                if (error == NativeMethods.ERROR_INVALID_PARAMETER)
                    Log.Write(NativeErrors.GetMessage("OpenProcess", error, handleInformation.UniqueProcessId));
                else
                    NativeErrors.ThrowException("OpenProcess", error);
            }
            using (new NativeHandleWrapper(processHandle))
            using (var duplicatedHandle = new UnmanagedPointer())
            {
                if (!NativeMethods.DuplicateHandle(processHandle, handleInformation.HandleValue, NativeMethods.GetCurrentProcess(), +duplicatedHandle, 0, false, NativeMethods.DUPLICATE_SAME_ACCESS | NativeMethods.DUPLICATE_CLOSE_SOURCE))
                    NativeErrors.ThrowLastErrorException("DuplicateHandle");

                using (new NativeHandleWrapper(duplicatedHandle))
                {
                }
            }
        }
    }
}
