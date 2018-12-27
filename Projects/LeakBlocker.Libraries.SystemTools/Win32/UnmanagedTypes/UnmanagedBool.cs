using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.SystemTools.Win32.UnmanagedTypes
{
    internal sealed class UnmanagedBool : UnmanagedMemory
    {
        internal UnmanagedBool() :
            base(4)
        {
        }

        internal UnmanagedBool(IntPtr memory) :
            base(memory, 4)
        {
        }

        internal UnmanagedBool(bool value) :
            base(4)
        {
            Value = value;
        }

        internal bool Value
        {
            get
            {
                lock (Synchronization)
                {
                    if (Address != IntPtr.Zero)
                        return (Marshal.ReadInt32(Address) != 0);
                    return false;
                }
            }
            set
            {
                lock (Synchronization)
                {
                    if (Address != IntPtr.Zero)
                        Marshal.WriteInt32(Address, value ? 1 : 0);
                }
            }
        }

        public static implicit operator bool(UnmanagedBool value)
        {
            return (value != null) && value.Value;
        }
    }
}
