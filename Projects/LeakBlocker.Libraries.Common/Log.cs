using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using LeakBlocker.Libraries.Common.IoC;

namespace LeakBlocker.Libraries.Common
{
    /// <summary>
    /// Class for writing log messages to file. Files are stored in ProgramData folder. Files are written in Unicode encoding.
    /// </summary>
    public static class Log
    {
        private const string detailedLogKeyFile = "DEBUG";

        private const int fileCount = 4;
        private const int fileLength = 64 * 1024 * 1024;

        private const string dateFormat = "yyyy-MM-dd HH.mm.ss.fff";
        private const string fileNameFormat = "{0} {1} {2} {4}@{3}.log";

        private const string messageTemplate =
            "{0} {1} {2} >> {3}";

        private const string headerTemplate =
@"Log v1.1
   OS:               {0}
   Local time:       {1}
   UTC time:         {2}
   Assembly:         {3}
   Assembly version: {4}
{5}";

        private const string continuationInfoTemplate =
@"
Continuation of {0}
";

        #region LogParameters

        private sealed class LogParameters
        {
            internal string MachineName
            {
                get;
                private set;
            }

            internal string VersionString
            {
                get;
                private set;
            }

            internal string ExecutablePath
            {
                get;
                private set;
            }

            internal string ExecutableName
            {
                get;
                private set;
            }

            internal string LogFolder
            {
                get;
                private set;
            }

            internal bool Disable
            {
                get;
                private set;
            }

            internal bool Detailed
            {
                get;
                private set;
            }

            internal LogParameters()
            {
                try
                {
                    Disable = TestContextData.IsTestContext;

                    if (Disable)
                        return;
                    
                    Detailed = File.Exists(SharedObjects.Constants.MainModuleFolder + detailedLogKeyFile);
                    ExecutablePath = SharedObjects.Constants.MainModulePath;
                    ExecutableName = Path.GetFileNameWithoutExtension(ExecutablePath);
                    MachineName = Environment.MachineName;
                    VersionString = SharedObjects.Constants.VersionString;
                    LogFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\Delta Corvi\Logs\";
#if DEBUG
                    Detailed = true;
#endif
                }
                catch
                {
                    if (Debugger.IsAttached)
                        Debugger.Break();
                }
            }
        }

        #endregion

        private static readonly LogParameters parameters = new LogParameters();

        private static string targetFile = string.Empty;
        private static FileStream stream;
        private static StreamWriter writer;
        private static int currentFileLength;

        /// <summary>
        /// Writes exception.
        /// </summary>
        /// <param name="exception">Exception.</param>
        public static void Write(Exception exception)
        {
            if (exception != null)
                WriteData(exception + Environment.NewLine + "Full call stack:" + Environment.NewLine + new StackTrace(true));
        }

        /// <summary>
        /// Replaces the format items in a specified string with the string interpretations of corresponding objects 
        /// in a specified array and writes the result string to the log file.
        /// </summary>
        /// <param name="text">A composite format string.</param>
        /// <param name="arguments">An object array that contains one or more objects to format.</param>
        [StringFormatMethod("text")]
        public static void Write(string text, params object[] arguments)
        {
            WriteData(text, arguments);
        }

        /// <summary>
        /// Replaces the format items in a specified string with the string interpretations of corresponding objects 
        /// in a specified array and writes the result string to the log file.
        /// </summary>
        /// <param name="text">A composite format string.</param>
        /// <param name="arguments">An object array that contains one or more objects to format.</param>
        [StringFormatMethod("text")]
        public static void Add(string text, params object[] arguments)
        {
            if (!parameters.Detailed)
                return;

            Write(text, arguments);
        }
        
        private static void WriteData(string text, params object[] arguments)
        {
            lock (parameters)
            {
                if (parameters.Disable)
                    return;

                try
                {
                    string formattedString = (arguments.Length == 0) ? text : string.Format(CultureInfo.InvariantCulture, text, arguments);

                    if (string.IsNullOrEmpty(formattedString))
                        return;

                    string line = string.Format(CultureInfo.InvariantCulture, messageTemplate,
                        DateTime.Now.ToString(dateFormat, CultureInfo.InvariantCulture), GetCaller(),
                        Thread.CurrentThread.ManagedThreadId.ToString(CultureInfo.InvariantCulture), formattedString);

                    if (string.IsNullOrEmpty(targetFile) || ((currentFileLength + line.Length) > fileLength))
                        StartNewFile();

                    writer.WriteLine(line);
                    writer.Flush();
                    stream.Flush();

                    currentFileLength += line.Length;
                }
                catch
                {
                    if (Debugger.IsAttached)
                        Debugger.Break();
                }
            }
        }

