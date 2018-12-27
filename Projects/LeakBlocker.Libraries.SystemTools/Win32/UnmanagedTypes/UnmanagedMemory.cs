using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.SystemTools.Win32.UnmanagedTypes
{
    internal class UnmanagedMemory : IDisposable
    {
        private readonly object synchronization = new object();

        private bool allocated;
        private bool disposed;

        protected object Synchronization
        {
            get
            {
                return synchronization;
            }
        }

        internal UnmanagedMemory(IntPtr memory, uint size)
        {
            Address = memory;
            SSize = Math.Max(0, unchecked((int)size));
        }

        internal UnmanagedMemory(IntPtr memory, int size)
        {
            Address = memory;
            SSize = Math.Max(0, size);
        }

        internal UnmanagedMemory(uint allocationSize)
        {
            Allocate(unchecked((int)allocationSize));
        }

        internal UnmanagedMemory(int allocationSize)
        {
            Allocate(allocationSize);
        }

        internal UnmanagedMemory(byte[] data)
        {
            int allocationSize = 0;
            if (data != null)
                allocationSize = data.Length;
            Allocate(allocationSize);
            BinaryData = data;
        }

        private void Allocate(int allocationSize)
        {
            SSize = Math.Max(0, allocationSize);
            Address = Marshal.AllocHGlobal(SSize);
            allocated = true;
            Clear();
        }

        public void Clear()
        {
            lock (Synchronization)
            {
                ClearUnsafe();
            }
        }

        protected void ClearUnsafe()
        {
            if (Address != IntPtr.Zero)
                NativeMethods.RtlZeroMemory(Address, new IntPtr(SSize));
        }

        internal IntPtr Address
        {
            get;
            private set;
        }

        internal int SSize
        {
            get;
            private set;
        }

        internal uint USize
        {
            get
            {
                return unchecked((uint)SSize);
            }
        }

        internal byte[] BinaryData
        {
            get
            {
                lock (Synchronization)
                {
                    var result = new byte[SSize];
                    if (Address != IntPtr.Zero)
                        Marshal.Copy(Address, result, 0, SSize);
                    return result;
                }
            }
            set
            {
                lock (Synchronization)
                {
                    ClearUnsafe();
                    if ((value != null) && (value.Length != 0) && (Address != IntPtr.Zero))
                        Marshal.Copy(value, 0, Address, Math.Min(SSize, value.Length));
                }
            }
        }

        protected void Dispose(bool disposing)
        {
            lock (Synchronization)
            {
                if (disposed)
                    return;

                if (disposing)
                {
                    // free managed
                }
                if (allocated && (Address != IntPtr.Zero))
                {
                    Marshal.FreeHGlobal(Address);
                    Address = IntPtr.Zero;
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~UnmanagedMemory()
        {
            if (synchronization != null)
                Dispose(false);
        }

        public static IntPtr operator +(UnmanagedMemory value)
        {
            return (value != null) ? value.Address : IntPtr.Zero;
        }
    }
}
