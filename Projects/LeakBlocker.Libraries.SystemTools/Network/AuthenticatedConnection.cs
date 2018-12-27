using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.SystemTools.Win32;
using LeakBlocker.Libraries.SystemTools.Win32.ResourceWrappers;
using LeakBlocker.Libraries.SystemTools.Win32.UnmanagedTypes;

namespace LeakBlocker.Libraries.SystemTools.Network
{
    /// <summary>
    /// Establishes connection between current and specified computers that can allow to perform some management tasks on a remote machine.
    /// </summary>
    internal sealed class AuthenticatedConnection : Disposable
    {
        private readonly string computer;
        private readonly string resource;

        private readonly bool impersonated;

        private readonly string user;

        /// <summary>
        /// Creates a connection.
        /// </summary>
        /// <param name="options">Target system access options.</param>
        /// <param name="resource">Resource name. For example shared folder.</param>
        internal AuthenticatedConnection(SystemAccessOptions options = default(SystemAccessOptions), string resource = @"IPC$")
        {
            Check.StringIsMeaningful(resource, "resource");

            if (options == default(SystemAccessOptions))
                return;

            computer = options.SystemName;
            this.resource = resource;

            impersonated = Impersonate(options);

            if (impersonated || (computer == null))
                return;

            Reset(computer, resource);
            Establish(computer, options, resource);

            user = options.UserName;
        }

        /// <summary>
        /// Releases resources.
        /// </summary>
        protected override void DisposeUnmanaged()
        {
            if (impersonated)
            {
                NativeMethods.RevertToSelf();
                return;
            }

            if (computer == null)
                return;

            Reset(computer, resource);
        }

        protected override void DisposedAfterException()
        {
            Log.Write("An exception was thrown during the remote session (Machine: {0}, Resource: {1}, User: {2}).", computer, resource, user);
        }

