using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Server.Installer.InternalTools
{
    internal sealed class FileSystemConstants : IFileSystemConstants
    {
        private const string resultServerFile = "LeakBlocker.Server.exe";
        private const string resultAgentFile = "LeakBlocker.Agent.Distributive.exe";
        private const string resultAdminViewFile = "LeakBlocker.AdminView.exe";
        private const string resultAdminViewLnk = "Leak Blocker.url";
        private const string resultInstallerFile = "LeakBlocker.Installer.exe";

        private static readonly string resultDirectory = SharedObjects.Constants.CurrentVersionProgramFilesFolder;
        private static readonly string productMainMenuDir = Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu);

        private static readonly string serverExePath = Path.Combine(resultDirectory, resultServerFile);
        private static readonly string agentExePath = Path.Combine(resultDirectory, resultAgentFile);
        private static readonly string adminViewExePath = Path.Combine(resultDirectory, resultAdminViewFile);
        private static readonly string adminViewMainMenuLinkPath = Path.Combine(productMainMenuDir, resultAdminViewLnk);
        private static readonly string installerExePath = Path.Combine(resultDirectory, resultInstallerFile);

        public string ProductMainMenuDirectory
        {
            get
            {
                return productMainMenuDir;
            }
        }

        public string InstallDirectory
        {
            get
            {
                return resultDirectory;
            }
        }

        public string ServicePath
        {
            get
            {
                return serverExePath;
            }
        }

        public string AgentPath
        {
            get
            {
                return agentExePath;
            }
        }

        public string AdminViewPath
        {
            get
            {
                return adminViewExePath;
            }
        }

        public string AdminViewStartMenuLink
        {
            get
            {
                return adminViewMainMenuLinkPath;
            }
        }

        public string UninstallerPath
        {
            get
            {
                return installerExePath;
            }
        }
    }
}
