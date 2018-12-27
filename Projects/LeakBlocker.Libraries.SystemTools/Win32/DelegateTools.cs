using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.SystemTools.Win32
{
    [AttributeUsage(AttributeTargets.Delegate | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    internal sealed class NativeCallbackAttribute : Attribute
    {
    }

    internal static class DelegateTools
    {
        internal static IntPtr GetPointer(Delegate method)
        {
            Check.ObjectIsNotNull(method, "method");

            if (method.Method.GetCustomAttributes(typeof(NativeCallbackAttribute), true).Length == 0)
                throw new InvalidOperationException("Method is not marked as native callback.");

            return Marshal.GetFunctionPointerForDelegate(method);
        }
    }
}
