using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.SystemTools.Win32.ResourceWrappers
{
    internal sealed class LocalObjectMemoryWrapper : BaseResourceWrapper
    {
        internal LocalObjectMemoryWrapper(IntPtr resource)
            : base(resource)
        {
        }

        protected override void CleanupResource(IntPtr resource)
        {
            IntPtr handle = NativeMethods.LocalFree(resource);
            if (handle != IntPtr.Zero)
                Log.Write(NativeErrors.GetLastErrorMessage("LocalFree"));
        }
    }
}
