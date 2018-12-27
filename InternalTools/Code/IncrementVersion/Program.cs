using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.IO;
using System.Linq;

namespace IncrementVersion
{
    static class Program
    {
        private static readonly StringBuilderTraceListener listener = new StringBuilderTraceListener();

        /// <summary>
        /// 1. Путь к папке с файлами
        /// 2. Номер Check In'а.
        /// </summary>
        static int Main(string[] args)
        {
            Trace.Listeners.Add(listener);

            if (args.Length < 2)
                return 2;

            try
            {
                string targetDirectory = new DirectoryInfo(args[0]).FullName;
                string targetBuildNumber = args[1];

                Trace.WriteLine("Target changeset string: " + targetBuildNumber);

                string pathToManualVersion = Path.Combine(targetDirectory, "ManualVersion.txt");

                Trace.WriteLine("pathToManualVersion: " + pathToManualVersion);

                string manualVersionString = File.ReadAllLines(pathToManualVersion)[0];

                Trace.WriteLine("manualVersionString: " + manualVersionString);

                Version manualVersion = Version.Parse(manualVersionString);

                Trace.WriteLine("manualVersion: " + manualVersion);

                int buildNumber = int.Parse(targetBuildNumber);

                Trace.WriteLine("changeset: " + buildNumber);

                var newVersion = new Version(manualVersion.Major, manualVersion.Minor, manualVersion.Build, buildNumber);

                IEnumerable<string> files = Directory.EnumerateFiles(targetDirectory, "*.cs").Union(Directory.EnumerateFiles(targetDirectory, "*.h"));

                foreach (string file in files)
                {
                    Trace.WriteLine("Reading file " + file);

                    string[] fileEntry = File.ReadAllLines(file);

                    bool needUpdate = false;

                    for (int lineIndex = 0; lineIndex < fileEntry.Length; lineIndex++)
                    {
                        string currentLine = fileEntry[lineIndex];

                        int macroStartIndex = currentLine.IndexOf("/*VERSION_MACRO", StringComparison.OrdinalIgnoreCase);

                        if (macroStartIndex < 0)
                            continue;

                        int macroEndIndex = currentLine.IndexOf("*/", macroStartIndex, StringComparison.OrdinalIgnoreCase);

                        int templateStartIndex = macroStartIndex + "/*VERSION_MACRO".Length;

                        string template = currentLine.Substring(templateStartIndex, macroEndIndex - templateStartIndex);

                        string beforeTemplateString = currentLine.Substring(0, macroEndIndex + "*/".Length);

                        string resultString = beforeTemplateString + 
                            string.Format(
                            template, 
                            newVersion.Major,
                            newVersion.Minor,
                            newVersion.Build, 
                            newVersion.Revision);

                        fileEntry[lineIndex] = resultString;

                        needUpdate = true;
                    }

                    if(!needUpdate)
                        continue;

                    Trace.WriteLine("Writing file " + file);

                    FileAttributes oldAttributes = File.GetAttributes(file);
                    File.SetAttributes(file, oldAttributes & ~FileAttributes.ReadOnly);

                    Trace.WriteLine("Version was successfully updated for file " + file);

                    File.WriteAllLines(file, fileEntry);
                }

                Trace.WriteLine("Version was successfully updated.");
                return 0;
            }
            catch (Exception exception)
            {
                Trace.WriteLine("Error: " + exception + ".");

                string pathToCurrentExe = Assembly.GetExecutingAssembly().Location;
                string directoryWithExe = Directory.GetParent(pathToCurrentExe).FullName;

                File.WriteAllText(Path.Combine(directoryWithExe, "IncrementVersion.txt"), listener.ToString());

                return 1;
            }
        }
    }
}
