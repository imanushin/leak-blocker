using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.SystemTools.Win32.ResourceWrappers
{
    internal sealed class NtHandleWrapper : BaseResourceWrapper
    {
        internal NtHandleWrapper(IntPtr resource)
            : base(resource)
        {
        }

        protected override void CleanupResource(IntPtr resource)
        {
            uint error = NativeMethods.NtClose(resource);
            if (error != NativeMethods.STATUS_SUCCESS)
                Log.Write(NativeErrors.GetLastErrorMessage("NtClose"));
        }
    }
}
