using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace LeakBlocker.Libraries.CleanupTool
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Console.Write("Cleanup tool.");

            if (args.Length == 0)
            {
                Console.WriteLine("Incorrect parameters.");
                return;
            }

            Console.WriteLine("Target file: {0}.", args[0]);

            for (int i = 0; i < 50; i++)
            {
                Console.Write("Starting cleanup.");

                try
                {
                    if (!File.Exists(args[0]))
                    {
                        Console.WriteLine("File was already deleted.");
                        break;
                    }

                    File.Delete(args[0]);

                    string versionFolder = Path.GetDirectoryName(args[0]);
                    string productFolder = Path.GetDirectoryName(versionFolder);
                    string companyFolder = Path.GetDirectoryName(productFolder);

                    foreach (string folder in new string[] { versionFolder, productFolder, companyFolder })
                    {
                        Console.WriteLine("Removing directory {0}.", folder);

                        if (Directory.EnumerateFiles(folder).Count() != 0)
                        {
                            Console.WriteLine("Directory is not empty.");
                            break;
                        }

                        Directory.Delete(folder, true);
                    }

                    break;
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.ToString());
                    Console.Write("Waiting...");
                    Thread.Sleep(5000);
                }
            }

            Console.Write("Completed.");
        }
    }
}
