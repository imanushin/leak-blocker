using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeakBlocker
{
    internal static class EntryPoint
    {
        internal static int Main(string[] parameters)
        {
            return Program.Start(parameters);
        }
    }
}
