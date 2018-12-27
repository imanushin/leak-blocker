using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.SystemTools.Win32.ResourceWrappers
{
    internal abstract class BaseResourceWrapper : IDisposable
    {
        private bool disposed;
        private readonly IntPtr unmanagedResource;
        
        internal BaseResourceWrapper(IntPtr resource)
        {
            unmanagedResource = resource;
        }

        protected abstract void CleanupResource(IntPtr resource);

        public void Dispose()
        {
            Cleanup();
            GC.SuppressFinalize(this);
        }

        ~BaseResourceWrapper()
        {
            Cleanup();
        }

        private void Cleanup()
        {
            if (disposed) 
                return;
            disposed = true;

            try
            {
                CleanupResource(unmanagedResource);
            }
            catch (Exception exception)
            {
                Log.Write(exception);
            }
        }
    }
}
