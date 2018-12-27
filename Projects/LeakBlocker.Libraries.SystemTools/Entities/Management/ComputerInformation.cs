using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.SystemTools.Network;
using LeakBlocker.Libraries.SystemTools.Win32;
using LeakBlocker.Libraries.SystemTools.Win32.ResourceWrappers;
using LeakBlocker.Libraries.SystemTools.Win32.UnmanagedTypes;
using Microsoft.Win32;

namespace LeakBlocker.Libraries.SystemTools.Entities.Management
{
    internal static class ComputerInformation
    {
        internal static string GetCurrentComputerName()
        {
            using (var sizeBuffer = new UnmanagedInteger())
            {
                if (!NativeMethods.GetComputerNameEx(NativeMethods.COMPUTER_NAME_FORMAT.ComputerNamePhysicalDnsFullyQualified, IntPtr.Zero, +sizeBuffer))
                {
                    uint error = NativeMethods.GetLastError();
                    if (error != NativeMethods.ERROR_MORE_DATA)
                        NativeErrors.ThrowLastErrorException("GetComputerNameEx");
                }

                using (var stringBuffer = new UnmanagedUnicodeString(sizeBuffer))
                {
                    if (!NativeMethods.GetComputerNameEx(NativeMethods.COMPUTER_NAME_FORMAT.ComputerNamePhysicalDnsFullyQualified, +stringBuffer, +sizeBuffer))
                        NativeErrors.ThrowLastErrorException("GetComputerNameEx");

                    Check.StringIsMeaningful(stringBuffer);
                    return stringBuffer;
                }
            }
        }

        internal static string GetComputerDomainName(string computerName = null, SystemAccessOptions options = default(SystemAccessOptions))
        {
            using (new AuthenticatedConnection(options))
            using (var objectAttributes = new UnmanagedStructure<NativeMethods.LSA_OBJECT_ATTRIBUTES>())
            using (var handle = new UnmanagedPointer())
            using (var data = new UnmanagedPointer())
            using (var systemName = new UnmanagedUnicodeString(computerName))
            using (var systemNameString = new UnmanagedStructure<NativeMethods.LSA_UNICODE_STRING>())
            {
                systemNameString.Value = new NativeMethods.LSA_UNICODE_STRING
                {
                    Buffer = +systemName,
                    Length = (ushort)systemName.USize,
                    MaximumLength = (ushort)systemName.USize,
                };

                uint error = NativeMethods.LsaOpenPolicy(string.IsNullOrEmpty(computerName) ? IntPtr.Zero : +systemNameString, +objectAttributes, NativeMethods.POLICY_VIEW_LOCAL_INFORMATION, +handle);
                if (error != 0)
                    NativeErrors.ThrowException("LsaOpenPolicy", error, computerName);

                using (new LsaHandleWrapper(handle))
                {
                    error = NativeMethods.LsaQueryInformationPolicy(handle, NativeMethods.POLICY_INFORMATION_CLASS.PolicyDnsDomainInformation, +data);
                    if (error != 0)
                        NativeErrors.ThrowException("LsaQueryInformationPolicy", error, computerName);

                    using (new LsaMemoryWrapper(data))
                    using (var domainInformation = new UnmanagedStructure<NativeMethods.POLICY_DNS_DOMAIN_INFO>(data))
                    {
                        return StringTools.FromPointer(domainInformation.Value.DnsDomainName.Buffer);
                    }
                }
            }
        }

        internal static string GetComputerShortDomainName(string computerName, SystemAccessOptions options = default(SystemAccessOptions))
        {
            Check.StringIsMeaningful(computerName, "computerName");

            string result = null;

            using (var pointerBuffer = new UnmanagedPointer())
            using (var typeBuffer = new UnmanagedInteger())
            using (new AuthenticatedConnection(options))
            {
                uint error = NativeMethods.NetGetJoinInformation(computerName, +pointerBuffer, +typeBuffer);
                if (error != NativeMethods.NERR_Success)
                    NativeErrors.ThrowException("NetGetJoinInformation", error);

                using (new NetMemoryWrapper(pointerBuffer))
                {
                    if ((NativeMethods.NETSETUP_JOIN_STATUS)typeBuffer.UValue == NativeMethods.NETSETUP_JOIN_STATUS.NetSetupDomainName)
                        result = StringTools.FromPointer(pointerBuffer);
                }
            }

            return result;
        }

        internal static string GetComputerParentDomainIdentifier(string computerName = null, SystemAccessOptions options = default(SystemAccessOptions))
        {
            using (new AuthenticatedConnection(options))
            using (var objectAttributes = new UnmanagedStructure<NativeMethods.LSA_OBJECT_ATTRIBUTES>())
            using (var handle = new UnmanagedPointer())
            using (var data = new UnmanagedPointer())
            using (var systemName = new UnmanagedUnicodeString(computerName))
            using (var systemNameString = new UnmanagedStructure<NativeMethods.LSA_UNICODE_STRING>())
            {
                systemNameString.Value = new NativeMethods.LSA_UNICODE_STRING
                {
                    Buffer = +systemName,
                    Length = (ushort)systemName.USize,
                    MaximumLength = (ushort)systemName.USize,
                };

                uint error = NativeMethods.LsaOpenPolicy(string.IsNullOrEmpty(computerName) ? IntPtr.Zero : +systemNameString, +objectAttributes, NativeMethods.POLICY_VIEW_LOCAL_INFORMATION, +handle);
                if (error != 0)
                    NativeErrors.ThrowException("LsaOpenPolicy", error);

                using (new LsaHandleWrapper(handle))
                {
                    error = NativeMethods.LsaQueryInformationPolicy(handle, NativeMethods.POLICY_INFORMATION_CLASS.PolicyDnsDomainInformation, +data);
                    if (error != 0)
                        NativeErrors.ThrowException("LsaQueryInformationPolicy", error);

                    using (new LsaMemoryWrapper(data))
                    using (var domainInformation = new UnmanagedStructure<NativeMethods.POLICY_DNS_DOMAIN_INFO>(data))
                    {
                        return new SecurityIdentifier(domainInformation.Value.Sid).Value;
                    }
                }
            }
        }

