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

[assembly: SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Scope = "member", Target = "LeakBlocker.Libraries.Common.IBackgroundWorker.#WorkCompleted")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Scope = "member", Target = "LeakBlocker.Libraries.Common.IBackgroundWorker.#DoWork")]
[assembly: SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Scope = "type", Target = "LeakBlocker.Libraries.Common.IoC.Factory`3")]
[assembly: SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Scope = "type", Target = "LeakBlocker.Libraries.Common.IoC.Factory`4")]
[assembly: SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Scope = "type", Target = "LeakBlocker.Libraries.Common.IoC.Factory`5")]
[assembly: SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Scope = "type", Target = "LeakBlocker.Libraries.Common.IoC.Factory`6")]
[assembly: SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Scope = "type", Target = "LeakBlocker.Libraries.Common.IoC.Factory`7")]
[assembly: SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Scope = "type", Target = "LeakBlocker.Libraries.Common.IoC.Factory`8")]
[assembly: SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Scope = "type", Target = "LeakBlocker.Libraries.Common.IoC.Factory`9")]
[assembly: SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Scope = "type", Target = "LeakBlocker.Libraries.Common.IoC.Factory`10")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1701:ResourceStringCompoundWordsShouldBeCasedCorrectly", MessageId = "logon", Scope = "resource", Target = "LeakBlocker.Libraries.Common.Resources.UserSessionTypeDescriptions.resources")]
[assembly: SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Scope = "member", Target = "LeakBlocker.Libraries.Common.SharedObjects.#.cctor()")]
[assembly: SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Scope = "member", Target = "LeakBlocker.Libraries.Common.SharedObjects.#.cctor()")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1703:ResourceStringsShouldBeSpelledCorrectly", MessageId = "pwd", Scope = "resource", Target = "LeakBlocker.Libraries.Common.Resources.ServerStrings.resources")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1703:ResourceStringsShouldBeSpelledCorrectly", MessageId = "ul", Scope = "resource", Target = "LeakBlocker.Libraries.Common.Resources.ServerStrings.resources")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1703:ResourceStringsShouldBeSpelledCorrectly", MessageId = "ul", Scope = "resource", Target = "LeakBlocker.Libraries.Common.Resources.AgentServiceStrings.resources")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1703:ResourceStringsShouldBeSpelledCorrectly", MessageId = "pwd", Scope = "resource", Target = "LeakBlocker.Libraries.Common.Resources.AgentServiceStrings.resources")]
