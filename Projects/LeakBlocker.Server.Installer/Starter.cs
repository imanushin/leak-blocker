using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Server.Installer
{
    internal static class Starter
    {
        private static int Main(string[] args)
        {
            AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;

            return MainProgram.EntryPoint(args);
        }

        private static Assembly AssemblyResolve(object sender, ResolveEventArgs args)
        {
            switch (new AssemblyName(args.Name).Name)
            {
                case "LeakBlocker.Libraries.Common":
                    return Assembly.Load(EmbeddedExecutables.LeakBlocker_Common);
                case "LeakBlocker.Libraries.SystemTools":
                    return Assembly.Load(EmbeddedExecutables.LeakBlocker_SystemTool);
                default:
                    return null;
            }
        }
    }
}
