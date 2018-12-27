﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using LeakBlocker.Libraries.Common;

[assembly: AssemblyTitle("LeakBlocker.Libraries.SystemTools")]
[assembly: AssemblyDescription("")]

[assembly: InternalsVisibleTo("DomainSnapshotGenerator, PublicKeyToken=4c4dbf36c515c26f, PublicKey=0024000004800000940000000602000000240000525341310004000001000100d5f33866d27ad84da592303299bdaf53ef364d7f715d87e25c7df98fdb6366eb6abe2538d36bbd7f88e3d61b9ce0649c0c3a4cb5810ac3fd67647544d0c3176c6ea55ab77868ae8e8fd2a7cdf813c4f182e9d1dbf2f0e9eee6aaf10bd41ff0fefb911e473256c1ddd68acb928a52791d006c6db033feb1bf6a049633b2b47ccc")]

[assembly: InternalsVisibleTo("LeakBlocker.Libraries.SystemTools.Tests, PublicKeyToken=4c4dbf36c515c26f, PublicKey=0024000004800000940000000602000000240000525341310004000001000100d5f33866d27ad84da592303299bdaf53ef364d7f715d87e25c7df98fdb6366eb6abe2538d36bbd7f88e3d61b9ce0649c0c3a4cb5810ac3fd67647544d0c3176c6ea55ab77868ae8e8fd2a7cdf813c4f182e9d1dbf2f0e9eee6aaf10bd41ff0fefb911e473256c1ddd68acb928a52791d006c6db033feb1bf6a049633b2b47ccc")]

[assembly: InternalsVisibleTo("TestCaller, PublicKeyToken=4c4dbf36c515c26f, PublicKey=0024000004800000940000000602000000240000525341310004000001000100d5f33866d27ad84da592303299bdaf53ef364d7f715d87e25c7df98fdb6366eb6abe2538d36bbd7f88e3d61b9ce0649c0c3a4cb5810ac3fd67647544d0c3176c6ea55ab77868ae8e8fd2a7cdf813c4f182e9d1dbf2f0e9eee6aaf10bd41ff0fefb911e473256c1ddd68acb928a52791d006c6db033feb1bf6a049633b2b47ccc")]

[assembly: InternalsVisibleTo("LeakBlocker.Server.Service.Tests, PublicKeyToken=4c4dbf36c515c26f, PublicKey=0024000004800000940000000602000000240000525341310004000001000100d5f33866d27ad84da592303299bdaf53ef364d7f715d87e25c7df98fdb6366eb6abe2538d36bbd7f88e3d61b9ce0649c0c3a4cb5810ac3fd67647544d0c3176c6ea55ab77868ae8e8fd2a7cdf813c4f182e9d1dbf2f0e9eee6aaf10bd41ff0fefb911e473256c1ddd68acb928a52791d006c6db033feb1bf6a049633b2b47ccc")]
[assembly: InternalsVisibleTo("LeakBlocker.ExternalTests, PublicKeyToken=4c4dbf36c515c26f, PublicKey=0024000004800000940000000602000000240000525341310004000001000100d5f33866d27ad84da592303299bdaf53ef364d7f715d87e25c7df98fdb6366eb6abe2538d36bbd7f88e3d61b9ce0649c0c3a4cb5810ac3fd67647544d0c3176c6ea55ab77868ae8e8fd2a7cdf813c4f182e9d1dbf2f0e9eee6aaf10bd41ff0fefb911e473256c1ddd68acb928a52791d006c6db033feb1bf6a049633b2b47ccc")]
[assembly: InternalsVisibleTo("LeakBlocker.ServerShared.AgentCommunication.Tests, PublicKeyToken=4c4dbf36c515c26f, PublicKey=0024000004800000940000000602000000240000525341310004000001000100d5f33866d27ad84da592303299bdaf53ef364d7f715d87e25c7df98fdb6366eb6abe2538d36bbd7f88e3d61b9ce0649c0c3a4cb5810ac3fd67647544d0c3176c6ea55ab77868ae8e8fd2a7cdf813c4f182e9d1dbf2f0e9eee6aaf10bd41ff0fefb911e473256c1ddd68acb928a52791d006c6db033feb1bf6a049633b2b47ccc")]
[assembly: InternalsVisibleTo("LeakBlocker.Agent.Core.Tests, PublicKeyToken=4c4dbf36c515c26f, PublicKey=0024000004800000940000000602000000240000525341310004000001000100d5f33866d27ad84da592303299bdaf53ef364d7f715d87e25c7df98fdb6366eb6abe2538d36bbd7f88e3d61b9ce0649c0c3a4cb5810ac3fd67647544d0c3176c6ea55ab77868ae8e8fd2a7cdf813c4f182e9d1dbf2f0e9eee6aaf10bd41ff0fefb911e473256c1ddd68acb928a52791d006c6db033feb1bf6a049633b2b47ccc")]
[assembly: InternalsVisibleTo("LocalConfigurationCreator, PublicKeyToken=4c4dbf36c515c26f, PublicKey=0024000004800000940000000602000000240000525341310004000001000100d5f33866d27ad84da592303299bdaf53ef364d7f715d87e25c7df98fdb6366eb6abe2538d36bbd7f88e3d61b9ce0649c0c3a4cb5810ac3fd67647544d0c3176c6ea55ab77868ae8e8fd2a7cdf813c4f182e9d1dbf2f0e9eee6aaf10bd41ff0fefb911e473256c1ddd68acb928a52791d006c6db033feb1bf6a049633b2b47ccc")]
[assembly: InternalsVisibleTo("LeakBlocker.Server.Installer.Tests, PublicKeyToken=4c4dbf36c515c26f, PublicKey=0024000004800000940000000602000000240000525341310004000001000100d5f33866d27ad84da592303299bdaf53ef364d7f715d87e25c7df98fdb6366eb6abe2538d36bbd7f88e3d61b9ce0649c0c3a4cb5810ac3fd67647544d0c3176c6ea55ab77868ae8e8fd2a7cdf813c4f182e9d1dbf2f0e9eee6aaf10bd41ff0fefb911e473256c1ddd68acb928a52791d006c6db033feb1bf6a049633b2b47ccc")]
[assembly: InternalsVisibleTo("LeakBlocker.AdminView.Desktop.Tests, PublicKeyToken=4c4dbf36c515c26f, PublicKey=0024000004800000940000000602000000240000525341310004000001000100d5f33866d27ad84da592303299bdaf53ef364d7f715d87e25c7df98fdb6366eb6abe2538d36bbd7f88e3d61b9ce0649c0c3a4cb5810ac3fd67647544d0c3176c6ea55ab77868ae8e8fd2a7cdf813c4f182e9d1dbf2f0e9eee6aaf10bd41ff0fefb911e473256c1ddd68acb928a52791d006c6db033feb1bf6a049633b2b47ccc")]

