using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.SystemTools.Win32
{
    internal static class StringTools
    {
        internal static string FromPointerAnsi(IntPtr pointer)
        {
            return Marshal.PtrToStringAnsi(pointer);
        }

        internal static string FromUnicodeString(NativeMethods.UNICODE_STRING data, bool trimTerminatingNulls = false)
        {
            if (data.Buffer == IntPtr.Zero)
                return null;

            if (data.Length == 0)
                return string.Empty;

            string result = Marshal.PtrToStringUni(data.Buffer, data.Length * 2) ?? string.Empty;

            return trimTerminatingNulls ? result.TrimEnd('\0') : result;
        }

        internal static string FromPointer(IntPtr pointer, int length)
        {
            return (pointer == IntPtr.Zero) ? null : Marshal.PtrToStringUni(pointer, length);
        }

        internal static string FromPointer(IntPtr pointer, bool doubleNullTerminated = false)
        {
            if (pointer == IntPtr.Zero)
                return null;

            if (!doubleNullTerminated)
                return Marshal.PtrToStringUni(pointer) ?? string.Empty;
            
            int nullCharactersCounter = 0;
            int length = 0;
            while (true)
            {
                short value = Marshal.ReadInt16(pointer, length * 2);
                if (value == 0)
                    nullCharactersCounter++;
                length++;

                if (nullCharactersCounter == 2)
                    break;
            }

            return Marshal.PtrToStringUni(pointer, length) ?? string.Empty;
        }

        internal static string ToDoubleNullTerminated(this IEnumerable<string> target)
        {
            Check.ObjectIsNotNull(target, "target");

            return string.Join("\0", target) + "\0\0";
        }

        internal static ReadOnlyCollection<string> FromDoubleNullTerminated(this string target)
        {
            Check.ObjectIsNotNull(target, "target");

            return new ReadOnlyCollection<string>(target.TrimEnd('\0').Split('\0'));
        }

        internal static ReadOnlyCollection<string> FromPointerDoubleNullTerminated(IntPtr pointer)
        {
            string data = FromPointer(pointer, true);
 
            return (data == null) ? new ReadOnlyCollection<string>(new string[0]) : FromDoubleNullTerminated(data);
        }
    }
}
