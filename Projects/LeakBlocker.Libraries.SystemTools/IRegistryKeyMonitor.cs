using System;
using Win32;

namespace LeakBlocker.Libraries.SystemTools
{
    [Flags()]
    internal enum RegistryChangeTypes
    {
        None = 0,
        Subkey = (int)NativeConstants.REG_NOTIFY_CHANGE_NAME,
        Value = (int)NativeConstants.REG_NOTIFY_CHANGE_LAST_SET,
        Attributes = (int)NativeConstants.REG_NOTIFY_CHANGE_ATTRIBUTES,
        Security = (int)NativeConstants.REG_NOTIFY_CHANGE_SECURITY,
        All = Subkey | Value | Attributes | Security
    }

    internal interface IRegistryKeyMonitor : IDisposable
    {
        event Action Changed;
    }
}
