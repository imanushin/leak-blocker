using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using SharedTestLibrary;
using TestHost.Generated;

namespace TestHost
{
    internal sealed class TestInvokerService : GeneratedTestInvokerService
    {
        private const string resultsTemplate = "{0}_results.trx";

        private const string localFolder = @"c:\tests";

        private static readonly string executablesFolder = Path.Combine(localFolder, "Executables");
        private static readonly string resultsFolder = Path.Combine(localFolder, "Results");

        private static readonly object syncRoot = new object();


        public TestInvokerService()
            : base(SessionManager.Instance)
        {
        }

        protected override TestsResultsData RunTests(byte[] fileEntries, ReadOnlySet<InputFile> inputFiles)
        {
            try
            {
                lock (syncRoot)
                {
                    if (Directory.Exists(executablesFolder))
                        Directory.Delete(executablesFolder, true);
                    if (Directory.Exists(resultsFolder))
                        Directory.Delete(resultsFolder, true);

                    Directory.CreateDirectory(executablesFolder);
                    Directory.CreateDirectory(resultsFolder);

                    foreach (InputFile inputFile in inputFiles)
                    {
                        string fullFileName = Path.Combine(executablesFolder, inputFile.FileName);

                        File.WriteAllBytes(fullFileName, inputFile.GetFileEntry(fileEntries));
                    }

                    return RunTestsFromFolder();
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);

                throw;
            }
        }


        private static TestsResultsData RunTestsFromFolder()
        {
            var directory = new DirectoryInfo(executablesFolder);

            IEnumerable<string> names = directory.EnumerateFiles("*.Tests.dll", SearchOption.TopDirectoryOnly).Select(file => file.Name);

            string testContainers = string.Join(" ", names.Select(name => string.Format("/testcontainer:{0}", name)));

            string resultFileFullPath = CreateFileNameUsingTemplate(resultsTemplate);

            if (File.Exists(resultFileFullPath))
                File.Delete(resultFileFullPath);

            using (var process = new Process())
            {
                Log.Add("Creating process start info...");
                
                var paths = new HashSet<string>
                        {
                            @"C:\Program Files (x86)\Microsoft Visual Studio 11.0\Common7\IDE\MsTest.exe",
                            @"C:\Program Files\Microsoft Visual Studio 11.0\Common7\IDE\MsTest.exe",
                            @"C:\Program Files (x86)\Microsoft Visual Studio 10.0\Common7\IDE\MsTest.exe",
                            @"C:\Program Files\Microsoft Visual Studio 10.0\Common7\IDE\MsTest.exe"
                        };

                string msTestExe = paths.FirstOrDefault(File.Exists);
                
                if (msTestExe == null)
                    throw new InvalidOperationException("Unable to retrieve environment variable 'MsTestPath'");

                process.StartInfo.WorkingDirectory = executablesFolder;
                process.StartInfo.FileName = msTestExe;
                process.StartInfo.Arguments = string.Format(@"{0} ""/resultsFile:{1}""", testContainers, resultFileFullPath);

                Log.Add("Starting new process for file {0}...".Combine(executablesFolder));

                process.Start();

                Log.Add("Process was started. Waiting for exit...");

                Thread.Sleep(1000);

                if (!process.HasExited)
                    process.WaitForExit();
            }

            byte[] resultsFileEntry = File.ReadAllBytes(resultFileFullPath);

            return new TestsResultsData(resultsFileEntry.ToReadOnlyList());
        }


        private static string CreateFileNameUsingTemplate(string template)
        {
            string name = string.Format(template, Environment.MachineName);

            return Path.Combine(resultsFolder, name);
        }

    }
}
