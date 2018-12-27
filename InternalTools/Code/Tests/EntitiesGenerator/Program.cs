using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using CommonObjectsGenerator;

namespace EntitiesGenerator
{
    internal static class Program
    {
        private static readonly StringBuilderTraceListener listener = new StringBuilderTraceListener();

        private static int Main(string[] args)
        {
            if (string.Equals("tfs", Environment.MachineName, StringComparison.InvariantCultureIgnoreCase))
                return 0;

            try
            {
                Trace.Listeners.Add(listener);

                if (args.Length < 1)
                {
                    const string error = "Wrong command line arguments. Please add first argument with the path to the projects folder";
                    Trace.WriteLine(error);

                    throw new ArgumentException(error, "args");
                }

                string inputPath = args[0];

                Trace.WriteLine(string.Format("Input path: {0}", inputPath));

                string pathToTheProject = new DirectoryInfo(inputPath).FullName;

                Trace.WriteLine(string.Format("Path to projects folder: {0}", pathToTheProject));

                string pathToBinaries = Path.Combine(pathToTheProject, "Binaries");

                Trace.WriteLine(string.Format("Path to binaries folder {0}", pathToBinaries));

                var databaseTypes = new HashSet<Type>(BaseObjectsFinder.GetDatabaseObjects(pathToBinaries));

                Type baseObjectType = BaseObjectsFinder.GetBaseObjectType(pathToBinaries);

                var generators = new IFileGenerator[]
                {
                    new DatabaseObjectsGenerator(baseObjectType), 
                    new DatabaseObjectsTestsGenerator(baseObjectType), 
                    new DatabaseModelInterfaceGenerator(baseObjectType), 
                    new DatabaseModelGenerator(baseObjectType) 
                };

                foreach (IFileGenerator generator in generators)
                {
                    string resultFileEntry = generator.CreateFileEntry(databaseTypes);

                    string projectFolder = Path.Combine(pathToTheProject, generator.ProjectName);

                    Trace.WriteLine("Project folder: " + projectFolder);

                    string resultFilePath = Path.Combine(projectFolder, generator.FileName);

                    Trace.WriteLine("Result file: " + resultFilePath);

                    if (File.Exists(resultFilePath))
                    {
                        string oldData = File.ReadAllText(resultFilePath);

                        if (oldData == resultFileEntry)
                            continue;

                        File.Delete(resultFilePath);
                    }

                    File.WriteAllText(resultFilePath, resultFileEntry);
                }

                return 0;
            }
            catch (ReflectionTypeLoadException ex)
            {
                foreach (Exception loaderException in ex.LoaderExceptions)
                {
                    Trace.WriteLine("Loader exception: " + loaderException);
                }

                Trace.WriteLine("Loaded types: " + string.Join(",", ex.Types.Where(type => type != null).Select(type => type.Name)));

                Console.Out.WriteLine(listener.ToString());

                return -1;
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Unhandled exception: " + ex);

                Console.Out.WriteLine(listener.ToString());

                return -1;
            }
        }
    }
}