        private static string GetCaller()
        {
            if (!parameters.Detailed)
                return string.Empty;

            var stackTrace = new StackTrace();
            StackFrame stackFrame = stackTrace.GetFrame(3);
            MethodBase method = stackFrame.GetMethod();
            Type classType = method.ReflectedType;

            string classTypeName = classType.FullName;
            string methodName = method.Name;

            string debugData = string.Empty;
            int lineNumber = stackFrame.GetFileLineNumber();
            string fileName = stackFrame.GetFileName();
            if ((lineNumber > 0) && !string.IsNullOrEmpty(fileName))
                debugData = "(" + fileName + ":" + lineNumber.ToString(CultureInfo.InvariantCulture) + ")";

            string result = classTypeName + "." + methodName + debugData;

            return result;
        }

        private static string GetFileHeader(string oldFileName)
        {
            string checkedOldFileName = string.Empty;
            if (!string.IsNullOrEmpty(oldFileName))
                checkedOldFileName = string.Format(CultureInfo.InvariantCulture, continuationInfoTemplate, oldFileName);
            string result = string.Format(CultureInfo.InvariantCulture, headerTemplate,
                Environment.OSVersion.VersionString,
                DateTime.Now.ToString(CultureInfo.InvariantCulture),
                DateTime.UtcNow.ToString(CultureInfo.InvariantCulture),
                parameters.ExecutablePath,
                parameters.VersionString,
                checkedOldFileName);
            return result;
        }

        private static void StartNewFile()
        {
            if (writer != null)
            {
                try
                {
                    writer.Flush();
                    try
                    {
                        writer.Dispose();
                    }
                    catch
                    {
                    }
                }
                catch
                {
                }
            }

            string fileName = string.Format(CultureInfo.InvariantCulture, fileNameFormat, parameters.MachineName,
                parameters.ExecutableName, DateTime.UtcNow.ToString(dateFormat, CultureInfo.InvariantCulture), Environment.UserDomainName, Environment.UserName);
            string oldFileName = targetFile;
            targetFile = parameters.LogFolder + fileName;
            if (!Directory.Exists(parameters.LogFolder))
                Directory.CreateDirectory(parameters.LogFolder);
            DeleteOldFiles();

            stream = File.Open(targetFile, FileMode.Create, FileAccess.Write, FileShare.Read);
            writer = new StreamWriter(stream, Encoding.Unicode);

            string header = GetFileHeader(oldFileName);

            writer.WriteLine(header);
            writer.Flush();
            stream.Flush();

            currentFileLength = header.Length;
        }

        private static void DeleteOldFiles()
        {
            string[] files = Directory.GetFiles(parameters.LogFolder);
            var sortedFiles = new List<KeyValuePair<string, DateTime>>();

            foreach (string currentFilePath in files)
            {
                try
                {
                    string fileName = Path.GetFileNameWithoutExtension(currentFilePath);

                    if (fileName == null)
                        continue;
                    if (fileName.Length != (parameters.MachineName.Length + 1 + parameters.ExecutableName.Length + 1 + dateFormat.Length + 2 + Environment.UserDomainName.Length + Environment.UserName.Length))
                        continue;
                    if (!fileName.StartsWith(parameters.MachineName + " " + parameters.ExecutableName, StringComparison.OrdinalIgnoreCase))
                        continue;
                    if (!fileName.EndsWith(Environment.UserName + "@" + Environment.UserDomainName, StringComparison.OrdinalIgnoreCase))
                        continue;

                    try
                    {
                        DateTime.ParseExact(fileName.Substring(parameters.MachineName.Length + 1 + parameters.ExecutableName.Length + 1, dateFormat.Length), dateFormat, CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        continue;
                    }

                    DateTime currentDate = File.GetCreationTimeUtc(currentFilePath);
                    var newItem = new KeyValuePair<string, DateTime>(currentFilePath, currentDate);
                    sortedFiles.Add(newItem);
                }
                catch
                {
                    if (Debugger.IsAttached)
                        Debugger.Break();
                }
            }

            bool flag = true;
            while (flag)
            {
                flag = false;
                for (int i = 0; i < (sortedFiles.Count - 1); i++)
                {
                    if (sortedFiles[i].Value <= sortedFiles[i + 1].Value)
                        continue;

                    KeyValuePair<string, DateTime> temp = sortedFiles[i];
                    sortedFiles[i] = sortedFiles[i + 1];
                    sortedFiles[i + 1] = temp;
                    flag = true;
                }
            }

            for (int i = 0; i < (sortedFiles.Count - (fileCount - 1)); i++)
            {
                try
                {
                    File.Delete(sortedFiles[i].Key);
                }
                catch
                {
                    if (Debugger.IsAttached)
                        Debugger.Break();
                }
            }
        }
    }
}

