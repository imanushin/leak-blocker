using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.SystemTools.Win32.ResourceWrappers
{
    internal sealed class FilterVolumeSearchHandleWrapper : BaseResourceWrapper
    {
        internal FilterVolumeSearchHandleWrapper(IntPtr resource)
            : base(resource)
        {
        }
        
        protected override void CleanupResource(IntPtr resource)
        {
            uint error = NativeMethods.FilterVolumeFindClose(resource);
            if (error != NativeMethods.S_OK)
                Log.Write(NativeErrors.GetLastErrorMessage("FilterVolumeFindClose"));
        }
    }
}
