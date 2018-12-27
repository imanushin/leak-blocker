using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Implementations;
using LeakBlocker.Libraries.Common.SystemTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedTestLibrary;
using System.Linq;
using LeakBlocker.Libraries.Common.Collections;

namespace ExternalTests
{
    [TestClass]
    public sealed class WcfTestInvoker
    {
        private static readonly string[] computers =
            new[] { "8x64", "vista86EN", "2003server86en", "2003server64en", "xp86en", "WIN-45I9EEK2REL" };

        private static readonly string mainModuleFile = Assembly.GetExecutingAssembly().Location;
        private static readonly string currentFolder = Directory.GetParent(mainModuleFile).FullName;
        private static readonly string binariesRelativePath = Path.Combine(currentFolder, "..\\..\\..\\Projects\\Binaries\\");
        private static readonly string binariesPath = new DirectoryInfo(binariesRelativePath).FullName;

        private static readonly string externalTestResultsFolder = Path.Combine(currentFolder, "..\\..\\..\\ExternalTestResults");

        [TestMethod]
        [Timeout(TestTimeout.Infinite)]
        public void ExternalInvoker()
        {
            if (!string.Equals("tfs", Environment.MachineName, StringComparison.OrdinalIgnoreCase))
                return;

            SharedObjects.Singletons.Constants.SetTestImplementation(new Constants());
            SharedObjects.Factories.ThreadPool.EnqueueConstructor(val => new NativeThreadPool(val));
            SharedObjects.Singletons.ExceptionSuppressor.SetTestImplementation(new ExceptionSuppressor());

            var totalBytes = new List<byte>();
            var totalFiles = new List<InputFile>();

            foreach (string filePath in ReadFiles(binariesPath))
            {
                byte[] fileEntry = File.ReadAllBytes(filePath);
                string fileName = Path.GetFileName(filePath);

                var fileData = new InputFile(fileName, totalBytes.Count, fileEntry.Length);
                totalFiles.Add(fileData);
                totalBytes.AddRange(fileEntry);
            }

            if (!Directory.Exists(externalTestResultsFolder))
                Directory.CreateDirectory(externalTestResultsFolder);

            byte[] fileEntries = totalBytes.ToArray();
            var fileDatas = totalFiles.ToReadOnlySet();

            computers.ParallelForEach(comp => RunTest(comp, fileEntries, fileDatas));
        }

        private static void RunTest(string computerName, byte[] fileEntries, ReadOnlySet<InputFile> fileData)
        {
            string text = string.Empty;

            try
            {
                using (ITestInvokerService client = new TestInvokerClient(computerName))
                {
                    var testResult = client.RunTests(fileEntries, fileData);

                    string resultsFileName = Path.Combine(externalTestResultsFolder, computerName + ".trx");

                    text = Encoding.UTF8.GetString(testResult.TestResultsFileEntry.ToArray());

                    File.WriteAllText(resultsFileName, text);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("There were error during connecting to computer {0}. See inner exception for details".Combine(computerName), ex);
            }

            if (text.Contains("outcome=\"Failed\"", StringComparison.OrdinalIgnoreCase))
                Assert.Fail("Some tests on the computer {0} was failed. See details in the folder like {1}".Combine(computerName, externalTestResultsFolder));
        }


        private static IEnumerable<string> ReadFiles(string folder)
        {
            var fileNames = Directory.EnumerateFiles(folder, "*.dll", SearchOption.TopDirectoryOnly)
                .Union(Directory.EnumerateFiles(folder, "*.exe", SearchOption.TopDirectoryOnly));

            fileNames = fileNames.Where(fileName => !fileName.Contains("LeakBlocker.Installer.Full.exe", StringComparison.OrdinalIgnoreCase));

            return fileNames;
        }
    }
}
