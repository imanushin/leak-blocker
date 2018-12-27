using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using LeakBlocker.Libraries.Common;

namespace TestHost
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            if (args.Length > 0 && string.Equals(args[0], "/noservice", StringComparison.OrdinalIgnoreCase))
            {
                MainInvoker.Start();

                Console.ReadKey();

                MainInvoker.Stop();

                return;
            }

            ServiceBase.Run(new MainService());
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Trace.WriteLine(e.ExceptionObject);
        }
    }
}
