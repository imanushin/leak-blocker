using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.SystemTools.Win32;
using LeakBlocker.Libraries.SystemTools.Win32.ResourceWrappers;
using LeakBlocker.Libraries.SystemTools.Win32.UnmanagedTypes;

namespace LeakBlocker.Libraries.SystemTools.Entities.Management
{
    internal sealed class LocalUserSession
    {
        private readonly SecurityIdentifier userIdentifier;

        internal string UserIdentifier
        {
            get
            {
                return userIdentifier.Value;
            }
        }

        internal string Name
        {
            get;
            private set;
        }

        internal string DomainName
        {
            get;
            private set;
        }

        internal string NetworkDomainName
        {
            get;
            private set;
        }

        internal bool Supported
        {
            get;
            private set;
        }

        internal bool Active
        {
            get;
            private set;
        }

        internal string UserPrincipalName
        {
            get;
            private set;
        }

        internal static IEnumerable<LocalUserSession> EnumerateLocalSessions()
        {
            var result = new HashSet<LocalUserSession>();

            string activeUser = GetActiveUserName();

            Log.Write("Active user name is {0}.", activeUser);

            using (var luidCountBuffer = new UnmanagedInteger())
            using (var luidPointerBuffer = new UnmanagedPointer())
            {
                uint error = NativeMethods.LsaEnumerateLogonSessions(+luidCountBuffer, +luidPointerBuffer);
                if (error != NativeMethods.STATUS_SUCCESS)
                    NativeErrors.ThrowLastErrorException("LsaEnumerateLogonSessions");

                using (new LsaReturnBufferWrapper(luidPointerBuffer.Value))
                using (var luidArrayBuffer = new UnmanagedArray<NativeMethods.LUID>(luidPointerBuffer.Value, luidCountBuffer.UValue))
                using (var sessionDataPointerBuffer = new UnmanagedPointer())
                {
                    for (uint i = 0; i < luidCountBuffer.UValue; i++)
                    {
                        error = NativeMethods.LsaGetLogonSessionData(luidArrayBuffer.GetElementAddress(i), +sessionDataPointerBuffer);
                        if (error != NativeMethods.STATUS_SUCCESS)
                            NativeErrors.ThrowException("LsaGetLogonSessionData", error);

                        using (new LsaReturnBufferWrapper(sessionDataPointerBuffer.Value))
                        using (var sessionDataBuffer = new UnmanagedStructure<NativeMethods.SECURITY_LOGON_SESSION_DATA>(sessionDataPointerBuffer.Value))
                        {
                            if (sessionDataBuffer.Value.Sid != IntPtr.Zero)
                                result.Add(new LocalUserSession(sessionDataBuffer.Value, activeUser));
                        }

                        sessionDataPointerBuffer.Clear();
                    }
                }
            }

            //WriteDebugData(result);

            return result;
        }

        private LocalUserSession(NativeMethods.SECURITY_LOGON_SESSION_DATA data, string activeUser)
        {
            userIdentifier = new SecurityIdentifier(data.Sid);

            bool systemAccount = (userIdentifier == new SecurityIdentifier(WellKnownSidType.LocalSystemSid, null)) ||
                (userIdentifier == new SecurityIdentifier(WellKnownSidType.NetworkServiceSid, null)) ||
                (userIdentifier == new SecurityIdentifier(WellKnownSidType.LocalServiceSid, null));
            NetworkDomainName = (data.DnsDomainName.Buffer == IntPtr.Zero) ? string.Empty : StringTools.FromPointer(data.DnsDomainName.Buffer);
            
            Supported = systemAccount || (userIdentifier.AccountDomainSid != null);
            Name = StringTools.FromPointer(data.UserName.Buffer) ?? string.Empty;
            DomainName = StringTools.FromPointer(data.LogonDomain.Buffer) ?? string.Empty;
            UserPrincipalName = StringTools.FromPointer(data.Upn.Buffer) ?? string.Empty;

            Active = !systemAccount &&
                (Name.ToUpperInvariant() == NameConversion.SimplifyUserName(activeUser).ToUpperInvariant()) &&
                (DomainName.ToUpperInvariant() == NameConversion.GetUserDomainName(activeUser).ToUpperInvariant());
        }

        public override int GetHashCode()
        {
            return UserIdentifier.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var localUserSession = obj as LocalUserSession;

            return (localUserSession != null) && (UserIdentifier == localUserSession.UserIdentifier);
        }

