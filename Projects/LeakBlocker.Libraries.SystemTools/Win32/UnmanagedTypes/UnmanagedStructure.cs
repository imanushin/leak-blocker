using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.SystemTools.Win32.UnmanagedTypes
{
    internal sealed class UnmanagedStructure<T> : UnmanagedMemory
    {
        internal UnmanagedStructure() :
            base(GetSize())
        {
        }

        internal UnmanagedStructure(IntPtr memory) :
            base(memory, GetSize())
        {
        }

        internal UnmanagedStructure(T value) :
            base(GetSize())
        {
            Value = value;
        }

        internal static uint GetSize()
        {
            return (uint)Marshal.SizeOf(typeof(T));
        }

        internal static int GetFieldOffset(string field)
        {
            return Marshal.OffsetOf(typeof(T), field).ToInt32();
        }

        internal IntPtr GetFieldAddress(string field)
        {
            return Address + Marshal.OffsetOf(typeof(T), field).ToInt32();
        }

        internal T Value
        {
            get
            {
                lock (Synchronization)
                {
                    if (Address != IntPtr.Zero)
                        return (T)Marshal.PtrToStructure(Address, typeof(T));
                    return default(T);
                }
            }
            set
            {
                lock (Synchronization)
                {
                    if (Address != IntPtr.Zero)
                        Marshal.StructureToPtr(value, Address, false);
                }
            }
        }

        public static implicit operator T(UnmanagedStructure<T> value)
        {
            return (value != null) ? value.Value : default(T);
        }
    }
}
