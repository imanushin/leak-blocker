using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.SystemTools.Win32.ResourceWrappers
{
    internal sealed class FilterInstanceSearchHandleWrapper : BaseResourceWrapper
    {
        internal FilterInstanceSearchHandleWrapper(IntPtr resource)
            : base(resource)
        {
        }

        protected override void CleanupResource(IntPtr resource)
        {
            uint error = NativeMethods.FilterInstanceFindClose(resource);
            if (error != NativeMethods.S_OK)
                Log.Write(NativeErrors.GetLastErrorMessage("FilterInstanceFindClose"));
        }
    }
}
