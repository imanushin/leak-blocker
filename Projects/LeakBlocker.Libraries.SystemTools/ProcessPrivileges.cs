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
    /// Privileges determine the type of system operations that a user account can perform.
    /// </summary>
    public enum ProcessPrivilege
    {
        /// <summary>
        /// Incorrect value.
        /// </summary>
        [ForbiddenToUse]
        None = 0,

        /// <summary>
        /// Assign the primary token of a process.
        /// </summary>
        AssignPrimaryToken = 1,

        /// <summary>
        /// Generate audit-log entries. 
        /// </summary>
        Audit,

        /// <summary>
        /// Perform backup operations.
        /// </summary>
        Backup,

        /// <summary>
        /// Receive notifications of changes to files or directories. 
        /// </summary>
        ChangeNotify,

        /// <summary>
        /// Create named file mapping objects in the global namespace during Terminal Services sessions.
        /// </summary>
        CreateGlobal,

        /// <summary>
        /// Create a paging file. 
        /// </summary>
        CreatePageFile,

        /// <summary>
        /// Create a permanent object. 
        /// </summary>
        CreatePermanent,

        /// <summary>
        /// Create a symbolic link.
        /// </summary>
        CreateSymbolicLink,

        /// <summary>
        /// Create a primary token. 
        /// </summary>
        CreateToken,

        /// <summary>
        /// Debug and adjust the memory of a process owned by another account. 
        /// </summary>
        Debug,

        /// <summary>
        /// Mark user and computer accounts as trusted for delegation.
        /// </summary>
        EnableDelegation,

        /// <summary>
        /// Impersonate.
        /// </summary>
        Impersonate,

        /// <summary>
        /// Increase the base priority of a process. 
        /// </summary>
        IncreaseBasePriority,

        /// <summary>
        /// Increase the quota assigned to a process. 
        /// </summary>
        IncreaseQuota,

        /// <summary>
        /// Allocate more memory for applications that run in the context of users.
        /// </summary>
        IncreaseWorkingSet,

        /// <summary>
        /// Load or unload a device driver. 
        /// </summary>
        LoadDriver,

        /// <summary>
        /// Lock physical pages in memory. 
        /// </summary>
        LockMemory,

        /// <summary>
        /// Create a computer account. 
        /// </summary>
        MachineAccount,

        /// <summary>
        /// Enable volume management privileges. 
        /// </summary>
        ManageVolume,

        /// <summary>
        /// Gather profiling information for a single process. 
        /// </summary>
        ProfileSingleProcess,

        /// <summary>
        /// Modify the mandatory integrity level of an object.
        /// </summary>
        Relabel,

        /// <summary>
        /// Shut down a system using a network request. 
        /// </summary>
        RemoteShutdown,

        /// <summary>
        /// Perform restore operations. 
        /// </summary>
        Restore,

        /// <summary>
        /// Perform a number of security-related functions, such as controlling and viewing audit messages.
        /// </summary>
        Security,

        /// <summary>
        /// Shut down a local system. 
        /// </summary>
        Shutdown,

        /// <summary>
        /// Use the Lightweight Directory Access Protocol directory synchronization services.
        /// </summary>
        SynchronizationAgent,

        /// <summary>
        /// Modify the nonvolatile RAM of systems that use this type of memory to store configuration information. 
        /// </summary>
        SystemEnvironment,

        /// <summary>
        /// Gather profiling information for the entire system. 
        /// </summary>
        SystemProfile,

        /// <summary>
        /// Modify the system time. 
        /// </summary>
        SystemTime,

        /// <summary>
        /// Take ownership of an object without being granted discretionary access.
        /// </summary>
        TakeOwnership,

        /// <summary>
        /// Identifies its holder as part of the trusted computer base. 
        /// </summary>
        TrustedComputerBase,

        /// <summary>
        /// Adjust the time zone associated with the computer's internal clock.
        /// </summary>
        TimeZone,

        /// <summary>
        /// Access Credential Manager as a trusted caller.
        /// </summary>
        TrustedCredentialManagerAccess,

        /// <summary>
        /// Undock a laptop.
        /// </summary>
        Undock,

        /// <summary>
        /// Read unsolicited input from a terminal device.
        /// </summary>
        UnsolicitedInput
    }

    /// <summary>
    /// Provides methods for adjusting process privileges.
    /// </summary>
    public static class ProcessPrivileges
    {
        private static readonly Dictionary<ProcessPrivilege, string> privilegeNames = new Dictionary<ProcessPrivilege, string>
        {
            #region values
            { ProcessPrivilege.AssignPrimaryToken, "SeAssignPrimaryTokenPrivilege" },
            { ProcessPrivilege.Audit, "SeAuditPrivilege" },
            { ProcessPrivilege.Backup, "SeBackupPrivilege" },
            { ProcessPrivilege.ChangeNotify, "SeChangeNotifyPrivilege" },
            { ProcessPrivilege.CreateGlobal, "SeCreateGlobalPrivilege" },
            { ProcessPrivilege.CreatePageFile, "SeCreatePagefilePrivilege" },
            { ProcessPrivilege.CreatePermanent, "SeCreatePermanentPrivilege" },
            { ProcessPrivilege.CreateSymbolicLink, "SeCreateSymbolicLinkPrivilege" },
            { ProcessPrivilege.CreateToken, "SeCreateTokenPrivilege" },
            { ProcessPrivilege.Debug, "SeDebugPrivilege" },
            { ProcessPrivilege.EnableDelegation, "SeEnableDelegationPrivilege" },
            { ProcessPrivilege.Impersonate, "SeImpersonatePrivilege" },
            { ProcessPrivilege.IncreaseBasePriority, "SeIncreaseBasePriorityPrivilege" },
            { ProcessPrivilege.IncreaseQuota, "SeIncreaseQuotaPrivilege" },
            { ProcessPrivilege.IncreaseWorkingSet, "SeIncreaseWorkingSetPrivilege" },
            { ProcessPrivilege.LoadDriver, "SeLoadDriverPrivilege" },
            { ProcessPrivilege.LockMemory, "SeLockMemoryPrivilege" },
            { ProcessPrivilege.MachineAccount, "SeMachineAccountPrivilege" },
            { ProcessPrivilege.ManageVolume, "SeManageVolumePrivilege" },
            { ProcessPrivilege.ProfileSingleProcess, "SeProfileSingleProcessPrivilege" },
            { ProcessPrivilege.Relabel, "SeRelabelPrivilege" },
            { ProcessPrivilege.RemoteShutdown, "SeRemoteShutdownPrivilege" },
            { ProcessPrivilege.Restore, "SeRestorePrivilege" },
            { ProcessPrivilege.Security, "SeSecurityPrivilege" },
            { ProcessPrivilege.Shutdown, "SeShutdownPrivilege" },
            { ProcessPrivilege.SynchronizationAgent, "SeSyncAgentPrivilege" },
            { ProcessPrivilege.SystemEnvironment, "SeSystemEnvironmentPrivilege" },
            { ProcessPrivilege.SystemProfile, "SeSystemProfilePrivilege" },
            { ProcessPrivilege.SystemTime, "SeSystemtimePrivilege" },
            { ProcessPrivilege.TakeOwnership, "SeTakeOwnershipPrivilege" },
            { ProcessPrivilege.TrustedComputerBase, "SeTcbPrivilege" },
            { ProcessPrivilege.TimeZone, "SeTimeZonePrivilege" },
            { ProcessPrivilege.TrustedCredentialManagerAccess, "SeTrustedCredManAccessPrivilege" },
            { ProcessPrivilege.Undock, "SeUndockPrivilege" },
            { ProcessPrivilege.UnsolicitedInput, "SeUnsolicitedInputPrivilege" }
            #endregion
        };

        /// <summary>
        /// Adds the specified privelege for the specified process token. 
        /// Process must have enough access rights for adjusting privileges.
        /// </summary>
        /// <param name="token">Process token.</param>
        /// <param name="privilege">Privilege.</param>
        public static void Add(IntPtr token, ProcessPrivilege privilege)
        {
            Check.PointerIsNotNull(token, "token");
            Check.EnumerationValueIsDefined(privilege, "privilege");

            string name = GetPrivilegeString(privilege);

            using (var luidBuffer = new UnmanagedStructure<NativeMethods.LUID>())
            {
                if (!NativeMethods.LookupPrivilegeValue(null, name, +luidBuffer))
                    NativeErrors.ThrowLastErrorException("LookupPrivilegeValue", name);

                using (var privilegesBuffer = new UnmanagedStructure<NativeMethods.TOKEN_PRIVILEGES>())
                {
                    privilegesBuffer.Value = new NativeMethods.TOKEN_PRIVILEGES
                    {
                        PrivilegeCount = 1,
                        Privileges = new[]
                        {
                            new NativeMethods.LUID_AND_ATTRIBUTES
                            {
                                Luid = luidBuffer,
                                Attributes = NativeMethods.SE_PRIVILEGE_ENABLED
                            }
                        }
                    };

                    if (!NativeMethods.AdjustTokenPrivileges(token, false, +privilegesBuffer, privilegesBuffer.USize, IntPtr.Zero, IntPtr.Zero))
                        NativeErrors.ThrowLastErrorException("AdjustTokenPrivileges");
                }
            }
        }

        /// <summary>
        /// Adds the specified privilege for the current process. 
        /// Process must have enough access rights for adjusting privileges.
        /// </summary>
        /// <param name="privilege">Privilege.</param>
        public static void AddForCurrentProcess(ProcessPrivilege privilege)
        {
            Check.EnumerationValueIsDefined(privilege, "privilege");

            using (var tokenBuffer = new UnmanagedPointer())
            {
                if (!NativeMethods.OpenProcessToken(NativeMethods.GetCurrentProcess(), NativeMethods.TOKEN_ADJUST_PRIVILEGES, +tokenBuffer))
                    NativeErrors.ThrowLastErrorException("OpenProcessToken");

                using (new NativeHandleWrapper(tokenBuffer))
                {
                    Add(tokenBuffer, privilege);
                }
            }
        }

        private static string GetPrivilegeString(ProcessPrivilege privilege)
        {
            if (!privilegeNames.ContainsKey(privilege))
                Exceptions.Throw(ErrorMessage.NotFound, "Privilege name is not defined.");

            return privilegeNames[privilege];
        }
    }
}