        internal static string GetActiveUserName()
        {
            using (var dataPointerBuffer = new UnmanagedPointer())
            using (var valuesCountBuffer = new UnmanagedInteger())
            {
                if (!NativeMethods.WTSEnumerateSessions(NativeMethods.WTS_CURRENT_SERVER_HANDLE, 0, 1, +dataPointerBuffer, +valuesCountBuffer))
                {
                    uint error = NativeMethods.GetLastError();
                    if (error == NativeMethods.RPC_S_INVALID_BINDING)
                    {
                        Log.Write("Cannot enumerate WTS sessions. Not initialized yet.");
                        return null;
                    }

                    NativeErrors.ThrowLastErrorException("WTSEnumerateSessions");
                }

                using (new WtsMemoryWrapper(dataPointerBuffer.Value))
                using (var sessionInformationStructuresBuffer = new UnmanagedArray<NativeMethods.WTS_SESSION_INFO>(dataPointerBuffer.Value, valuesCountBuffer.UValue))
                {
                    var consoleSessions = new HashSet<string>();
                    var otherSessions = new HashSet<string>();

                    foreach (NativeMethods.WTS_SESSION_INFO currentItem in sessionInformationStructuresBuffer.Value)
                    {
                        int type;
                        string domain;
                        string user;

                        using (var dataSize = new UnmanagedInteger())
                        using (var dataPointer = new UnmanagedPointer())
                        {
                            if (!NativeMethods.WTSQuerySessionInformation(NativeMethods.WTS_CURRENT_SERVER_HANDLE, currentItem.SessionId, NativeMethods.WTS_INFO_CLASS.WTSClientProtocolType, +dataPointer, +dataSize))
                                NativeErrors.ThrowLastErrorException("WTSQuerySessionInformation");

                            using (new WtsMemoryWrapper(dataPointer))
                            using (var data = new UnmanagedShort(dataPointer))
                            {
                                if (dataSize != 2)
                                    Exceptions.Throw(ErrorMessage.InvalidData, "Unexpected data size ({0})".Combine(dataSize));

                                type = data.SValue;
                            }

                            dataSize.Clear();
                            dataPointer.Clear();

                            if (!NativeMethods.WTSQuerySessionInformation(NativeMethods.WTS_CURRENT_SERVER_HANDLE, currentItem.SessionId, NativeMethods.WTS_INFO_CLASS.WTSDomainName, +dataPointer, +dataSize))
                                NativeErrors.ThrowLastErrorException("WTSQuerySessionInformation");

                            using (new WtsMemoryWrapper(dataPointer))
                            using (var data = new UnmanagedUnicodeString(dataPointer, dataSize / 2))
                            {
                                domain = data.Value;
                            }

                            dataSize.Clear();
                            dataPointer.Clear();

                            if (!NativeMethods.WTSQuerySessionInformation(NativeMethods.WTS_CURRENT_SERVER_HANDLE, currentItem.SessionId, NativeMethods.WTS_INFO_CLASS.WTSUserName, +dataPointer, +dataSize))
                                NativeErrors.ThrowLastErrorException("WTSQuerySessionInformation");

                            using (new WtsMemoryWrapper(dataPointer))
                            using (var data = new UnmanagedUnicodeString(dataPointer, dataSize / 2))
                            {
                                user = data.Value;
                            }
                        }

                        if (string.IsNullOrEmpty(user))
                            continue;

                        if (string.IsNullOrEmpty(domain))
                            domain = NameConversion.GetUserDomainName(user);
                        user = NameConversion.SimplifyUserName(user);

                        string fullName = "{0}\\{1}".Combine(domain, user);

                        ((type == 0) ? consoleSessions : otherSessions).Add(fullName);
                    }

                    return consoleSessions.FirstOrDefault() ?? otherSessions.FirstOrDefault();
                }
            }
        }


        private static void WriteDebugData(IEnumerable<LocalUserSession> sessions)
        {
            StringBuilder result = new StringBuilder();

            foreach (LocalUserSession session in sessions)
            {
                var data = new Dictionary<string, object>
                {
                    { "UserIdentifier", session.UserIdentifier },
                    { "Name", session.Name },
                    { "DomainName", session.DomainName },
                    { "NetworkDomainName", session.NetworkDomainName },
                    { "Supported", session.Supported },
                    { "Active", session.Active },
                    { "UserPrincipalName", session.UserPrincipalName },
                };

                result.AppendLine("   ***** SESSION *****");
                foreach (KeyValuePair<string, object> currentItem in data)
                {
                    result.AppendLine();
                    result.Append("   {0,-20} {1}".Combine(currentItem.Key, currentItem.Value));
                }
            }

            Log.Write(result.ToString());
        }
    }
}
