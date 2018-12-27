using System;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.SystemTools.Win32.ResourceWrappers
{
    internal sealed class LsaHandleWrapper : BaseResourceWrapper
    {
        public LsaHandleWrapper(IntPtr resource)
            : base(resource)
        {
        }

        protected override void CleanupResource(IntPtr resource)
        {
            uint error = NativeMethods.LsaClose(resource);
            if (error != NativeMethods.STATUS_SUCCESS)
                Log.Write(NativeErrors.GetMessage("LsaClose", error));
        }
    }
}
