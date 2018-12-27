using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.SystemTools.Win32.ResourceWrappers
{
    internal sealed class DirectoryServiceCredentials : BaseResourceWrapper
    {
        internal DirectoryServiceCredentials(IntPtr resource)
            : base(resource)
        {
        }

        protected override void CleanupResource(IntPtr resource)
        {
            NativeMethods.DsFreePasswordCredentials(resource);
        }
    }
}
