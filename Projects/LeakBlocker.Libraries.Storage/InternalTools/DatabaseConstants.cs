using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Storage.InternalTools
{
    internal sealed class DatabaseConstants : IDatabaseConstants
    {
        private static readonly string databaseName = "LeakBlockerDb_{0}.sdf".Combine(GetShortProcessName());

        private static string GetShortProcessName()
        {
            string exeName = Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.ModuleName);

            Check.ObjectIsNotNull(exeName);

            return exeName.Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries)[1];
        }

        public string DatabaseName
        {
            get
            {
                return databaseName;
            }
        }

        public string DatabasePath
        {
            get
            {
                return Path.Combine(DatabaseFolder, databaseName);
            }
        }

        public string DatabaseFolder
        {
            get
            {
                return SharedObjects.Constants.UserDataFolder;
            }
        }
    }
}
