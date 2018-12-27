using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.CommonInterfaces;

namespace LeakBlocker.Libraries.Common.Implementations
{
    internal sealed class Constants : IConstants
    {
        public int DefaultTcpPort
        {
            get
            {
                return 49584;
            }
        }

        public TimeSpan DefaultTcpTimeout
        {
            get
            {
                return TimeSpan.FromMinutes(2);
            }
        }

        public string UserDataFolder
        {
            get;
            private set;
        }

        public string CurrentVersionProgramFilesFolder
        {
            get;
            private set;
        }

        public string MainModuleFolder
        {
            get;
            private set;
        }

        public string MainModulePath
        {
            get;
            private set;
        }

        public string MainModuleFile
        {
            get;
            private set;
        }

        public string TemporaryFolder
        {
            get;
            private set;
        }

        public Version Version
        {
            get;
            private set;
        }

        public string VersionString
        {
            get;
            private set;
        }

        public NativeImageType CurrentProcessImageType
        {
            get;
            private set;
        }

        internal Constants()
        {
            const string companyName = "Delta Corvi";
            const string productName = "Leak Blocker";

            var versionAttribute = Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyFileVersionAttribute)) as AssemblyFileVersionAttribute;
            Version = ((versionAttribute != null) ? new Version(versionAttribute.Version) : new Version());

            VersionString = string.Format(CultureInfo.InvariantCulture, "{0}_{1}_{2}",
                Version.Major, Version.Minor, Version.Build);
            CurrentProcessImageType = ((IntPtr.Size == 8) ? NativeImageType.X64 : NativeImageType.X86);

            TemporaryFolder = Path.GetTempPath().TrimEnd('\\') + '\\';
            UserDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\" + companyName + @"\" + productName + "\\";

            MainModulePath = Process.GetCurrentProcess().MainModule.FileName;
            MainModuleFolder = (Path.GetDirectoryName(MainModulePath) ?? string.Empty).TrimEnd('\\') + '\\';
            MainModuleFile = Path.GetFileName(MainModulePath);
            CurrentVersionProgramFilesFolder = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\" +
                companyName + @"\" + productName + @"\" + Version.ToString(3) + '\\';

            try
            {
                if (!Directory.Exists(UserDataFolder))
                    Directory.CreateDirectory(UserDataFolder);
            }
            catch
            {
            }
        }
    }
}
