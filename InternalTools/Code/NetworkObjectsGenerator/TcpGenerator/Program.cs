using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TcpGenerator
{
    internal static class Program
    {
        /// <summary>
        /// Параметры командной строки:
        /// 1. Instrumented dll - библиотека, которая содержит интерфейсы
        /// 3. Client Project Path - путь к проекту для генерации сервера
        /// 2. Server Project Path - путь к проекту для генерации клиентов
        /// </summary>
        private static int Main(string[] args)
        {
            var memoryStreamListener = new MemoryStreamListener();

            try
            {
                Trace.Listeners.Add(memoryStreamListener);

                if (args.Length < 1)
                {
                    const string error = "Wrong command line arguments.";
                    Trace.WriteLine(error);

                    throw new ArgumentException(error, "args");
                }

                string inputDllPath = args[0];

                Trace.WriteLine(string.Format("Input dll path: {0}", inputDllPath));

                string dllName = Path.GetFileNameWithoutExtension(inputDllPath);

                Trace.WriteLine(string.Format("Dll name: {0}", dllName));

                string serverProjectFolder = args[1];

                Trace.WriteLine(string.Format("Server project path: {0}", serverProjectFolder));

                string clientProjectFolder = args[2];

                Assembly targetAssembly = Assembly.LoadFrom(inputDllPath);

                var types = new HashSet<Type>(TypesFinder.FindTypes(targetAssembly));

                foreach (var generator in new BaseGenerator[]
                        {
                            new ServerGenerator(serverProjectFolder, dllName), 
                            new ClientGenerator(clientProjectFolder, dllName),
                        })
                {
                    string fileEntry = generator.GenerateFile(types);

                    string filePath = Path.Combine(generator.ProjectPath, generator.FileName);

                    if (File.Exists(filePath) && File.ReadAllText(filePath) == fileEntry)
                        continue;

                    File.WriteAllText(filePath, fileEntry);
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

                Console.Out.WriteLine(memoryStreamListener.GetString());

                return 1;
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Unhandled exception: " + ex);

                Console.Out.WriteLine(memoryStreamListener.GetString());

                return 1;
            }

        }

        private sealed class MemoryStreamListener : TraceListener
        {
            private readonly StringBuilder strings = new StringBuilder();

            public override void Write(string message)
            {
                strings.Append(message);
            }

            public override void WriteLine(string message)
            {
                strings.AppendLine(message);
            }

            public string GetString()
            {
                return strings.ToString();
            }
        }
    }
}
