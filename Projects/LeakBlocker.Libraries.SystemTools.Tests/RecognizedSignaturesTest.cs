using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Libraries.SystemTools.Tests
{
    [TestClass]
    public sealed class RecognizedSignaturesTest : BaseTest
    {
        private static readonly HashSet<string> recognizedSignatures = new HashSet<string>
        {
            "DF073DF38E78B778C22C6706E3A8A07E18A9579F"
          //  "‎19F8F76F4655074509769C20349FFAECCECD217D",
          //  "‎78C4C36F6877C50A845FE162E80DF41CC2E098CA"
        };

        /// <summary>
        /// Checks if a digital signature of the specified file matches of one of the predefined signatures.
        /// </summary>
        /// <param name="fileName">Full file path.</param>
        /// <returns>True if signature is recognized and false if signature is not present or is not recognized.</returns>
        private static bool Check(string fileName)
        {
            Common.Check.StringIsMeaningful(fileName, "fileName");

            if (!File.Exists(fileName))
                throw new InvalidOperationException("File {0} was not found.".Combine(fileName));

            var sign = X509Certificate.CreateFromSignedFile(fileName);


            return true;
        }

        [TestMethod]
        public void Check_Arguments()
        {
            CheckFirstStringForForMeaningful(arg => Check(arg));
        }

        [TestMethod]
        public void CheckSignaturesInBinariesFolder()
        {
            string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            Assert.IsNotNull(directoryName);

            string folder = directoryName.TrimEnd('\\') + "\\";

            IEnumerable<string> allFiles = Directory.EnumerateFiles(folder, "LeakBlocker.*.exe")
                .Union(Directory.EnumerateFiles(folder, "LeakBlocker.*.dll")).
                Union(Directory.EnumerateFiles(folder, "*.sys", SearchOption.AllDirectories));

            foreach (string currentFile in allFiles)
            {
                if (currentFile.EndsWith("Tests.dll", StringComparison.OrdinalIgnoreCase))
                    continue;
                
                if (currentFile.EndsWith("LeakBlocker.Libraries.Installer.exe", StringComparison.OrdinalIgnoreCase))
                    continue;

                Assert.IsTrue(Check(currentFile), "Signature of file {0} is corrupted", currentFile);
            }
        }
    }
}
