using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.SystemTools.Win32.ResourceWrappers
{
    internal sealed class DirectoryServiceBindHandle : BaseResourceWrapper
    {
        internal DirectoryServiceBindHandle(IntPtr resource)
            : base(resource)
        {
        }

        protected override void CleanupResource(IntPtr resource)
        {
            uint error = NativeMethods.DsUnBind(resource);
            if (error != 0)
                Log.Write(NativeErrors.GetMessage("DsUnBind", error));
        }
    }
}
