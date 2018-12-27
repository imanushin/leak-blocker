using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.SystemTools.Win32.UnmanagedTypes;

namespace LeakBlocker.Libraries.SystemTools.Win32
{
    internal static class NativeErrors
    {
        private const string formatTemplate = "{0} failed ({1:X}): {2} (Additional data: {3})";

        internal static string GetMessage(string method, uint error, params object[] additionalData)
        {
            return GetSystemText(method, error, additionalData);
        }

        internal static string GetLastErrorMessage(string method, params object[] additionalData)
        {
            return GetMessage(method, NativeMethods.GetLastError(), additionalData);
        }

        internal static void ThrowException(string method, uint error, params object[] additionalData)
        {
            throw new InvalidOperationException(GetSystemText(method, error, additionalData), new Win32Exception(unchecked((int)error)));
        }

        internal static void ThrowLastErrorException(string method, params object[] additionalData)
        {
            ThrowException(method, NativeMethods.GetLastError(), additionalData);
        }

        private static string GetSystemText(string method, uint error, object[] additionalData)
        {
            string data = (additionalData == null) ? null : string.Join(" ", additionalData);

            string description = null;

            const uint formatFlags = (NativeMethods.FORMAT_MESSAGE_FROM_SYSTEM |
                NativeMethods.FORMAT_MESSAGE_IGNORE_INSERTS | NativeMethods.FORMAT_MESSAGE_ALLOCATE_BUFFER);

            using (var bufferPointer = new UnmanagedPointer())
            {
                try
                {
                    uint length = NativeMethods.FormatMessage(formatFlags, IntPtr.Zero, error, 0, +bufferPointer, 0, IntPtr.Zero);
                    if (length != 0)
                    {
                        using (var buffer = new UnmanagedUnicodeString(bufferPointer, length))
                        {
                            description = buffer.Value;
                            if (description != null)
                                description = description.TrimEnd(new[] { '\r', '\n' });
                        }
                    }
                }
                catch
                {
                }
                if (bufferPointer != IntPtr.Zero)
                    NativeMethods.LocalFree(bufferPointer);
            }

            return string.Format(CultureInfo.InvariantCulture, formatTemplate, method, error, description ?? string.Empty, data ?? string.Empty);
        }
    }
}