        internal static string GetComputerShortName(string computerName = null, SystemAccessOptions options = default(SystemAccessOptions))
        {
            using (new AuthenticatedConnection(options))
            using (var pointer = new UnmanagedPointer())
            {
                uint error = NativeMethods.NetWkstaGetInfo(computerName, 100, +pointer);
                if (error != NativeMethods.NERR_Success)
                    NativeErrors.ThrowException("NetWkstaGetInfo", error, computerName);

                using (new NetMemoryWrapper(pointer))
                using (var data = new UnmanagedStructure<NativeMethods.WKSTA_INFO_100>(pointer))
                {
                    return StringTools.FromPointer(data.Value.wki100_computername);
                }
            }
        }

        internal static string GetComputerIdentifier(string computerName = null, SystemAccessOptions options = default(SystemAccessOptions))
        {
            string checkedComputerName = GetComputerShortName(computerName, options);

            using (var accountTypeBuffer = new UnmanagedInteger())
            using (var domainNameSizeBuffer = new UnmanagedInteger())
            using (var requiredSizeBuffer = new UnmanagedInteger())
            using (new AuthenticatedConnection(options))
            using (var pointer = new UnmanagedPointer())
            {
                bool domainController = false;

                pointer.Clear();
                uint error = NativeMethods.NetServerGetInfo(computerName, 101, +pointer);
                if (error != NativeMethods.NERR_Success)
                    NativeErrors.ThrowException("NetServerGetInfo", error, computerName);

                using (new NetMemoryWrapper(pointer))
                using (var dataStructure = new UnmanagedStructure<NativeMethods.SERVER_INFO_101>(pointer))
                {
                    domainController = dataStructure.Value.sv101_type.HasAnyFlag(NativeMethods.SV_TYPE_DOMAIN_CTRL | NativeMethods.SV_TYPE_DOMAIN_BAKCTRL);
                }

                if (domainController)
                    checkedComputerName += "$";

                error = NativeMethods.ERROR_SUCCESS;
                if (!NativeMethods.LookupAccountName(computerName, checkedComputerName, IntPtr.Zero, +requiredSizeBuffer, IntPtr.Zero, +domainNameSizeBuffer, +accountTypeBuffer))
                    error = NativeMethods.GetLastError();

                if ((error != NativeMethods.ERROR_SUCCESS) && (error != NativeMethods.ERROR_INSUFFICIENT_BUFFER))
                    NativeErrors.ThrowException("LookupAccountName", error, computerName, checkedComputerName);

                using (var domainNameBuffer = new UnmanagedUnicodeString(domainNameSizeBuffer))
                using (var securityIdentifierBuffer = new UnmanagedMemory(requiredSizeBuffer))
                using (var securityIdentifierStringPointerBuffer = new UnmanagedPointer())
                {
                    if (!NativeMethods.LookupAccountName(computerName, checkedComputerName, +securityIdentifierBuffer, +requiredSizeBuffer,
                        +domainNameBuffer, +domainNameSizeBuffer, +accountTypeBuffer))
                    {
                        NativeErrors.ThrowException("LookupAccountName", error, computerName, checkedComputerName);
                    }

                    if (!NativeMethods.ConvertSidToStringSid(+securityIdentifierBuffer, +securityIdentifierStringPointerBuffer))
                        NativeErrors.ThrowLastErrorException("ConvertSidToStringSid");

                    using (new LocalObjectMemoryWrapper(securityIdentifierStringPointerBuffer))
                    {
                        return StringTools.FromPointer(securityIdentifierStringPointerBuffer);
                    }
                }
            }
        }

        internal static Version GetComputerSystemVersion(string computerName = null, SystemAccessOptions options = default(SystemAccessOptions))
        {
            computerName = computerName ?? GetCurrentComputerName();

            using (new AuthenticatedConnection(options))
            using (var pointer = new UnmanagedPointer())
            {
                uint error = NativeMethods.NetWkstaGetInfo(computerName, 100, +pointer);
                if (error != NativeMethods.NERR_Success)
                    NativeErrors.ThrowException("NetWkstaGetInfo", error, computerName);

                using (new NetMemoryWrapper(pointer))
                using (var data = new UnmanagedStructure<NativeMethods.WKSTA_INFO_100>(pointer))
                {
                    return new Version((int)data.Value.wki100_ver_major, (int)data.Value.wki100_ver_minor);
                }
            }
        }

        internal static bool IsDomainController(string computerName = null, SystemAccessOptions options = default(SystemAccessOptions))
        {
            using (var pointerBuffer = new UnmanagedPointer())
            using (new AuthenticatedConnection(options))
            {
                uint error = NativeMethods.NetServerGetInfo(computerName, 101, +pointerBuffer);
                if (error != NativeMethods.NERR_Success)
                    NativeErrors.ThrowException("NetServerGetInfo", error);

                using (new NetMemoryWrapper(pointerBuffer))
                using (var dataStructure = new UnmanagedStructure<NativeMethods.SERVER_INFO_101>(pointerBuffer))
                {
                    return dataStructure.Value.sv101_type.HasAnyFlag(NativeMethods.SV_TYPE_DOMAIN_CTRL | NativeMethods.SV_TYPE_DOMAIN_BAKCTRL);
                }
            }
        }
    }
}
