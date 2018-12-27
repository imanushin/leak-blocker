using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Storage
{
    internal interface IDatabaseConstants
    {
        string DatabaseName
        {
            get;
        }

        string DatabasePath
        {
            get;
        }

        string DatabaseFolder
        {
            get;
        }
    }
}