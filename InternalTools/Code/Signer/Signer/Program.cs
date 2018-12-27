using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Signer
{
    internal static class Program
    {
        private const string productionSignTemplate =
            "sign /v /a /ph /ac \"{1}DigiCert High Assurance EV Root CA.crt\" /n \"Delta Corvi LLC\" /d \"Leak Blocker\" /du \"http://www.leakblocker.com\" /t \"http://timestamp.digicert.com\" \"{0}\"";

        private const string testSignTemplate =
            "sign /v /f \"{1}test.pfx\" /p Qwerty1 /d \"Leak Blocker\" /du \"http://www.leakblocker.com\" /t \"http://timestamp.digicert.com\" \"{0}\"";

        private static int Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.Out.WriteLine("Please set the first parameter - full path to the signing file");

                return 1;
            }

            string targetFilePath = args[0];

            bool isBuildMachine = Environment.MachineName.ToUpperInvariant() == "TFS";

            if (!isBuildMachine && targetFilePath.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))//На Dev машинах не подписываем dll-ки
                return 0;

            string currentFolder = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

            string commonProperties = Path.Combine(currentFolder,"..\\..\\Projects\\CommonProperties\\");

            var signTool = new Process();

            signTool.StartInfo.FileName = Path.Combine(currentFolder, "signtool.exe");

            string argumentsTemplate;

            if (isBuildMachine || File.Exists(commonProperties + "sign"))
                argumentsTemplate = productionSignTemplate;
            else
                argumentsTemplate = testSignTemplate;

            signTool.StartInfo.Arguments = string.Format(argumentsTemplate, targetFilePath, commonProperties);

            signTool.StartInfo.RedirectStandardError = true;
            signTool.StartInfo.RedirectStandardOutput = true;
            signTool.StartInfo.UseShellExecute = false;

            signTool.Start();

            signTool.WaitForExit();

            if (0 == signTool.ExitCode)
                return 0;

            string errors = signTool.StandardError.ReadToEnd();
            string output = signTool.StandardOutput.ReadToEnd();

            Console.Error.WriteLine(output);
            Console.Error.WriteLine(errors);

            return 2;
        }
    }
}