//ERROR_EXPECTED_SECTION_NAME = 0;
//ERROR_BAD_SECTION_NAME_LINE = 1;
//ERROR_SECTION_NAME_TOO_LONG = 2;
//ERROR_GENERAL_SYNTAX = 3;
//ERROR_WRONG_INF_STYLE = 0xE0000100;
//ERROR_SECTION_NOT_FOUND = 0xE0000101;
//ERROR_LINE_NOT_FOUND = 0xE0000102;
//ERROR_NO_BACKUP = 0xE0000103;
//ERROR_NO_ASSOCIATED_CLASS = 0xE0000200;
//ERROR_CLASS_MISMATCH = 0xE0000201;
//ERROR_DUPLICATE_FOUND = 0xE0000202;
//ERROR_NO_DRIVER_SELECTED = 0xE0000203;
//ERROR_KEY_DOES_NOT_EXIST = 0xE0000204;
//ERROR_INVALID_DEVINST_NAME = 0xE0000205;
//ERROR_INVALID_CLASS = 0xE0000206;
//ERROR_DEVINST_ALREADY_EXISTS = 0xE0000207;
//ERROR_DEVINFO_NOT_REGISTERED = 0xE0000208;
//ERROR_INVALID_REG_PROPERTY = 0xE0000209;
//ERROR_NO_INF = 0xE000020A;
//ERROR_NO_SUCH_DEVINST = 0xE000020B;
//ERROR_CANT_LOAD_CLASS_ICON = 0xE000020C;
//ERROR_INVALID_CLASS_INSTALLER = 0xE000020D;
//ERROR_DI_DO_DEFAULT = 0xE000020E;
//ERROR_DI_NOFILECOPY = 0xE000020F;
//ERROR_INVALID_HWPROFILE = 0xE0000210;
//ERROR_NO_DEVICE_SELECTED = 0xE0000211;
//ERROR_DEVINFO_LIST_LOCKED = 0xE0000212;
//ERROR_DEVINFO_DATA_LOCKED = 0xE0000213;
//ERROR_DI_BAD_PATH = 0xE0000214;
//ERROR_NO_CLASSINSTALL_PARAMS = 0xE0000215;
//ERROR_FILEQUEUE_LOCKED = 0xE0000216;
//ERROR_BAD_SERVICE_INSTALLSECT = 0xE0000217;
//ERROR_NO_CLASS_DRIVER_LIST = 0xE0000218;
//ERROR_NO_ASSOCIATED_SERVICE = 0xE0000219;
//ERROR_NO_DEFAULT_DEVICE_INTERFACE = 0xE000021A;
//ERROR_DEVICE_INTERFACE_ACTIVE = 0xE000021B;
//ERROR_DEVICE_INTERFACE_REMOVED = 0xE000021C;
//ERROR_BAD_INTERFACE_INSTALLSECT = 0xE000021D;
//ERROR_NO_SUCH_INTERFACE_CLASS = 0xE000021E;
//ERROR_INVALID_REFERENCE_STRING = 0xE000021F;
//ERROR_INVALID_MACHINENAME = 0xE0000220;
//ERROR_REMOTE_COMM_FAILURE = 0xE0000221;
//ERROR_MACHINE_UNAVAILABLE = 0xE0000222;
//ERROR_NO_CONFIGMGR_SERVICES = 0xE0000223;
//ERROR_INVALID_PROPPAGE_PROVIDER = 0xE0000224;
//ERROR_NO_SUCH_DEVICE_INTERFACE = 0xE0000225;
//ERROR_DI_POSTPROCESSING_REQUIRED = 0xE0000226;
//ERROR_INVALID_COINSTALLER = 0xE0000227;
//ERROR_NO_COMPAT_DRIVERS = 0xE0000228;
//ERROR_NO_DEVICE_ICON = 0xE0000229;
//ERROR_INVALID_INF_LOGCONFIG = 0xE000022A;
//ERROR_DI_DONT_INSTALL = 0xE000022B;
//ERROR_INVALID_FILTER_DRIVER = 0xE000022C;
//ERROR_NON_WINDOWS_NT_DRIVER = 0xE000022D;
//ERROR_NON_WINDOWS_DRIVER = 0xE000022E;
//ERROR_NO_CATALOG_FOR_OEM_INF = 0xE000022F;
//ERROR_DEVINSTALL_QUEUE_NONNATIVE = 0xE0000230;
//ERROR_NOT_DISABLEABLE = 0xE0000231;
//ERROR_CANT_REMOVE_DEVINST = 0xE0000232;
//ERROR_INVALID_TARGET = 0xE0000233;
//ERROR_DRIVER_NONNATIVE = 0xE0000234;
//ERROR_IN_WOW64 = 0xE0000235;
//ERROR_SET_SYSTEM_RESTORE_POINT = 0xE0000236;
//ERROR_SCE_DISABLED = 0xE0000238;
//ERROR_UNKNOWN_EXCEPTION = 0xE0000239;
//ERROR_PNP_REGISTRY_ERROR = 0xE000023A;
//ERROR_REMOTE_REQUEST_UNSUPPORTED = 0xE000023B;
//ERROR_NOT_AN_INSTALLED_OEM_INF = 0xE000023C;
//ERROR_INF_IN_USE_BY_DEVICES = 0xE000023D;
//ERROR_DI_FUNCTION_OBSOLETE = 0xE000023E;
//ERROR_NO_AUTHENTICODE_CATALOG = 0xE000023F;
//ERROR_AUTHENTICODE_DISALLOWED = 0xE0000240;
//ERROR_AUTHENTICODE_TRUSTED_PUBLISHER = 0xE0000241;
//ERROR_AUTHENTICODE_TRUST_NOT_ESTABLISHED = 0xE0000242;
//ERROR_AUTHENTICODE_PUBLISHER_NOT_TRUSTED = 0xE0000243;
//ERROR_SIGNATURE_OSATTRIBUTE_MISMATCH = 0xE0000244;
//ERROR_ONLY_VALIDATE_VIA_AUTHENTICODE = 0xE0000245;
//ERROR_DEVICE_INSTALLER_NOT_READY = 0xE0000246;
//ERROR_DRIVER_STORE_ADD_FAILED = 0xE0000247;
//ERROR_DEVICE_INSTALL_BLOCKED = 0xE0000248;
//ERROR_DRIVER_INSTALL_BLOCKED = 0xE0000249;
//ERROR_WRONG_INF_TYPE = 0xE000024A;
//ERROR_FILE_HASH_NOT_IN_CATALOG = 0xE000024B;
//ERROR_DRIVER_STORE_DELETE_FAILED = 0xE000024C;
//ERROR_UNRECOVERABLE_STACK_OVERFLOW = 0xE0000300;
//ERROR_NOT_INSTALLED = 0xE0001000;