        private static void Reset(string computerName, string resource = @"IPC$")
        {
            string name = @"\\" + computerName + @"\" + resource;
            uint error = NativeMethods.WNetCancelConnection2(name, 0, true);
            if ((error != NativeMethods.ERROR_SUCCESS) && (error != NativeMethods.ERROR_NOT_CONNECTED))
                Log.Write(NativeErrors.GetMessage("WNetCancelConnection2", error, name));

            error = NativeMethods.WNetCancelConnection2(name, NativeMethods.CONNECT_UPDATE_PROFILE, true);
            if ((error != NativeMethods.ERROR_SUCCESS) && (error != NativeMethods.ERROR_NOT_CONNECTED))
                Log.Write(NativeErrors.GetMessage("WNetCancelConnection2", error, name));

            using (var enumerationHandle = new UnmanagedPointer())
            {
                error = NativeMethods.WNetOpenEnum(NativeMethods.RESOURCE_CONNECTED, NativeMethods.RESOURCETYPE_ANY, 0, IntPtr.Zero, +enumerationHandle);
                if (error != NativeMethods.NO_ERROR)
                    NativeErrors.ThrowException("WnetOpenEnum", error);

                using (new WNetEnumerationHandle(enumerationHandle))
                using (var testBuffer = new UnmanagedMemory(1))
                using (var requiredSize = new UnmanagedInteger())
                using (var requestedEntries = new UnmanagedInteger())
                {
                    while (true)
                    {
                        requiredSize.UValue = testBuffer.USize;
                        requestedEntries.SValue = -1;

                        error = NativeMethods.WNetEnumResource(enumerationHandle, +requestedEntries, +testBuffer, +requiredSize);
                        if (error == NativeMethods.ERROR_NO_MORE_ITEMS)
                            break;
                        if (error != NativeMethods.ERROR_MORE_DATA)
                            NativeErrors.ThrowException("WNetEnumResource", error);

                        using (var dataBuffer = new UnmanagedMemory(requiredSize))
                        {
                            requestedEntries.SValue = -1;
                            error = NativeMethods.WNetEnumResource(enumerationHandle, +requestedEntries, +dataBuffer, +requiredSize);
                            if (error == NativeMethods.ERROR_NO_MORE_ITEMS)
                                continue;
                            if (error != NativeMethods.NO_ERROR)
                                NativeErrors.ThrowException("WNetEnumResource", error);

                            using (var resources = new UnmanagedArray<NativeMethods.NETRESOURCE>(+dataBuffer, requestedEntries))
                            {
                                foreach (NativeMethods.NETRESOURCE currentItem in resources)
                                {
                                    name = StringTools.FromPointer(currentItem.lpRemoteName);
                                    if ((name == null) || !name.Substring(2).StartsWith(computerName, StringComparison.OrdinalIgnoreCase))
                                        continue;

                                    error = NativeMethods.WNetCancelConnection2(name, 0, true);
                                    if ((error != NativeMethods.ERROR_SUCCESS) && (error != NativeMethods.ERROR_NOT_CONNECTED))
                                        Log.Write(NativeErrors.GetMessage("WNetCancelConnection2", error, name));

                                    error = NativeMethods.WNetCancelConnection2(name, NativeMethods.CONNECT_UPDATE_PROFILE, true);
                                    if ((error != NativeMethods.ERROR_SUCCESS) && (error != NativeMethods.ERROR_NOT_CONNECTED))
                                        Log.Write(NativeErrors.GetMessage("WNetCancelConnection2", error, name));
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void Establish(string computerName, SystemAccessOptions options = default(SystemAccessOptions), string resource = @"IPC$")
        {
            using (var resourceBuffer = new UnmanagedStructure<NativeMethods.NETRESOURCE>())
            using (var resourceNameBuffer = new UnmanagedUnicodeString(@"\\" + computerName + @"\" + resource))
            {
                resourceBuffer.Value = new NativeMethods.NETRESOURCE
                {
                    lpRemoteName = +resourceNameBuffer
                };

                uint error = NativeMethods.WNetAddConnection2(+resourceBuffer, options.Password, options.UserName, NativeMethods.CONNECT_TEMPORARY);
                if (error != 0)
                    NativeErrors.ThrowException("WNetAddConnection2", error, resourceNameBuffer.Value);
            }
        }

        private static bool Impersonate(SystemAccessOptions options = default(SystemAccessOptions))
        {
            using (var token = new UnmanagedPointer())
            {
                if (!NativeMethods.LogonUser(options.ShortUserName, options.DomainName, options.Password, NativeMethods.LOGON32_LOGON_INTERACTIVE, NativeMethods.LOGON32_PROVIDER_DEFAULT, +token))
                    return false;

                using (new NativeHandleWrapper(token))
                {
                    return NativeMethods.ImpersonateLoggedOnUser(token);
                }
            }
        }

        /// <summary>
        /// Checks if the specified computer is accessible.
        /// </summary>
        /// <param name="computerName">Target computer. </param>
        /// <returns>True if remote computer is accessible and connection can be established.</returns>
        internal static bool CanBeEstablished(string computerName)
        {
            if (string.IsNullOrEmpty(computerName))
                return true;

            using (var resourceBuffer = new UnmanagedStructure<NativeMethods.NETRESOURCE>())
            using (var resourceNameBuffer = new UnmanagedUnicodeString(@"\\" + computerName + @"\IPC$"))
            {
                resourceBuffer.Value = new NativeMethods.NETRESOURCE
                {
                    lpRemoteName = +resourceNameBuffer
                };

                uint error = NativeMethods.WNetAddConnection2(+resourceBuffer, null, null, NativeMethods.CONNECT_TEMPORARY);

                switch (error)
                {
                    case 0:
                        Reset(computerName);
                        return true;
                    case NativeMethods.ERROR_ACCESS_DENIED:
                    case NativeMethods.ERROR_LOGON_FAILURE:
                    case NativeMethods.ERROR_BAD_USERNAME:
                    case NativeMethods.ERROR_NOT_LOGGED_ON:
                    case NativeMethods.ERROR_TRUST_FAILURE:
                    case NativeMethods.ERROR_NETLOGON_NOT_STARTED:
                    case NativeMethods.ERROR_CONNECTED_OTHER_PASSWORD:
                    case NativeMethods.ERROR_CONNECTED_OTHER_PASSWORD_DEFAULT:
                        return true;
                    //case NativeMethods.ERROR_PORT_UNREACHABLE:
                    //case NativeMethods.ERROR_HOST_DOWN:

                    //case NativeMethods.ERROR_BAD_NET_NAME:
                    //case NativeMethods.ERROR_BAD_NETPATH:
                    //case NativeMethods.ERROR_INVALID_NETNAME:
                    //case NativeMethods.ERROR_NO_NET_OR_BAD_PATH:
                    //case NativeMethods.ERROR_NO_NETWORK:
                    //case NativeMethods.ERROR_DEV_NOT_EXIST:
                    //case NativeMethods.ERROR_NETNAME_DELETED:
                    //case NativeMethods.ERROR_CONNECTION_REFUSED:
                    //case NativeMethods.ERROR_NETWORK_UNREACHABLE:
                    //case NativeMethods.ERROR_HOST_UNREACHABLE:
                    //case NativeMethods.ERROR_PROTOCOL_UNREACHABLE:

                    //case NativeMethods.ERROR_ALREADY_ASSIGNED:
                    //case NativeMethods.ERROR_BAD_DEV_TYPE:
                    //case NativeMethods.ERROR_BAD_DEVICE:
                    //case NativeMethods.ERROR_BAD_PROFILE:
                    //case NativeMethods.ERROR_BAD_PROVIDER:
                    //case NativeMethods.ERROR_BUSY:
                    //case NativeMethods.ERROR_CANCELLED:
                    //case NativeMethods.ERROR_CANNOT_OPEN_PROFILE:
                    //case NativeMethods.ERROR_DEVICE_ALREADY_REMEMBERED:
                    //case NativeMethods.ERROR_EXTENDED_ERROR:
                    //case NativeMethods.ERROR_INVALID_ADDRESS:
                    //case NativeMethods.ERROR_INVALID_PARAMETER:
                    default:
                        Log.Write("Authenticated connection check result for computer {0} is {1}.", computerName, error);
                        return false;
                }
            }
        }
    }
}
   