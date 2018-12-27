using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.SystemTools.Win32.UnmanagedTypes
{
    internal sealed class UnmanagedUnicodeString : UnmanagedMemory
    {
        internal UnmanagedUnicodeString(int length) :
            base(LengthToSize(length))
        {
            SLength = Math.Max(0, length);
        }

        internal UnmanagedUnicodeString(IntPtr memory, int length) :
            base(memory, LengthToSize(length))
        {
            SLength = Math.Max(0, length);
        }

        internal UnmanagedUnicodeString(uint length) :
            base(LengthToSize(unchecked((int)length)))
        {
            SLength = Math.Max(0, unchecked((int)length));
        }

        internal UnmanagedUnicodeString(IntPtr memory, uint length) :
            base(memory, LengthToSize(unchecked((int)length)))
        {
            SLength = Math.Max(0, unchecked((int)length));
        }

        internal UnmanagedUnicodeString(string text) :
            base(LengthToSize(text))
        {
            SLength = (text != null) ? text.Length : 0;
            Value = text;
        }

        private static int LengthToSize(int length)
        {
            return unchecked(length * 2 + 2);
        }

        private static int LengthToSize(string text)
        {
            if (text == null)
                return 0;
            return unchecked(text.Length * 2 + 2);
        }

        internal int SLength
        {
            get;
            private set;
        }

        internal uint ULength
        {
            get
            {
                return unchecked((uint)SLength);
            }
        }

        internal string Value
        {
            get
            {
                return FullValue.TrimEnd('\0');
            }
            set
            {
                FullValue = value;
            }
        }

        internal string FullValue
        {
            get
            {
                lock (Synchronization)
                {
                    byte[] data = BinaryData;
                    if ((data.Length > 0) && (SLength * 2 <= data.Length))
                        return Encoding.Unicode.GetString(data, 0, SLength * 2);
                    return string.Empty;
                }
            }
            set
            {
                lock (Synchronization)
                {
                    ClearUnsafe();
                    if ((value == null) || (Address == IntPtr.Zero)) 
                        return;

                    byte[] data = Encoding.Unicode.GetBytes(value);
                    Marshal.Copy(data, 0, Address, Math.Min(data.Length, Math.Max(0, Math.Min(value.Length * 2, SSize - 2))));
                }
            }
        }

        public static implicit operator string(UnmanagedUnicodeString value)
        {
            return (value != null) ? value.Value : string.Empty;
        }
    }
}
