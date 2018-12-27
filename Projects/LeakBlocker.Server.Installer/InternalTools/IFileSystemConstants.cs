using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Server.Installer.InternalTools
{
    internal interface IFileSystemConstants
    {
        string ProductMainMenuDirectory
        {
            get;
        }

        string InstallDirectory
        {
            get;
        }

        string ServicePath
        {
            get;
        }

        string AgentPath
        {
            get;
        }

        string AdminViewPath
        {
            get;
        }

        string AdminViewStartMenuLink
        {
            get;
        }

        string UninstallerPath
        {
            get;
        }
    }
}
