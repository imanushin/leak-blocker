// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.
//
// To add a suppression to this file, right-click the message in the 
// Code Analysis results, point to "Suppress Message", and click 
// "In Suppression File".
// You do not need to add suppressions to this file manually.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using LeakBlocker.Libraries.Common;

[assembly: SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "logon", Scope = "member", Target = "LeakBlocker.Libraries.Security.Sessions.UserSession.#.ctor(LeakBlocker.Libraries.Security.Sessions.UserSessionType,LeakBlocker.Libraries.Security.Accounts.UserAccount,LeakBlocker.Libraries.Security.Accounts.ComputerAccount,System.DateTime)")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "Logon", Scope = "member", Target = "LeakBlocker.Libraries.Security.Sessions.UserSession.#LogonTime")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "Logon", Scope = "member", Target = "LeakBlocker.Libraries.Security.Sessions.SystemSessions.SystemUserSession.#LogonTime")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "LeakBlocker.Libraries.Security.ActiveDirectory.OrganizationalUnit.#Key")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "LeakBlocker.Libraries.Security.Credentials.#Key")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "LeakBlocker.Libraries.Security.Accounts.Account.#Key")]
[assembly: SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Scope = "member", Target = "LeakBlocker.Libraries.Security.SecurityObjects.#.cctor()")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "logon", Scope = "member", Target = "LeakBlocker.Libraries.Security.Sessions.UserSession.#.ctor(LeakBlocker.Libraries.Security.Sessions.UserSessionType,LeakBlocker.Libraries.Common.Entities.Security.IBaseUserAccount,LeakBlocker.Libraries.Common.Entities.Security.ComputerAccount,System.DateTime)")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Scope = "member", Target = "LeakBlocker.Libraries.Security.Entities.ISecurityObjectCache.#Updated")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "logon", Scope = "member", Target = "LeakBlocker.Libraries.Security.Sessions.UserSession.#.ctor(LeakBlocker.Libraries.Security.Sessions.UserSessionType,LeakBlocker.Libraries.Common.Entities.Security.BaseUserAccount,LeakBlocker.Libraries.Common.Entities.Security.BaseComputerAccount,System.DateTime)")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "Logon", Scope = "member", Target = "LeakBlocker.Libraries.Security.UserPrivilege.#BatchLogon")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "Logon", Scope = "member", Target = "LeakBlocker.Libraries.Security.UserPrivilege.#DenyBatchLogon")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "Logon", Scope = "member", Target = "LeakBlocker.Libraries.Security.UserPrivilege.#DenyInteractiveLogon")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "Logon", Scope = "member", Target = "LeakBlocker.Libraries.Security.UserPrivilege.#DenyNetworkLogon")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "Logon", Scope = "member", Target = "LeakBlocker.Libraries.Security.UserPrivilege.#DenyRemoteInteractiveLogon")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "Logon", Scope = "member", Target = "LeakBlocker.Libraries.Security.UserPrivilege.#DenyServiceLogon")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "Logon", Scope = "member", Target = "LeakBlocker.Libraries.Security.UserPrivilege.#InteractiveLogon")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "Logon", Scope = "member", Target = "LeakBlocker.Libraries.Security.UserPrivilege.#NetworkLogon")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "Logon", Scope = "member", Target = "LeakBlocker.Libraries.Security.UserPrivilege.#RemoteInteractiveLogon")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "Logon", Scope = "member", Target = "LeakBlocker.Libraries.Security.UserPrivilege.#ServiceLogon")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "Logon", Scope = "member", Target = "LeakBlocker.Libraries.SystemTools.UserPrivilege.#ServiceLogon")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "Logon", Scope = "member", Target = "LeakBlocker.Libraries.SystemTools.UserPrivilege.#RemoteInteractiveLogon")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "Logon", Scope = "member", Target = "LeakBlocker.Libraries.SystemTools.UserPrivilege.#NetworkLogon")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "Logon", Scope = "member", Target = "LeakBlocker.Libraries.SystemTools.UserPrivilege.#InteractiveLogon")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "Logon", Scope = "member", Target = "LeakBlocker.Libraries.SystemTools.UserPrivilege.#DenyServiceLogon")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "Logon", Scope = "member", Target = "LeakBlocker.Libraries.SystemTools.UserPrivilege.#DenyRemoteInteractiveLogon")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "Logon", Scope = "member", Target = "LeakBlocker.Libraries.SystemTools.UserPrivilege.#DenyNetworkLogon")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "Logon", Scope = "member", Target = "LeakBlocker.Libraries.SystemTools.UserPrivilege.#DenyInteractiveLogon")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "Logon", Scope = "member", Target = "LeakBlocker.Libraries.SystemTools.UserPrivilege.#DenyBatchLogon")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "Logon", Scope = "member", Target = "LeakBlocker.Libraries.SystemTools.UserPrivilege.#BatchLogon")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ieee", Scope = "member", Target = "LeakBlocker.Libraries.SystemTools.Devices.DeviceType.#Ieee1394NetworkEnumerator")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ieee", Scope = "member", Target = "LeakBlocker.Libraries.SystemTools.Devices.DeviceType.#Ieee1394Debugger")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "SCSI", Scope = "member", Target = "LeakBlocker.Libraries.SystemTools.Devices.DeviceType.#SCSIAdapter")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "PCMCIA", Scope = "member", Target = "LeakBlocker.Libraries.SystemTools.Devices.DeviceType.#PCMCIA")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "IEEE", Scope = "member", Target = "LeakBlocker.Libraries.SystemTools.Devices.DeviceType.#IEEE1394")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "IEC", Scope = "member", Target = "LeakBlocker.Libraries.SystemTools.Devices.DeviceType.#IEC61883")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Scope = "member", Target = "LeakBlocker.Libraries.SystemTools.Entities.ISecurityObjectCache.#Updated")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ce", Scope = "member", Target = "LeakBlocker.Libraries.SystemTools.Devices.DeviceType.#WindowsCeActiveSync")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Ce", Scope = "member", Target = "LeakBlocker.Libraries.SystemTools.Devices.DeviceType.#WindowsCeActiveSync")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Iec", Scope = "member", Target = "LeakBlocker.Libraries.SystemTools.Devices.DeviceType.#Iec61883")]
[assembly: SuppressMessage("Microsoft.Design", "CA1027:MarkEnumsWithFlags", Scope = "type", Target = "LeakBlocker.Libraries.SystemTools.Devices.DeviceError")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ce", Scope = "member", Target = "LeakBlocker.Libraries.SystemTools.Devices.SystemDeviceType.#WindowsCeActiveSync")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Iec", Scope = "member", Target = "LeakBlocker.Libraries.SystemTools.Devices.SystemDeviceType.#Iec61883")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Ce", Scope = "member", Target = "LeakBlocker.Libraries.SystemTools.Devices.SystemDeviceType.#WindowsCeActiveSync")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Scope = "member", Target = "LeakBlocker.Libraries.SystemTools.Drivers.FileSystemDriverController.#FileAccessed")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Scope = "member", Target = "LeakBlocker.Libraries.SystemTools.Drivers.FileSystemDriverController.#VolumeDetached")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Scope = "member", Target = "LeakBlocker.Libraries.SystemTools.Drivers.FileSystemDriverController.#VolumeAttached")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Scope = "member", Target = "LeakBlocker.Libraries.SystemTools.ProcessTools.IServiceEventHandler.#UninstallRequested")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Scope = "member", Target = "LeakBlocker.Libraries.SystemTools.ProcessTools.IServiceEventHandler.#StopRequested")]
[assembly: SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Scope = "member", Target = "LeakBlocker.Libraries.SystemTools.SystemObjects.#.cctor()")]
[assembly: SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Scope = "member", Target = "LeakBlocker.Libraries.SystemTools.SystemObjects.#.cctor()")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Scope = "member", Target = "LeakBlocker.Libraries.SystemTools.ProcessTools.IServiceHandler.#UninstallRequested")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Scope = "member", Target = "LeakBlocker.Libraries.SystemTools.ProcessTools.IServiceHandler.#StopRequested")]
[assembly: SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.Runtime.InteropServices.SafeHandle.DangerousGetHandle", Scope = "member", Target = "LeakBlocker.Libraries.SystemTools.Implementations.NamedPipeImpersonation.#ConnectAndImpersonateServer(System.String,System.String,System.Collections.Generic.ICollection`1<System.Int32>)")]
[assembly: SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.Runtime.InteropServices.SafeHandle.DangerousGetHandle", Scope = "member", Target = "LeakBlocker.Libraries.SystemTools.Implementations.NamedPipeImpersonation.#.ctor(System.String,System.String,System.TimeSpan)")]
[assembly: SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "LeakBlocker.Libraries.SystemTools.SystemObjects+Factories.#DriverPackage")]
[assembly: SuppressMessage("Microsoft.Usage", "CA2205:UseManagedEquivalentsOfWin32Api", Scope = "member", Target = "LeakBlocker.Libraries.SystemTools.Win32.NativeMethods.#Process32Next(System.IntPtr,System.IntPtr)")]
[assembly: SuppressMessage("Microsoft.Usage", "CA2205:UseManagedEquivalentsOfWin32Api", Scope = "member", Target = "LeakBlocker.Libraries.SystemTools.Win32.NativeMethods.#Process32First(System.IntPtr,System.IntPtr)")]
[assembly: SuppressMessage("Microsoft.Usage", "CA2205:UseManagedEquivalentsOfWin32Api", Scope = "member", Target = "LeakBlocker.Libraries.SystemTools.Win32.NativeMethods.#GetLastError()")]
[assembly: SuppressMessage("Microsoft.Design", "CA1027:MarkEnumsWithFlags", Scope = "type", Target = "LeakBlocker.Libraries.SystemTools.ProcessTools.SystemServiceType")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1415:DeclarePInvokesCorrectly", Scope = "member", Target = "LeakBlocker.Libraries.SystemTools.Win32.NativeMethods.#ReadFile(System.IntPtr,System.IntPtr,System.UInt32,System.IntPtr,System.IntPtr)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1415:DeclarePInvokesCorrectly", Scope = "member", Target = "LeakBlocker.Libraries.SystemTools.Win32.NativeMethods.#WriteFile(System.IntPtr,System.IntPtr,System.UInt32,System.IntPtr,System.IntPtr)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Scope = "member", Target = "LeakBlocker.Libraries.SystemTools.Network.IMailslotServer.#MessageReceived")]
