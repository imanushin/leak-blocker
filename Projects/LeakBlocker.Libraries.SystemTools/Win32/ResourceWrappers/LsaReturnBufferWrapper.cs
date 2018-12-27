using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.SystemTools.Win32.ResourceWrappers
{
    internal sealed class LsaReturnBufferWrapper : BaseResourceWrapper
    {
        internal LsaReturnBufferWrapper(IntPtr resource)
            : base(resource)
        {
        }
        
        protected override void CleanupResource(IntPtr resource)
        {
            uint error = NativeMethods.LsaFreeReturnBuffer(resource);
            if (error != NativeMethods.STATUS_SUCCESS)
                Log.Write(NativeErrors.GetMessage("LsaFreeReturnBuffer", error));
        }
    }
}
