using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.SystemTools.Win32.UnmanagedTypes
{
    internal sealed class UnmanagedInteger : UnmanagedMemory
    {
        internal UnmanagedInteger() :
            base(4)
        {
        }

        internal UnmanagedInteger(IntPtr memory) :
            base(memory, 4)
        {
        }

        internal UnmanagedInteger(int value) :
            base(4)
        {
            SValue = value;
        }

        internal UnmanagedInteger(uint value) :
            base(4)
        {
            UValue = value;
        }

        internal int SValue
        {
            get
            {
                lock (Synchronization)
                {
                    return (Address != IntPtr.Zero) ? Marshal.ReadInt32(Address) : 0;
                }
            }
            set
            {
                lock (Synchronization)
                {
                    if (Address != IntPtr.Zero)
                        Marshal.WriteInt32(Address, value);
                }
            }
        }

        internal uint UValue
        {
            get
            {
                return unchecked((uint)SValue);
            }
            set
            {
                SValue = unchecked((int)value);
            }
        }

        public static implicit operator int(UnmanagedInteger value)
        {
            return (value != null) ? value.SValue : 0;
        }

        public static implicit operator uint(UnmanagedInteger value)
        {
            return (value != null) ? value.UValue : 0;
        }
    }
}
