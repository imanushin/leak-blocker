using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.SystemTools.Win32.ResourceWrappers
{
    internal sealed class NativeHandleWrapper : BaseResourceWrapper
    {
        internal NativeHandleWrapper(IntPtr resource)
            : base(resource)
        {
        }
  
        protected override void CleanupResource(IntPtr resource)
        {
            if (!NativeMethods.CloseHandle(resource))
                Log.Write(NativeErrors.GetLastErrorMessage("CloseHandle"));
        }
    }
}
