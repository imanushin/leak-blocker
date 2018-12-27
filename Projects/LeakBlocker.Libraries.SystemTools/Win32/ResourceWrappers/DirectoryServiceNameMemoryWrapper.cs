using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.SystemTools.Win32.ResourceWrappers
{
    internal sealed class DirectoryServiceNameMemoryWrapper : BaseResourceWrapper
    {
        internal DirectoryServiceNameMemoryWrapper(IntPtr resource)
            : base(resource)
        {
        }

        protected override void CleanupResource(IntPtr resource)
        {
            NativeMethods.DsFreeNameResult(resource);
        }
    }
}
