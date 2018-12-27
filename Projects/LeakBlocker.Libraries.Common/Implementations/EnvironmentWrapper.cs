using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.CommonInterfaces;

namespace LeakBlocker.Libraries.Common.Implementations
{
    internal sealed class EnvironmentWrapper : IEnvironment
    {
        public ReadOnlyList<string> CommandLineArguments
        {
            get
            {
                return Environment.GetCommandLineArgs().ToReadOnlyList();
            }
        }
    }
}
