using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.SystemTools.Win32.UnmanagedTypes
{
    internal sealed class UnmanagedShort : UnmanagedMemory
    {
        internal UnmanagedShort() :
            base(2)
        {
        }

        internal UnmanagedShort(IntPtr memory) :
            base(memory, 2)
        {
        }

        internal UnmanagedShort(short value) :
            base(2)
        {
            SValue = value;
        }

        internal UnmanagedShort(ushort value) :
            base(2)
        {
            UValue = value;
        }

        internal short SValue
        {
            get
            {
                lock (Synchronization)
                {
                    return (Address != IntPtr.Zero) ? Marshal.ReadInt16(Address) : (short)0;
                }
            }
            set
            {
                lock (Synchronization)
                {
                    if (Address != IntPtr.Zero)
                        Marshal.WriteInt16(Address, value);
                }
            }
        }

        internal ushort UValue
        {
            get
            {
                return unchecked((ushort)SValue);
            }
            set
            {
                SValue = unchecked((short)value);
            }
        }

        public static implicit operator short(UnmanagedShort value)
        {
            return (value != null) ? value.SValue : (short)0;
        }

        public static implicit operator ushort(UnmanagedShort value)
        {
            return (value != null) ? value.UValue : (ushort)0;
        }
    }
}
