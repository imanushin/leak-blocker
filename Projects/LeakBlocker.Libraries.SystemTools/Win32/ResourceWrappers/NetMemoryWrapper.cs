using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.SystemTools.Win32.ResourceWrappers
{
    internal sealed class NetMemoryWrapper : BaseResourceWrapper
    {
        internal NetMemoryWrapper(IntPtr resource)
            : base(resource)
        {
        }
        
        protected override void CleanupResource(IntPtr resource)
        {
            uint error = NativeMethods.NetApiBufferFree(resource);
            if (error != NativeMethods.NERR_Success)
                Log.Write(NativeErrors.GetMessage("NetApiBufferFree", error));
        }
    }
}
