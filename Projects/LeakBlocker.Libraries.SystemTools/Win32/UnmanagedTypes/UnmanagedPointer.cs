using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.SystemTools.Win32.UnmanagedTypes
{
    internal sealed class UnmanagedPointer : UnmanagedMemory
    {
        internal UnmanagedPointer() :
            base(Marshal.SizeOf(typeof(IntPtr)))
        {
        }

        internal UnmanagedPointer(IntPtr memory) :
            base(memory, Marshal.SizeOf(typeof(IntPtr)))
        {
        }

        internal UnmanagedPointer(long value) :
            base(Marshal.SizeOf(typeof(IntPtr)))
        {
            Value = new IntPtr(value);
        }

        internal IntPtr Value
        {
            get
            {
                lock (Synchronization)
                {
                    return (Address != IntPtr.Zero) ? Marshal.ReadIntPtr(Address) : IntPtr.Zero;
                }
            }
            set
            {
                lock (Synchronization)
                {
                    if (Address != IntPtr.Zero)
                        Marshal.WriteIntPtr(Address, value);
                }
            }
        }

        public static implicit operator IntPtr(UnmanagedPointer value)
        {
            return (value != null) ? value.Value : IntPtr.Zero;
        }
    }
}
