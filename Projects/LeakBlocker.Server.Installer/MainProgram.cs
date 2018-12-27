using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Runtime.CompilerServices;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Equality;
using LeakBlocker.Server.Installer.InternalTools;
using LeakBlocker.Libraries.SystemTools;
using LeakBlocker.Libraries.Common.Resources;
using System.Windows.Forms;

namespace LeakBlocker.Server.Installer
{
    internal static class MainProgram
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int EntryPoint(string[] args)
        {
            Log.Write("Installation started");
            Console.Out.WriteLine("Installation started");

            try
            {
                if (args.Length < 1)
                {
                    Console.Out.WriteLine("Please specify one of the possible arguments:  /install or /remove");

                    return 1;
                }

                switch (args[0].ToUpperInvariant())
                {
                    case "/REMOVE":
                        RemoveProduct();

                        Log.Write("Removing succeeded");

                        Console.Out.WriteLine("Removing succeeded");

                        return 0;

                    case "/INSTALL":
                        InstallProduct();

                        Log.Write("Installation succeeded");

                        Console.Out.WriteLine("Installation succeeded");

                        return 0;

                    default:
                        Console.Out.WriteLine("Please specify one of the possible arguments:  /install or /remove");

                        return 1;
                }
            }
            catch (Exception ex)
            {
                Log.Write("Installation failed: " + ex);

                Console.Out.WriteLine("Installation failed: " + ex);

                return 1;
            }
        }

        private static void RemoveProduct()
        {
            DialogResult result = MessageBox.Show(ServerStrings.UninstallerWarningText.Combine(SharedObjects.Constants.Version.ToString(3)), 
                ServerStrings.UninstallerHeader, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2, 0);
            if (result == DialogResult.No)
                return;

            foreach (var process in Process.GetProcessesByName("LeakBlocker.AdminView"))
            {
                using (process)
                {
                    SharedObjects.ExceptionSuppressor.Run(process.Kill);
                }
            }

            InternalObjects.ServiceHelper.Uninstall();

            InternalObjects.ProductRegistrator.UnregisterProduct();

            SharedObjects.ExceptionSuppressor.Run(RemoveFile, InternalObjects.FileSystemConstants.AdminViewStartMenuLink);
            SharedObjects.ExceptionSuppressor.Run(RemoveFile, InternalObjects.FileSystemConstants.ServicePath);
            SharedObjects.ExceptionSuppressor.Run(RemoveFile, InternalObjects.FileSystemConstants.AgentPath);
            SharedObjects.ExceptionSuppressor.Run(RemoveFile, InternalObjects.FileSystemConstants.AdminViewPath);

            SharedObjects.ExceptionSuppressor.Run(RemoveFile, InternalObjects.FileSystemConstants.InstallDirectory + "sqlcese40.dll");
            SharedObjects.ExceptionSuppressor.Run(RemoveFile, InternalObjects.FileSystemConstants.InstallDirectory + "sqlceqp40.dll");
            SharedObjects.ExceptionSuppressor.Run(RemoveFile, InternalObjects.FileSystemConstants.InstallDirectory + "sqlceme40.dll");
            SharedObjects.ExceptionSuppressor.Run(RemoveFile, InternalObjects.FileSystemConstants.InstallDirectory + "sqlceer40EN.dll");
            SharedObjects.ExceptionSuppressor.Run(RemoveFile, InternalObjects.FileSystemConstants.InstallDirectory + "sqlcecompact40.dll");
            SharedObjects.ExceptionSuppressor.Run(RemoveFile, InternalObjects.FileSystemConstants.InstallDirectory + "sqlceca40.dll");
            SharedObjects.ExceptionSuppressor.Run(RemoveFile, InternalObjects.FileSystemConstants.InstallDirectory + "msvcr90.dll");
            SharedObjects.ExceptionSuppressor.Run(RemoveFile, InternalObjects.FileSystemConstants.InstallDirectory + "Microsoft.VC90.CRT.manifest");
            SharedObjects.ExceptionSuppressor.Run(RemoveFile, InternalObjects.FileSystemConstants.InstallDirectory + "System.Data.SqlServerCe.dll");
            SharedObjects.ExceptionSuppressor.Run(RemoveFile, InternalObjects.FileSystemConstants.InstallDirectory + "System.Data.SqlServerCe.Entity.dll");

            SystemObjects.FileTools.RemoveCurrentExecutable();
        }

        private static void RemoveFile(string fileName)
        {
            if (File.Exists(fileName))
                File.Delete(fileName);
        }

