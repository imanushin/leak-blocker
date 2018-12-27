using System;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.SystemTools.Win32.ResourceWrappers
{
    internal sealed class LsaMemoryWrapper : BaseResourceWrapper
    {
        public LsaMemoryWrapper(IntPtr resource)
            : base(resource)
        {
        }

        protected override void CleanupResource(IntPtr resource)
        {
            uint error = NativeMethods.LsaFreeMemory(resource);
            if (error != NativeMethods.STATUS_SUCCESS)
                Log.Write(NativeErrors.GetMessage("LsaFreeMemory", error));
        }
    }
}
