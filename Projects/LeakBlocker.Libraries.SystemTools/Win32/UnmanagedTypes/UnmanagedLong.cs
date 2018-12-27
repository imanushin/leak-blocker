using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.SystemTools.Win32.UnmanagedTypes
{
    internal sealed class UnmanagedLong : UnmanagedMemory
    {
        internal UnmanagedLong() :
            base(8)
        {
        }

        internal UnmanagedLong(IntPtr memory) :
            base(memory, 8)
        {
        }

        internal UnmanagedLong(long value) :
            base(8)
        {
            SValue = value;
        }

        internal UnmanagedLong(ulong value) :
            base(8)
        {
            UValue = value;
        }

        internal long SValue
        {
            get
            {
                lock (Synchronization)
                {
                    return (Address != IntPtr.Zero) ? Marshal.ReadInt64(Address) : 0;
                }
            }
            set
            {
                lock (Synchronization)
                {
                    if (Address != IntPtr.Zero)
                        Marshal.WriteInt64(Address, value);
                }
            }
        }

        internal ulong UValue
        {
            get
            {
                return unchecked((ulong)SValue);
            }
            set
            {
                SValue = unchecked((long)value);
            }
        }

        public static implicit operator long(UnmanagedLong value)
        {
            return (value != null) ? value.SValue : 0;
        }

        public static implicit operator ulong(UnmanagedLong value)
        {
            return (value != null) ? value.UValue : 0;
        }
    }
}