        private static void InstallProduct()
        {
            if (!Directory.Exists(InternalObjects.FileSystemConstants.InstallDirectory))
                Directory.CreateDirectory(InternalObjects.FileSystemConstants.InstallDirectory);

            string removeOldString = InternalObjects.ProductRegistrator.GetRemoveString();

            if (!string.IsNullOrWhiteSpace(removeOldString))//Это значит мы знаем схему удаления
            {
                Log.Write("Product had already been installed. Removing old program using command line: {0}".Combine(removeOldString));

                SharedObjects.ExceptionSuppressor.Run(RemoveOldProduct, removeOldString);

                Log.Write("Current version was successfully removed");
            }

            WriteFile(InternalObjects.FileSystemConstants.ServicePath, EmbeddedExecutables.LeakBlocker_Server);
            WriteFile(InternalObjects.FileSystemConstants.AgentPath, EmbeddedExecutables.LeakBlocker_Agent);
            WriteFile(InternalObjects.FileSystemConstants.AdminViewPath, EmbeddedExecutables.LeakBlocker_AdminView);

            CreateLinkToAdminView();

            WriteFile(InternalObjects.FileSystemConstants.InstallDirectory + "sqlcese40.dll", Environment.Is64BitOperatingSystem ? SQL.sqlcese40_64 : SQL.sqlcese40_32);
            WriteFile(InternalObjects.FileSystemConstants.InstallDirectory + "sqlceqp40.dll", Environment.Is64BitOperatingSystem ? SQL.sqlceqp40_64 : SQL.sqlceqp40_32);
            WriteFile(InternalObjects.FileSystemConstants.InstallDirectory + "sqlceme40.dll", Environment.Is64BitOperatingSystem ? SQL.sqlceme40_64 : SQL.sqlceme40_32);
            WriteFile(InternalObjects.FileSystemConstants.InstallDirectory + "sqlceer40EN.dll", Environment.Is64BitOperatingSystem ? SQL.sqlceer40EN_64 : SQL.sqlceer40EN_32);
            WriteFile(InternalObjects.FileSystemConstants.InstallDirectory + "sqlcecompact40.dll", Environment.Is64BitOperatingSystem ? SQL.sqlcecompact40_64 : SQL.sqlcecompact40_32);
            WriteFile(InternalObjects.FileSystemConstants.InstallDirectory + "sqlceca40.dll", Environment.Is64BitOperatingSystem ? SQL.sqlceca40_64 : SQL.sqlceca40_32);
            WriteFile(InternalObjects.FileSystemConstants.InstallDirectory + "msvcr90.dll", Environment.Is64BitOperatingSystem ? SQL.msvcr90_64 : SQL.msvcr90_32);
            WriteFile(InternalObjects.FileSystemConstants.InstallDirectory + "Microsoft.VC90.CRT.manifest", Environment.Is64BitOperatingSystem ? SQL.manifest_64 : SQL.manifest_32);
            WriteFile(InternalObjects.FileSystemConstants.InstallDirectory + "System.Data.SqlServerCe.dll", SQL.System_Data_SqlServerCe);
            WriteFile(InternalObjects.FileSystemConstants.InstallDirectory + "System.Data.SqlServerCe.Entity.dll", SQL.System_Data_SqlServerCe_Entity);

            string currentFile = Process.GetCurrentProcess().MainModule.FileName;

            WriteFile(InternalObjects.FileSystemConstants.UninstallerPath, File.ReadAllBytes(currentFile));

            InternalObjects.ProductRegistrator.RegisterProduct();

            InternalObjects.ServiceHelper.Install();
        }

        private static void RemoveOldProduct(string removeOldString)
        {
            ReadOnlyList<string> arguments = SharedObjects.CommandLine.Split(removeOldString);

            string commandLineArguments = SharedObjects.CommandLine.CreateArguments(arguments.Skip(1).ToReadOnlyList());

            using (Process process = Process.Start(arguments.First(), commandLineArguments))
            {
                Check.ObjectIsNotNull(process);

                process.WaitForExit();
            }
        }

        private static void CreateLinkToAdminView()
        {
            if (!Directory.Exists(InternalObjects.FileSystemConstants.ProductMainMenuDirectory))
                Directory.CreateDirectory(InternalObjects.FileSystemConstants.ProductMainMenuDirectory);

            using (Stream outFile = File.Open(InternalObjects.FileSystemConstants.AdminViewStartMenuLink, FileMode.Create))
            {
                using (var writer = new StreamWriter(outFile))
                {
                    string app = InternalObjects.FileSystemConstants.AdminViewPath;
                    writer.WriteLine("[InternetShortcut]");
                    writer.WriteLine("URL=file:///" + app);
                    writer.WriteLine("IconIndex=0");
                    string icon = app.Replace('\\', '/');
                    writer.WriteLine("IconFile=" + icon);
                    writer.Flush();
                }
            }
        }

        private static void WriteFile(string fileName, byte[] fileEntry)
        {
            try
            {
                if (File.Exists(fileName) && EnumerableComparer.Compare(File.ReadAllBytes(fileName), fileEntry))
                    return;

                File.WriteAllBytes(fileName, fileEntry);
            }
            catch (Exception ex)
            {
                Log.Write("Unable to write file {0}:", fileName);
                Log.Write(ex);

                throw;//Бросаем, ибо адекватным метом уже проверили
            }
        }
    }
}