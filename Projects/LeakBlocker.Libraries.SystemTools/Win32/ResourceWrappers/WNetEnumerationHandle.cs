using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.SystemTools.Win32.ResourceWrappers
{
    internal sealed class WNetEnumerationHandle : BaseResourceWrapper
    {
        internal WNetEnumerationHandle(IntPtr resource)
            : base(resource)
        {
        }

        protected override void CleanupResource(IntPtr resource)
        {
            uint error = NativeMethods.WNetCloseEnum(resource);
            if (error != NativeMethods.NO_ERROR)
                Log.Write(NativeErrors.GetMessage("WNetCloseEnum", error));
        }
    }
}
