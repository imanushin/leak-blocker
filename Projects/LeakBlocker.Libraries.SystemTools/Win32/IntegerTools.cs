using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.SystemTools.Win32.UnmanagedTypes;

namespace LeakBlocker.Libraries.SystemTools.Win32
{
    internal static class IntegerTools
    {
        internal static bool HasAnyFlag(this uint value, uint flag)
        {
            return ((value & flag) != 0);
        }

        internal static bool HasAnyFlag(this int value, int flag)
        {
            return ((value & flag) != 0);
        }

        internal static bool HasAnyFlag(this UnmanagedInteger value, uint flag)
        {
            Check.ObjectIsNotNull(value, "value");

            return ((value & flag) != 0);
        }

        internal static bool HasAnyFlag(this UnmanagedInteger value, int flag)
        {
            Check.ObjectIsNotNull(value, "value");

            return ((value & flag) != 0);
        }
        
        internal static bool HasFlag(this uint value, uint flag)
        {
            return ((value & flag) == flag);
        }

        internal static bool HasFlag(this int value, int flag)
        {
            return ((value & flag) == flag);
        }

        internal static bool HasFlag(this UnmanagedInteger value, uint flag)
        {
            Check.ObjectIsNotNull(value, "value");

            return ((value & flag) == flag);
        }

        internal static bool HasFlag(this UnmanagedInteger value, int flag)
        {
            Check.ObjectIsNotNull(value, "value");

            return ((value & flag) == flag);
        }
    }
}
