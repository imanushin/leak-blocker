using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.SystemTools.Network;
using LeakBlocker.Libraries.SystemTools.Win32;
using LeakBlocker.Libraries.SystemTools.Win32.ResourceWrappers;
using LeakBlocker.Libraries.SystemTools.Win32.UnmanagedTypes;

namespace LeakBlocker.Libraries.SystemTools.Entities.Management
{
    internal static class DomainTools
    {
        internal static bool IsDomain(string name, SystemAccessOptions options = default(SystemAccessOptions))
        {
            Check.StringIsMeaningful(name, "name");

            using (var binding = new UnmanagedPointer())
            {
                uint error;

                if (options.UserName != null)
                {
                    using (var credentialsPointerBuffer = new UnmanagedPointer())
                    {
                        error = NativeMethods.DsMakePasswordCredentials(options.ShortUserName, name, options.Password, +credentialsPointerBuffer);
                        if (error != 0)
                            NativeErrors.ThrowException("DsMakePasswordCredentials", error, options.ShortUserName, name);

                        using (new DirectoryServiceCredentials(credentialsPointerBuffer))
                        {
                            error = NativeMethods.DsBindWithCred(null, name, credentialsPointerBuffer, +binding);
                        }
                    }
                }
                else
                    error = NativeMethods.DsBind(null, name, +binding);

                if ((error != NativeMethods.ERROR_INVALID_DOMAINNAME) && (error != NativeMethods.ERROR_NO_SUCH_DOMAIN))
                {
                    if (error == 0)
                    {
                        using (new DirectoryServiceBindHandle(binding))
                        {
                        }
                    }
                    return true;
                }

                using (new AuthenticatedConnection(new SystemAccessOptions(options.UserName, options.Password, name)))
                using (var pointerBuffer = new UnmanagedPointer())
                using (var typeBuffer = new UnmanagedInteger())
                {
                    error = NativeMethods.NetServerGetInfo(name, 101, +pointerBuffer);
                    if (error != NativeMethods.NERR_Success)
                        NativeErrors.ThrowException("NetServerGetInfo", error, name);

                    using (new NetMemoryWrapper(pointerBuffer))
                    using (var dataStructure = new UnmanagedStructure<NativeMethods.SERVER_INFO_101>(pointerBuffer))
                    {
                        if (!dataStructure.Value.sv101_type.HasAnyFlag(NativeMethods.SV_TYPE_DOMAIN_CTRL | NativeMethods.SV_TYPE_DOMAIN_BAKCTRL))
                            return false;
                    }

                    pointerBuffer.Clear();

                    error = NativeMethods.NetGetJoinInformation(name, +pointerBuffer, +typeBuffer);
                    if (error != NativeMethods.NERR_Success)
                        NativeErrors.ThrowException("NetGetJoinInformation", error, name);

                    using (new NetMemoryWrapper(pointerBuffer))
                    {
                        if (typeBuffer.UValue != (uint)NativeMethods.NETSETUP_JOIN_STATUS.NetSetupDomainName)
                            return false;
                    }

                    pointerBuffer.Clear();

                    error = NativeMethods.DsRoleGetPrimaryDomainInformation(name,
                        NativeMethods.DSROLE_PRIMARY_DOMAIN_INFO_LEVEL.DsRolePrimaryDomainInfoBasic, +pointerBuffer);
                    if (error != NativeMethods.NERR_Success)
                        NativeErrors.ThrowException("DsRoleGetPrimaryDomainInformation", error, name);

                    using (new DirectoryServiceDomainInformation(pointerBuffer))
                    using (var dataBuffer = new UnmanagedStructure<NativeMethods.DSROLE_PRIMARY_DOMAIN_INFO_BASIC>(pointerBuffer))
                    {
                        string shortName = StringTools.FromPointer(dataBuffer.Value.DomainNameFlat);
                        string fullName = string.Empty;
                        if (dataBuffer.Value.DomainNameDns != IntPtr.Zero)
                            fullName = StringTools.FromPointer(dataBuffer.Value.DomainNameDns);

                        return name.Equals(shortName, StringComparison.OrdinalIgnoreCase) ||
                            name.Equals(fullName, StringComparison.OrdinalIgnoreCase);
                    }
                }
            }
        }
    }
}
