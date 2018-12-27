using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;

namespace LeakBlocker.Libraries.Common
{
    /// <summary>
    /// Base class that implements the recommended Disposable pattern.
    /// </summary>
    public abstract class Disposable : IDisposable
    {
        private readonly object synchronization = new object();

        /// <summary>
        /// Indicates if object was disposed.
        /// </summary>
        public bool Disposed
        {
            get;
            private set;
        }

        /// <summary>
        /// Calls the appropriate methods for releasing resources. 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Method for releasing managed resources.
        /// </summary>
        protected virtual void DisposeManaged()
        {
        }

        /// <summary>
        /// Method for releasing unmanaged resources.
        /// </summary>
        protected virtual void DisposeUnmanaged()
        {
        }

        /// <summary>
        /// Throws exception if the current object is disposed.
        /// </summary>
        protected void ThrowIfDisposed()
        {
            if (Disposed)
                Exceptions.Throw(ErrorMessage.ObjectDisposed);
        }

        /// <summary>
        /// Called if Dispose was invoked from finally block of using directive.
        /// </summary>
        protected virtual void DisposedAfterException()
        {
        }

        /// <summary>
        /// Calls the appropriate methods for releasing resources. 
        /// </summary>
        ~Disposable()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (synchronization == null)
            {
                DisposeWithoutLock(disposing);
            }
            else
            {
                lock (synchronization)
                {
                    DisposeWithoutLock(disposing);
                }
            }
        }

        private void DisposeWithoutLock(bool disposing)
        {
            if (Disposed)
                return;

            Disposed = true;
            
            try
            {
                if (ExceptionExtensions.UnhandledExceptionContext)
                    DisposedAfterException();

                if (disposing)
                    DisposeManaged();
              
                DisposeUnmanaged();
            }
            catch (Exception exception)
            {
                Log.Write(exception);
            }
        }

        /// <summary>
        /// Calls dispose method of the target object suppressing all exceptions.
        /// </summary>
        /// <param name="target">Target object.</param>
        public static void DisposeSafe(IDisposable target)
        {
            if (target == null)
                return;

            try
            {
                target.Dispose();
            }
            catch (Exception exception)
            {
                Log.Write(exception);
            }
        }

        /// <summary>
        /// Calls dispose method for all objects in the specified collection suppressing all exceptions.
        /// </summary>
        /// <param name="target">Collection of disposable objects.</param>
        public static void DisposeSafe(IEnumerable<IDisposable> target)
        {
            if (target == null)
                return;

            try
            {
                target.ForEach(DisposeSafe);
            }
            catch (Exception exception)
            {
                Log.Write(exception);
            }
        }
    }
}
