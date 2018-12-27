using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.CommonInterfaces;

namespace LeakBlocker.Libraries.Common.Tests
{
    internal sealed class ConstantsStub : IConstants
    {
        private static readonly long classCreateTime = DateTime.UtcNow.Ticks;

        public TimeSpan DefaultTcpTimeout
        {
            get
            {
                return TimeSpan.FromMinutes(1);
            }
        }

        public string UserDataFolder
        {
            get
            {
                return GetFolderPath("UserDataFolder");
            }
        }

        private static string GetFolderPath(string propertyName)
        {
            string tempFolder = Path.Combine(Environment.CurrentDirectory, "TestTempFolder");

            string subFolder = string.Format("DeltaCorviTest_{1}_{0}", classCreateTime, propertyName);

            return Path.Combine(tempFolder, subFolder);
        }

        public int DefaultTcpPort
        {
            get
            {
                return 49584;
            }
        }

        public string CurrentVersionProgramFilesFolder
        {
            get
            {
                return GetFolderPath("CurrentVersionProgramFilesFolder");
            }
        }

        public string MainModuleFolder
        {
            get
            {
                return GetFolderPath("MainModuleFolder");
            }
        }

        public string MainModulePath
        {
            get
            {
                return GetFolderPath("MainModulePath");
            }
        }

        public string TemporaryFolder
        {
            get
            {
                return "qwerty";
            }
        }

        public Version Version
        {
            get
            {
                return new Version(1, 1, 1);
            }
        }

        public string VersionString
        {
            get
            {
                return "qwerty";
            }
        }

        public NativeImageType CurrentProcessImageType
        {
            get
            {
                return ((IntPtr.Size == 8) ? NativeImageType.X64 : NativeImageType.X86);
            }
        }


        public string MainModuleFile
        {
            get
            {
                return Path.GetFileName(MainModulePath);
            }
        }
    }
}