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

[assembly: SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Scope = "member", Target = "LeakBlocker.Agent.MainService.TemporaryAccess.TrayIcon.#IconDoubleClick")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Scope = "member", Target = "LeakBlocker.Agent.MainService.TemporaryAccess.TrayIcon.#MenuLaunchAtStartupCheckedChanged")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Scope = "member", Target = "LeakBlocker.Agent.MainService.TemporaryAccess.TrayIcon.#MenuExitClick")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Scope = "member", Target = "LeakBlocker.Agent.MainService.TemporaryAccess.TrayIcon.#MenuShowWindowClick")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Scope = "member", Target = "LeakBlocker.Agent.MainService.Network.TemporaryAccess.TemporaryAccessServer.#ClientDisconnected")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Scope = "member", Target = "LeakBlocker.Agent.MainService.Network.TemporaryAccess.TemporaryAccessServer.#TemporaryAccessCancelled")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Scope = "member", Target = "LeakBlocker.Agent.MainService.Network.TemporaryAccess.TemporaryAccessServer.#PasswordReceived")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Scope = "member", Target = "LeakBlocker.Agent.MainService.Network.TemporaryAccess.TemporaryAccessClient.#NotificationReceived")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "Logoff", Scope = "member", Target = "LeakBlocker.Agent.MainService.SessionEventType.#Logoff")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "Logon", Scope = "member", Target = "LeakBlocker.Agent.MainService.SessionEventType.#Logon")]
[assembly: SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.Runtime.InteropServices.SafeHandle.DangerousGetHandle", Scope = "member", Target = "LeakBlocker.Agent.MainService.SystemTools.RegistryKeyMonitor.#MonitorThread()")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Scope = "member", Target = "LeakBlocker.Agent.MainService.SystemTools.RegistryKeyMonitor.#Changed")]
[assembly: SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.Runtime.InteropServices.SafeHandle.DangerousGetHandle", Scope = "member", Target = "LeakBlocker.Agent.MainService.SystemTools.NamedPipeImpersonation.#ConnectAndImpersonateServer(System.String,System.String,System.Collections.Generic.ICollection'1<System.Int32>)")]
[assembly: SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.Runtime.InteropServices.SafeHandle.DangerousGetHandle", Scope = "member", Target = "LeakBlocker.Agent.MainService.SystemTools.NamedPipeImpersonation.#.ctor(System.String,System.String,System.TimeSpan)")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Scope = "member", Target = "LeakBlocker.Agent.MainService.Network.LocalConfigurator.LocalConfiguratorServer.#ConfigurationQueried")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Scope = "member", Target = "LeakBlocker.Agent.MainService.Network.LocalConfigurator.LocalConfiguratorClient.#NotificationReceived")]
[assembly: SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.Runtime.InteropServices.SafeHandle.DangerousGetHandle", Scope = "member", Target = "LeakBlocker.Agent.MainService.SystemTools.NamedPipeImpersonation.#ConnectAndImpersonateServer(System.String,System.String,System.Collections.Generic.ICollection`1<System.Int32>)")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Scope = "member", Target = "LeakBlocker.Agent.FileSystemDriver.Controller.FileSystemDriverController.#FileOperationOccurred")]
[assembly: SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.Runtime.InteropServices.SafeHandle.DangerousGetHandle", Scope = "member", Target = "LeakBlocker.Agent.Core.SystemTools.NamedPipeImpersonation.#.ctor(System.String,System.String,System.TimeSpan)")]
[assembly: SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.Runtime.InteropServices.SafeHandle.DangerousGetHandle", Scope = "member", Target = "LeakBlocker.Agent.Core.SystemTools.NamedPipeImpersonation.#ConnectAndImpersonateServer(System.String,System.String,System.Collections.Generic.ICollection`1<System.Int32>)")]
[assembly: SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.Runtime.InteropServices.SafeHandle.DangerousGetHandle", Scope = "member", Target = "LeakBlocker.Agent.Core.SystemTools.RegistryKeyMonitor.#MonitorThread()")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "Logon", Scope = "member", Target = "LeakBlocker.Agent.Core.SessionEventType.#Logon")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "Logoff", Scope = "member", Target = "LeakBlocker.Agent.Core.SessionEventType.#Logoff")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Scope = "member", Target = "LeakBlocker.Agent.Core.Network.TemporaryAccess.TemporaryAccessClient.#NotificationReceived")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Scope = "member", Target = "LeakBlocker.Agent.Core.Network.TemporaryAccess.TemporaryAccessServer.#TemporaryAccessCancelled")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Scope = "member", Target = "LeakBlocker.Agent.Core.Network.TemporaryAccess.TemporaryAccessServer.#PasswordReceived")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Scope = "member", Target = "LeakBlocker.Agent.Core.Network.TemporaryAccess.TemporaryAccessServer.#ClientDisconnected")]
[assembly: SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Scope = "member", Target = "LeakBlocker.Agent.Core.AgentObjects.#.cctor()")]
[assembly: SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Scope = "member", Target = "LeakBlocker.Agent.Core.AgentObjects.#.cctor()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2232:MarkWindowsFormsEntryPointsWithStaThread")]
