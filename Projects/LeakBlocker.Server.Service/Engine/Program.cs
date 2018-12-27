using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Server.Service.Engine
{
    [ExcludeFromCodeCoverage]
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static int Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;

            if (args.Contains("/awaitDebugging"))
            {
                while (!Debugger.IsAttached)
                {
                    Thread.Sleep(1000);
                }
            }

            if (args.Length == 1)
            {
                switch (args[0].ToUpperInvariant())
                {
                    case "/NOSERVICE":
                        RunAsConsoleApplication();
                        return 0;

                    case "/AWAITDEBUGGING":
                        break;

                    default:
                        throw new InvalidOperationException("Unable to retrieve argument {0}".Combine(args[0]));
                }
            }

            RunAsService();

            return 0;
        }

        private static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Write(e.ExceptionObject.ToString());
        }

        private static void RunAsService()
        {
            ServiceBase.Run(new MainService());
        }

        private static void RunAsConsoleApplication()
        {
            MainInvoker.Start();

            string[] arguments = Environment.GetCommandLineArgs();

            if (arguments.Length >= 3)
            {
                int otherProcess = int.Parse(arguments[2], CultureInfo.InvariantCulture);

                Process.GetProcessById(otherProcess).WaitForExit();
            }
            else
            {
                Thread.Sleep(int.MaxValue);
            }

            MainInvoker.Stop();
        }
    }
}
