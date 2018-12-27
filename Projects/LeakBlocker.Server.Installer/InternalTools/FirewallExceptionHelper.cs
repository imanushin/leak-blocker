using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Server.Installer.InternalTools
{
    internal sealed class FirewallExceptionHelper : IFirewallExceptionHelper
    {
        private static readonly string exeFile = InternalObjects.FileSystemConstants.ServicePath;

        private static readonly string version = SharedObjects.Constants.Version.ToString(3);

        private static readonly string vistaInRuleName = "Leak Blocker " + version;
        private static readonly string vistaOutRuleName = "Leak Blocker " + version;
        private static readonly string xpRuleName = "Leak Blocker " + version;

        public void AddExceptionsForCurrentProcess()
        {
            if (Environment.OSVersion.Version.Major > 5)
            {
                AddRuleVista("in", vistaInRuleName);
                AddRuleVista("out", vistaOutRuleName);
            }
            else
            {
                AddRuleXp();
            }
        }

        public void RemoveExceptionsForCurrentProcess()
        {
            if (Environment.OSVersion.Version.Major > 5)
            {
                RemoveRuleVista();
            }
            else
            {
                RemoveRuleXp();
            }
        }

        private static void RemoveRuleXp()
        {
            const string createRuleFormat = "firewall delete allowedprogram \"{0}\"";

            string args = createRuleFormat.Combine(exeFile);

            RunAndWriteError(args);
        }

        private static void RemoveRuleVista()
        {
            const string createRuleFormat = "advfirewall firewall delete rule \"{0}\"";

            RunAndWriteError(createRuleFormat.Combine(vistaInRuleName));
            RunAndWriteError(createRuleFormat.Combine(vistaOutRuleName));
        }

        private static void AddRuleXp()
        {
            if (IsRuleExistsXp())
                return;

            const string createRuleFormat = "firewall add allowedprogram \"{0}\" \"{1}\" ENABLE";

            string args = createRuleFormat.Combine(exeFile, xpRuleName);

            RunAndWriteError(args);
        }

        private static void RunAndWriteError(string args)
        {
            using (Process process = RunProcess(args))
            {
                if (process.ExitCode == 0)
                    return;

                string errorData = process.StandardOutput.ReadToEnd();

                Log.Write(errorData);
            }
        }

        private static bool IsRuleExistsXp()
        {
            using (Process process = RunProcess("firewall show allowedprogram"))
            {
                string outData = process.StandardOutput.ReadToEnd();

                return outData.Contains(xpRuleName);
            }
        }

        private static void AddRuleVista(string direction, string ruleName)
        {
            if (IsRuleExistsVista(ruleName))
                return;

            const string createRuleFormat = "advfirewall firewall add rule name=\"{0}\" dir={1} action=allow program=\"{2}\" enable=yes profile=any";

            string args = createRuleFormat.Combine(ruleName, direction, exeFile);

            using (Process createRuleProcess = RunProcess(args))
            {
                string errorData = createRuleProcess.StandardOutput.ReadToEnd();

                if (createRuleProcess.ExitCode == 0)
                    return;

                Log.Write(errorData);
            }
        }

        private static bool IsRuleExistsVista(string ruleName)
        {
            const string getRuleDataTemplate = "advfirewall firewall show rule name=\"{0}\"";

            string getDataArgs = getRuleDataTemplate.Combine(ruleName);

            using (Process getRuleProcess = RunProcess(getDataArgs))
            {
                string outResult = getRuleProcess.StandardOutput.ReadToEnd();

                return outResult.Contains(ruleName);
            }
        }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]//Удалим выше
        private static Process RunProcess(string args)
        {
            var resultProcess = new Process();

            Log.Write("Starting process: netsh " + args); 
            var startInfo = new ProcessStartInfo("netsh", args);

            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;

            resultProcess.StartInfo = startInfo;

            resultProcess.Start();

            resultProcess.WaitForExit();


            return resultProcess;
        }
    }
}
