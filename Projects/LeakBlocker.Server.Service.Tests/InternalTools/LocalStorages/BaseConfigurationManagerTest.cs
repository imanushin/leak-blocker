using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Tests;

namespace LeakBlocker.Server.Service.Tests.InternalTools.LocalStorages
{
    public abstract class BaseConfigurationManagerTest : BaseTest
    {

        protected static void CleanupDirectory()
        {
            string directory = SharedObjects.Constants.UserDataFolder;

            if (Directory.Exists(directory))
                Directory.EnumerateFiles(directory).ForEach(File.Delete);
        }
    }
}
