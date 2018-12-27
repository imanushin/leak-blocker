using System;
using System.IO;
using LeakBlocker.Libraries.Common;
using Win32;

namespace LeakBlocker.Libraries.SystemTools.Network
{ 
    public static class SharedFolder
    {
        public static string Name
        {
            get
            {
                return "LeakBlocker_" + SharedObjects.Constants.VersionString;
            }
        }
    }
}
