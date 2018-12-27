using System;
using System.Collections.Generic;
using LeakBlocker.Libraries.Common;
using Win32;

namespace LeakBlocker.Libraries.SystemTools.Implementations
{
    internal sealed class RegistryKeyMonitor : Disposable, IRegistryKeyMonitor
    {
        private readonly IntPtr targetKey;
        private readonly IntPtr notificationEvent;
        private readonly uint notificationTypes;
        private readonly IWaitHandle waitHandle;

        public event Action Changed;

        public RegistryKeyMonitor(string key, RegistryChangeTypes notifications, bool view64 = true)
        {
            Check.StringIsMeaningful(key, "key");
            Check.EnumerationValueIsDefined(notifications, "notifications");

            using (UnmanagedPointer keyBuffer = new UnmanagedPointer())
            {
                KeyValuePair<IntPtr, string> registryKey = SplitKeyPath(key);

                notificationTypes = (uint)notifications;

                uint accessRights = NativeConstants.KEY_NOTIFY | (view64 ? NativeConstants.KEY_WOW64_64KEY : NativeConstants.KEY_WOW64_32KEY);

                notificationEvent = NativeMethods.CreateEvent(IntPtr.Zero, false, false, null);
                if (notificationEvent == IntPtr.Zero)
                    NativeErrors.ThrowException("CreateEvent");

                int error = NativeMethods.RegOpenKeyEx(registryKey.Key, registryKey.Value, 0, accessRights, +keyBuffer);
                if (error != NativeConstants.ERROR_SUCCESS)
                    NativeErrors.ThrowException("RegOpenKeyEx", error);

                targetKey = keyBuffer;

                waitHandle = SharedObjects.AsyncInvoker.Invoke(MonitorThread);
            }
        }

        protected override void DisposeManaged()
        {
            if (!NativeMethods.SetEvent(notificationEvent))
                Log.Write(NativeErrors.GetMessage("SetEvent"));

            if (!waitHandle.Wait(TimeSpan.FromSeconds(5)))
                Log.Write("Thread waiting failed.");
        }

        protected override void DisposeUnmanaged()
        {
            if (!NativeMethods.SetEvent(notificationEvent))
                Log.Write(NativeErrors.GetMessage("SetEvent"));

            int error = NativeMethods.RegCloseKey(targetKey);
            if (error != NativeConstants.ERROR_SUCCESS)
                Log.Write(NativeErrors.GetMessage("RegCloseKey"));

            if (!NativeMethods.CloseHandle(notificationEvent))
                Log.Write(NativeErrors.GetMessage("CloseHandle"));
        }

        private void MonitorThread()
        {
            while (true)
            {
                int error = NativeMethods.RegNotifyChangeKeyValue(targetKey, true, notificationTypes, notificationEvent, true);
                if (error != NativeConstants.ERROR_SUCCESS)
                    NativeErrors.ThrowException("RegNotifyChangeKeyValue", error);

                uint waitResult = NativeMethods.WaitForSingleObject(notificationEvent, NativeConstants.INFINITE);
                if (waitResult != NativeConstants.WAIT_OBJECT_0)
                {
                    Log.Write(NativeErrors.GetMessage("WaitForSingleObject"));
                    break;
                }

                if (Disposed)
                    break;

                SharedObjects.AsyncInvoker.Invoke(() =>
                {
                    if (Changed != null)
                        Changed();
                });
            }
        }

        private static KeyValuePair<IntPtr, string> SplitKeyPath(string key)
        {
            Check.StringIsMeaningful(key, "key");

            IntPtr hive = IntPtr.Zero;
            string subKey = null;

            if (key.StartsWith("HKEY_CLASSES_ROOT", StringComparison.OrdinalIgnoreCase))
            {
                hive = NativeConstants.HKEY_CLASSES_ROOT;
                subKey = key.Substring("HKEY_CLASSES_ROOT".Length + 1);
            }
            if (key.StartsWith("HKEY_CURRENT_USER", StringComparison.OrdinalIgnoreCase))
            {
                hive = NativeConstants.HKEY_CURRENT_USER;
                subKey = key.Substring("HKEY_CURRENT_USER".Length + 1);
            }
            if (key.StartsWith("HKEY_CURRENT_CONFIG", StringComparison.OrdinalIgnoreCase))
            {
                hive = NativeConstants.HKEY_CURRENT_CONFIG;
                subKey = key.Substring("HKEY_CURRENT_CONFIG".Length + 1);
            }
            if (key.StartsWith("HKEY_CURRENT_USER_LOCAL_SETTINGS", StringComparison.OrdinalIgnoreCase))
            {
                hive = NativeConstants.HKEY_CURRENT_USER_LOCAL_SETTINGS;
                subKey = key.Substring("HKEY_CURRENT_USER_LOCAL_SETTINGS".Length + 1);
            }
            if (key.StartsWith("HKEY_LOCAL_MACHINE", StringComparison.OrdinalIgnoreCase))
            {
                hive = NativeConstants.HKEY_LOCAL_MACHINE;
                subKey = key.Substring("HKEY_LOCAL_MACHINE".Length + 1);
            }
            if (key.StartsWith("HKEY_USERS", StringComparison.OrdinalIgnoreCase))
            {
                hive = NativeConstants.HKEY_USERS;
                subKey = key.Substring("HKEY_USERS".Length + 1);
            }
            if (key.StartsWith("HKEY_PERFORMANCE_DATA", StringComparison.OrdinalIgnoreCase))
            {
                hive = NativeConstants.HKEY_PERFORMANCE_DATA;
                subKey = key.Substring("HKEY_PERFORMANCE_DATA".Length + 1);
            }
            if (key.StartsWith("HKEY_PERFORMANCE_TEXT", StringComparison.OrdinalIgnoreCase))
            {
                hive = NativeConstants.HKEY_PERFORMANCE_TEXT;
                subKey = key.Substring("HKEY_PERFORMANCE_TEXT".Length + 1);
            }
            if (key.StartsWith("HKEY_PERFORMANCE_NLSTEXT", StringComparison.OrdinalIgnoreCase))
            {
                hive = NativeConstants.HKEY_PERFORMANCE_NLSTEXT;
                subKey = key.Substring("HKEY_PERFORMANCE_NLSTEXT".Length + 1);
            }

            Check.StringIsMeaningful(subKey, "subKey");

            return new KeyValuePair<IntPtr, string>(hive, subKey);
        }
    }
}
