using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using LeakBlocker.Libraries.Common;

// ReSharper disable InconsistentNaming
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable MemberCanBePrivate.Global

namespace LeakBlocker.Libraries.SystemTools.Win32
{
    internal static class NativeMethods
    {
        internal const uint S_OK = 0;
        internal const uint ERROR_SUCCESS = 0;
        internal const uint STATUS_SUCCESS = 0x00000000;
        internal const uint NO_ERROR = 0;
        internal const uint ERROR_NOT_CONNECTED = 2250;
        internal const uint ERROR_INSUFFICIENT_BUFFER = 122;
        internal const uint ERROR_NO_MORE_ITEMS = 259;
        internal const uint ERROR_MORE_DATA = 234;
        internal const uint ERROR_SERVICE_DOES_NOT_EXIST = 1060;
        internal const uint ERROR_SERVICE_ALREADY_RUNNING = 1056;
        internal const uint ERROR_ALREADY_EXISTS = 183;
        internal const uint ERROR_INVALID_PARAMETER = 87;
        internal const uint ERROR_IO_PENDING = 997;
        internal const uint ERROR_BAD_NETPATH = 53;
        internal const uint ERROR_BAD_NET_NAME = 67;
        internal const uint ERROR_FILE_NOT_FOUND = 2;
        internal const uint ERROR_INVALID_DOMAINNAME = 1212;
        internal const uint ERROR_NO_SUCH_DOMAIN = 1355;
        internal const uint ERROR_INVALID_DATA = 13;
        internal const uint ERROR_NO_SUCH_DEVINST = 0xE000020B;
        internal const uint STATUS_OBJECT_PATH_INVALID = 0xC0000039;
        internal const uint STATUS_INFO_LENGTH_MISMATCH = 0xC0000004;
        internal const uint ERROR_ACCESS_DENIED = 5;
        internal const uint ERROR_INVALID_HANDLE = 6;
        internal const uint ERROR_NOT_SUPPORTED = 50;
        internal const uint STATUS_NOT_SUPPORTED = 0xC00000BB;
        internal const uint ERROR_NO_MORE_FILES = 18;
        internal const uint ERROR_LOGON_FAILURE = 1326;
        internal const uint SEC_E_LOGON_DENIED = 0x8009030C;
        internal const uint ERROR_INVALID_CLASS = 0xE0000206;
        internal const uint ERROR_INVALID_FUNCTION = 1;
        internal const uint ERROR_HANDLE_EOF = 38;
        internal const uint ERROR_NO_NET_OR_BAD_PATH = 1203;
        internal const uint ERROR_INVALID_NETNAME = 1214;
        internal const uint ERROR_BAD_USERNAME = 2202;
        internal const uint ERROR_INVALID_PASSWORD = 86;
        internal const uint ERROR_NO_NETWORK = 1222;
        internal const uint ERROR_ALREADY_ASSIGNED = 85;
        internal const uint ERROR_BAD_DEV_TYPE = 66;
        internal const uint ERROR_BAD_DEVICE = 1200;
        internal const uint ERROR_BAD_PROFILE = 1206;
        internal const uint ERROR_BAD_PROVIDER = 1204;
        internal const uint ERROR_BUSY = 170;
        internal const uint ERROR_CANCELLED = 1223;
        internal const uint ERROR_CANNOT_OPEN_PROFILE = 1205;
        internal const uint ERROR_DEVICE_ALREADY_REMEMBERED = 1202;
        internal const uint ERROR_EXTENDED_ERROR = 1208;
        internal const uint ERROR_INVALID_ADDRESS = 487;
        internal const uint ERROR_DEV_NOT_EXIST = 55;
        internal const uint ERROR_NETNAME_DELETED = 64;
        internal const uint ERROR_CONNECTION_REFUSED = 1225;
        internal const uint ERROR_NETWORK_UNREACHABLE = 1231;
        internal const uint ERROR_HOST_UNREACHABLE = 1232;
        internal const uint ERROR_PROTOCOL_UNREACHABLE = 1233;
        internal const uint ERROR_PORT_UNREACHABLE = 1234;
        internal const uint ERROR_NOT_LOGGED_ON = 1245;
        internal const uint ERROR_HOST_DOWN = 1256;
        internal const uint ERROR_TRUST_FAILURE = 1790;
        internal const uint ERROR_NETLOGON_NOT_STARTED = 1792;
        internal const uint ERROR_CONNECTED_OTHER_PASSWORD = 2108;
        internal const uint ERROR_CONNECTED_OTHER_PASSWORD_DEFAULT = 2109;
        internal const uint RPC_S_INVALID_BINDING = 1702;

        internal const uint TOKEN_ASSIGN_PRIMARY = 0x0001;
        internal const uint TOKEN_DUPLICATE = 0x0002;
        internal const uint TOKEN_IMPERSONATE = 0x0004;
        internal const uint TOKEN_QUERY = 0x0008;
        internal const uint TOKEN_QUERY_SOURCE = 0x0010;
        internal const uint TOKEN_ADJUST_PRIVILEGES = 0x0020;
        internal const uint TOKEN_ADJUST_GROUPS = 0x0040;
        internal const uint TOKEN_ADJUST_DEFAULT = 0x0080;
        internal const uint TOKEN_ADJUST_SESSIONID = 0x0100;
        internal const uint TOKEN_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED | TOKEN_ASSIGN_PRIMARY | TOKEN_DUPLICATE | TOKEN_IMPERSONATE | TOKEN_QUERY | TOKEN_QUERY_SOURCE | TOKEN_ADJUST_PRIVILEGES | TOKEN_ADJUST_GROUPS | TOKEN_ADJUST_DEFAULT | TOKEN_ADJUST_SESSIONID);

        internal const uint WAIT_OBJECT_0 = 0x00000000;

        internal const uint SV_TYPE_WORKSTATION = 0x00000001;
        internal const uint SV_TYPE_SERVER = 0x00000002;
        internal const uint SV_TYPE_DOMAIN_CTRL = 0x00000008;
        internal const uint SV_TYPE_DOMAIN_BAKCTRL = 0x00000010;
        internal const uint SV_TYPE_DOMAIN_ENUM = 0x80000000;
        internal const uint SV_TYPE_ALL = 0xFFFFFFFF;

        internal const uint MAX_COMPUTERNAME_LENGTH = 15;

        internal const uint SPDRP_PHYSICAL_DEVICE_OBJECT_NAME = 0x0000000E;
        internal const uint SPDRP_ENUMERATOR_NAME = 0x00000016;
        internal const uint SPDRP_CLASS = 0x00000007;
        internal const uint SPDRP_MFG = 0x0000000B;
        internal const uint SPDRP_DEVICEDESC = 0x00000000;
        internal const uint SPDRP_FRIENDLYNAME = 0x0000000C;
        internal const uint SPDRP_CAPABILITIES = 0x0000000F;
        internal const uint SPDRP_HARDWAREID = 0x00000001;
        internal const uint SPDRP_SERVICE = 0x00000004;

        internal const uint CM_PROB_NOT_CONFIGURED = 0x00000001;
        internal const uint CM_PROB_DEVLOADER_FAILED = 0x00000002;
        internal const uint CM_PROB_OUT_OF_MEMORY = 0x00000003;
        internal const uint CM_PROB_ENTRY_IS_WRONG_TYPE = 0x00000004;
        internal const uint CM_PROB_LACKED_ARBITRATOR = 0x00000005;
        internal const uint CM_PROB_BOOT_CONFIG_CONFLICT = 0x00000006;
        internal const uint CM_PROB_FAILED_FILTER = 0x00000007;
        internal const uint CM_PROB_DEVLOADER_NOT_FOUND = 0x00000008;
        internal const uint CM_PROB_INVALID_DATA = 0x00000009;
        internal const uint CM_PROB_FAILED_START = 0x0000000A;
        internal const uint CM_PROB_LIAR = 0x0000000B;
        internal const uint CM_PROB_NORMAL_CONFLICT = 0x0000000C;
        internal const uint CM_PROB_NOT_VERIFIED = 0x0000000D;
        internal const uint CM_PROB_NEED_RESTART = 0x0000000E;
        internal const uint CM_PROB_REENUMERATION = 0x0000000F;
        internal const uint CM_PROB_PARTIAL_LOG_CONF = 0x00000010;
        internal const uint CM_PROB_UNKNOWN_RESOURCE = 0x00000011;
        internal const uint CM_PROB_REINSTALL = 0x00000012;
        internal const uint CM_PROB_REGISTRY = 0x00000013;
        internal const uint CM_PROB_WILL_BE_REMOVED = 0x00000015;
        internal const uint CM_PROB_DISABLED = 0x00000016;
        internal const uint CM_PROB_DEVLOADER_NOT_READY = 0x00000017;
        internal const uint CM_PROB_DEVICE_NOT_THERE = 0x00000018;
        internal const uint CM_PROB_MOVED = 0x00000019;
        internal const uint CM_PROB_TOO_EARLY = 0x0000001A;
        internal const uint CM_PROB_NO_VALID_LOG_CONF = 0x0000001B;
        internal const uint CM_PROB_FAILED_INSTALL = 0x0000001C;
        internal const uint CM_PROB_HARDWARE_DISABLED = 0x0000001D;
        internal const uint CM_PROB_CANT_SHARE_IRQ = 0x0000001E;
        internal const uint CM_PROB_FAILED_ADD = 0x0000001F;
        internal const uint CM_PROB_DISABLED_SERVICE = 0x00000020;
        internal const uint CM_PROB_TRANSLATION_FAILED = 0x00000021;
        internal const uint CM_PROB_NO_SOFTCONFIG = 0x00000022;
        internal const uint CM_PROB_BIOS_TABLE = 0x00000023;
        internal const uint CM_PROB_IRQ_TRANSLATION_FAILED = 0x00000024;
        internal const uint CM_PROB_FAILED_DRIVER_ENTRY = 0x00000025;
        internal const uint CM_PROB_DRIVER_FAILED_PRIOR_UNLOAD = 0x00000026;
        internal const uint CM_PROB_DRIVER_FAILED_LOAD = 0x00000027;
        internal const uint CM_PROB_DRIVER_SERVICE_KEY_INVALID = 0x00000028;
        internal const uint CM_PROB_LEGACY_SERVICE_NO_DEVICES = 0x00000029;
        internal const uint CM_PROB_DUPLICATE_DEVICE = 0x0000002A;
        internal const uint CM_PROB_FAILED_POST_START = 0x0000002B;
        internal const uint CM_PROB_HALTED = 0x0000002C;
        internal const uint CM_PROB_PHANTOM = 0x0000002D;
        internal const uint CM_PROB_SYSTEM_SHUTDOWN = 0x0000002E;
        internal const uint CM_PROB_HELD_FOR_EJECT = 0x0000002F;
        internal const uint CM_PROB_DRIVER_BLOCKED = 0x00000030;
        internal const uint CM_PROB_REGISTRY_TOO_LARGE = 0x00000031;
        internal const uint CM_PROB_SETPROPERTIES_FAILED = 0x00000032;
        internal const uint CM_PROB_WAITING_ON_DEPENDENCY = 0x00000033;
        internal const uint CM_PROB_UNSIGNED_DRIVER = 0x00000034;

        internal const uint DN_ROOT_ENUMERATED = 0x00000001;
        internal const uint DN_DRIVER_LOADED = 0x00000002;
        internal const uint DN_ENUM_LOADED = 0x00000004;
        internal const uint DN_STARTED = 0x00000008;
        internal const uint DN_MANUAL = 0x00000010;
        internal const uint DN_NEED_TO_ENUM = 0x00000020;
        internal const uint DN_NOT_FIRST_TIME = 0x00000040;
        internal const uint DN_HARDWARE_ENUM = 0x00000080;
        internal const uint DN_LIAR = 0x00000100;
        internal const uint DN_HAS_MARK = 0x00000200;
        internal const uint DN_HAS_PROBLEM = 0x00000400;
        internal const uint DN_FILTERED = 0x00000800;
        internal const uint DN_MOVED = 0x00001000;
        internal const uint DN_DISABLEABLE = 0x00002000;
        internal const uint DN_REMOVABLE = 0x00004000;
        internal const uint DN_PRIVATE_PROBLEM = 0x00008000;
        internal const uint DN_MF_PARENT = 0x00010000;
        internal const uint DN_MF_CHILD = 0x00020000;
        internal const uint DN_WILL_BE_REMOVED = 0x00040000;
        internal const uint DN_NOT_FIRST_TIMEE = 0x00080000;
        internal const uint DN_STOP_FREE_RES = 0x00100000;
        internal const uint DN_REBAL_CANDIDATE = 0x00200000;
        internal const uint DN_BAD_PARTIAL = 0x00400000;
        internal const uint DN_NT_ENUMERATOR = 0x00800000;
        internal const uint DN_NT_DRIVER = 0x01000000;
        internal const uint DN_NEEDS_LOCKING = 0x02000000;
        internal const uint DN_ARM_WAKEUP = 0x04000000;
        internal const uint DN_APM_ENUMERATOR = 0x08000000;
        internal const uint DN_APM_DRIVER = 0x10000000;
        internal const uint DN_SILENT_INSTALL = 0x20000000;
        internal const uint DN_NO_SHOW_IN_DM = 0x40000000;
        internal const uint DN_BOOT_LOG_PROB = 0x80000000;
        internal const uint DN_NEED_RESTART = 0x00000100;

        internal const uint SERVICE_CONTROL_CONTINUE = 0x00000003;
        internal const uint SERVICE_CONTROL_INTERROGATE = 0x00000004;
        internal const uint SERVICE_CONTROL_NETBINDADD = 0x00000007;
        internal const uint SERVICE_CONTROL_NETBINDDISABLE = 0x0000000A;
        internal const uint SERVICE_CONTROL_NETBINDENABLE = 0x00000009;
        internal const uint SERVICE_CONTROL_NETBINDREMOVE = 0x00000008;
        internal const uint SERVICE_CONTROL_PARAMCHANGE = 0x00000006;
        internal const uint SERVICE_CONTROL_PAUSE = 0x00000002;
        internal const uint SERVICE_CONTROL_STOP = 0x00000001;
        internal const uint SERVICE_CONTROL_USERMODEREBOOT = 0x00000040;
        internal const uint SERVICE_CONTROL_SHUTDOWN = 0x00000005;
        internal const uint SERVICE_CONTROL_PRESHUTDOWN = 0x0000000F;
        internal const uint SERVICE_CONTROL_DEVICEEVENT = 0x0000000B;
        internal const uint SERVICE_CONTROL_HARDWAREPROFILECHANGE = 0x0000000C;
        internal const uint SERVICE_CONTROL_POWEREVENT = 0x0000000D;
        internal const uint SERVICE_CONTROL_SESSIONCHANGE = 0x0000000E;
        internal const uint SERVICE_CONTROL_TIMECHANGE = 0x00000010;
        internal const uint SERVICE_CONTROL_TRIGGEREVENT = 0x00000020;

        internal const uint SERVICE_STOPPED = 0x00000001;
        internal const uint SERVICE_START_PENDING = 0x00000002;
        internal const uint SERVICE_STOP_PENDING = 0x00000003;
        internal const uint SERVICE_RUNNING = 0x00000004;
        internal const uint SERVICE_CONTINUE_PENDING = 0x00000005;
        internal const uint SERVICE_PAUSE_PENDING = 0x00000006;
        internal const uint SERVICE_PAUSED = 0x00000007;

        internal const uint SERVICE_ACCEPT_STOP = 0x00000001;
        internal const uint SERVICE_ACCEPT_PAUSE_CONTINUE = 0x00000002;
        internal const uint SERVICE_ACCEPT_SHUTDOWN = 0x00000004;
        internal const uint SERVICE_ACCEPT_PARAMCHANGE = 0x00000008;
        internal const uint SERVICE_ACCEPT_NETBINDCHANGE = 0x00000010;
        internal const uint SERVICE_ACCEPT_HARDWAREPROFILECHANGE = 0x00000020;
        internal const uint SERVICE_ACCEPT_POWEREVENT = 0x00000040;
        internal const uint SERVICE_ACCEPT_SESSIONCHANGE = 0x00000080;

        internal const uint SERVICE_ADAPTER = 0x00000004;
        internal const uint SERVICE_FILE_SYSTEM_DRIVER = 0x00000002;
        internal const uint SERVICE_KERNEL_DRIVER = 0x00000001;
        internal const uint SERVICE_RECOGNIZER_DRIVER = 0x00000008;
        internal const uint SERVICE_WIN32_OWN_PROCESS = 0x00000010;
        internal const uint SERVICE_WIN32_SHARE_PROCESS = 0x00000020;

        internal const uint SERVICE_AUTO_START = 0x00000002;
        internal const uint SERVICE_DEMAND_START = 0x00000003;
        internal const uint SERVICE_DISABLED = 0x00000004;

        internal const uint SERVICE_ERROR_IGNORE = 0x00000000;
        internal const uint SERVICE_ERROR_NORMAL = 0x00000001;
        internal const uint SERVICE_ERROR_SEVERE = 0x00000002;
        internal const uint SERVICE_ERROR_CRITICAL = 0x00000003;

        internal const uint DRIVER_PACKAGE_SILENT = 0x00000002;
        internal const uint DRIVER_PACKAGE_FORCE = 0x00000004;
        internal const uint DRIVER_PACKAGE_LEGACY_MODE = 0x00000010;
        internal const uint DRIVER_PACKAGE_DELETE_FILES = 0x00000020;

        internal enum DIFXAPI_LOG : uint
        {
        }

        internal enum OBJECT_INFORMATION_CLASS : uint
        {
            ObjectNameInformation = 1,
        }

        public enum DS_NAME_ERROR
        {
            DS_NAME_NO_ERROR = 0,
        }

        internal const uint WTS_CONSOLE_CONNECT = 0x1;
        internal const uint WTS_CONSOLE_DISCONNECT = 0x2;
        internal const uint WTS_REMOTE_CONNECT = 0x3;
        internal const uint WTS_REMOTE_DISCONNECT = 0x4;
        internal const uint WTS_SESSION_LOGON = 0x5;
        internal const uint WTS_SESSION_LOGOFF = 0x6;
        internal const uint WTS_SESSION_LOCK = 0x7;
        internal const uint WTS_SESSION_UNLOCK = 0x8;
        internal const uint WTS_SESSION_REMOTE_CONTROL = 0x9;
        internal const uint WTS_SESSION_CREATE = 0xA;
        internal const uint WTS_SESSION_TERMINATE = 0xB;

        internal const uint PBT_APMPOWERSTATUSCHANGE = 0x000A;
        internal const uint PBT_APMRESUMEAUTOMATIC = 0x0012;
        internal const uint PBT_APMRESUMESUSPEND = 0x0007;
        internal const uint PBT_APMSUSPEND = 0x0004;
        internal const uint PBT_POWERSETTINGCHANGE = 0x8013;
        internal const uint PBT_APMBATTERYLOW = 0x0009;
        internal const uint PBT_APMOEMEVENT = 0x000B;
        internal const uint PBT_APMQUERYSUSPEND = 0x0000;
        internal const uint PBT_APMQUERYSUSPENDFAILED = 0x0002;
        internal const uint PBT_APMRESUMECRITICAL = 0x0006;

        internal const uint DBT_DEVICEARRIVAL = 0x8000;
        internal const uint DBT_DEVICEREMOVECOMPLETE = 0x8004;
        internal const uint DBT_DEVICEQUERYREMOVE = 0x8001;
        internal const uint DBT_DEVICEQUERYREMOVEFAILED = 0x8002;
        internal const uint DBT_DEVICEREMOVEPENDING = 0x8003;
        internal const uint DBT_CUSTOMEVENT = 0x8006;
        internal const uint DBT_QUERYCHANGECONFIG = 0x0017;
        internal const uint DBT_CONFIGCHANGED = 0x0018;
        internal const uint DBT_CONFIGCHANGECANCELED = 0x0019;

        internal const uint REG_BINARY = 3;
        internal const uint REG_DWORD = 4;
        internal const uint REG_EXPAND_SZ = 2;
        internal const uint REG_MULTI_SZ = 7;
        internal const uint REG_SZ = 1;

        internal const uint REG_NOTIFY_CHANGE_NAME = 0x00000001;
        internal const uint REG_NOTIFY_CHANGE_ATTRIBUTES = 0x00000002;
        internal const uint REG_NOTIFY_CHANGE_LAST_SET = 0x00000004;
        internal const uint REG_NOTIFY_CHANGE_SECURITY = 0x00000008;

        internal const uint CM_DEVCAP_LOCKSUPPORTED = 0x00000001;
        internal const uint CM_DEVCAP_EJECTSUPPORTED = 0x00000002;
        internal const uint CM_DEVCAP_REMOVABLE = 0x00000004;
        internal const uint CM_DEVCAP_DOCKDEVICE = 0x00000008;
        internal const uint CM_DEVCAP_UNIQUEID = 0x00000010;
        internal const uint CM_DEVCAP_SILENTINSTALL = 0x00000020;
        internal const uint CM_DEVCAP_RAWDEVICEOK = 0x00000040;
        internal const uint CM_DEVCAP_SURPRISEREMOVALOK = 0x00000080;
        internal const uint CM_DEVCAP_HARDWAREDISABLED = 0x00000100;
        internal const uint CM_DEVCAP_NONDYNAMIC = 0x00000200;

        internal const uint DICS_ENABLE = 0x00000001;
        internal const uint DICS_DISABLE = 0x00000002;

        internal const uint DICS_FLAG_GLOBAL = 0x00000001;

        internal const uint DIGCF_PRESENT = 0x00000002;
        internal const uint DIGCF_ALLCLASSES = 0x00000004;
        internal const uint DIGCF_DEVICEINTERFACE = 0x00000010;

        internal const uint DEVICE_NOTIFY_SERVICE_HANDLE = 0x00000001;
        internal const uint DEVICE_NOTIFY_ALL_INTERFACE_CLASSES = 0x00000004;

        internal const uint SC_MANAGER_CONNECT = 0x0001;
        internal const uint SC_MANAGER_CREATE_SERVICE = 0x0002;

        internal const uint SERVICE_QUERY_CONFIG = 0x0001;
        internal const uint SERVICE_CHANGE_CONFIG = 0x0002;
        internal const uint SERVICE_QUERY_STATUS = 0x0004;
        internal const uint SERVICE_START = 0x0010;
        internal const uint SERVICE_STOP = 0x0020;
        internal const uint SERVICE_PAUSE_CONTINUE = 0x0040;
        internal const uint SERVICE_INTERROGATE = 0x0080;
        internal const uint SERVICE_USER_DEFINED_CONTROL = 0x0100;

        internal const uint CR_SUCCESS = 0x00000000;
        internal const uint CR_DEFAULT = 0x00000001;
        internal const uint CR_OUT_OF_MEMORY = 0x00000002;
        internal const uint CR_INVALID_POINTER = 0x00000003;
        internal const uint CR_INVALID_FLAG = 0x00000004;
        internal const uint CR_INVALID_DEVNODE = 0x00000005;
        internal const uint CR_INVALID_RES_DES = 0x00000006;
        internal const uint CR_INVALID_LOG_CONF = 0x00000007;
        internal const uint CR_INVALID_ARBITRATOR = 0x00000008;
        internal const uint CR_INVALID_NODELIST = 0x00000009;
        internal const uint CR_DEVNODE_HAS_REQS = 0x0000000A;
        internal const uint CR_INVALID_RESOURCEID = 0x0000000B;
        internal const uint CR_DLVXD_NOT_FOUND = 0x0000000C;
        internal const uint CR_NO_SUCH_DEVNODE = 0x0000000D;
        internal const uint CR_NO_MORE_LOG_CONF = 0x0000000E;
        internal const uint CR_NO_MORE_RES_DES = 0x0000000F;
        internal const uint CR_ALREADY_SUCH_DEVNODE = 0x00000010;
        internal const uint CR_INVALID_RANGE_LIST = 0x00000011;
        internal const uint CR_INVALID_RANGE = 0x00000012;
        internal const uint CR_FAILURE = 0x00000013;
        internal const uint CR_NO_SUCH_LOGICAL_DEV = 0x00000014;
        internal const uint CR_CREATE_BLOCKED = 0x00000015;
        internal const uint CR_NOT_SYSTEM_VM = 0x00000016;
        internal const uint CR_REMOVE_VETOED = 0x00000017;
        internal const uint CR_APM_VETOED = 0x00000018;
        internal const uint CR_INVALID_LOAD_TYPE = 0x00000019;
        internal const uint CR_BUFFER_SMALL = 0x0000001A;
        internal const uint CR_NO_ARBITRATOR = 0x0000001B;
        internal const uint CR_NO_REGISTRY_HANDLE = 0x0000001C;
        internal const uint CR_REGISTRY_ERROR = 0x0000001D;
        internal const uint CR_INVALID_DEVICE_ID = 0x0000001E;
        internal const uint CR_INVALID_DATA = 0x0000001F;
        internal const uint CR_INVALID_API = 0x00000020;
        internal const uint CR_DEVLOADER_NOT_READY = 0x00000021;
        internal const uint CR_NEED_RESTART = 0x00000022;
        internal const uint CR_NO_MORE_HW_PROFILES = 0x00000023;
        internal const uint CR_DEVICE_NOT_THERE = 0x00000024;
        internal const uint CR_NO_SUCH_VALUE = 0x00000025;
        internal const uint CR_WRONG_TYPE = 0x00000026;
        internal const uint CR_INVALID_PRIORITY = 0x00000027;
        internal const uint CR_NOT_DISABLEABLE = 0x00000028;
        internal const uint CR_FREE_RESOURCES = 0x00000029;
        internal const uint CR_QUERY_VETOED = 0x0000002A;
        internal const uint CR_CANT_SHARE_IRQ = 0x0000002B;
        internal const uint CR_NO_DEPENDENT = 0x0000002C;
        internal const uint CR_SAME_RESOURCES = 0x0000002D;
        internal const uint CR_NO_SUCH_REGISTRY_KEY = 0x0000002E;
        internal const uint CR_INVALID_MACHINENAME = 0x0000002F;
        internal const uint CR_REMOTE_COMM_FAILURE = 0x00000030;
        internal const uint CR_MACHINE_UNAVAILABLE = 0x00000031;
        internal const uint CR_NO_CM_SERVICES = 0x00000032;
        internal const uint CR_ACCESS_DENIED = 0x00000033;
        internal const uint CR_CALL_NOT_IMPLEMENTED = 0x00000034;
        internal const uint CR_INVALID_PROPERTY = 0x00000035;
        internal const uint CR_DEVICE_INTERFACE_ACTIVE = 0x00000036;
        internal const uint CR_NO_SUCH_DEVICE_INTERFACE = 0x00000037;
        internal const uint CR_INVALID_REFERENCE_STRING = 0x00000038;
        internal const uint CR_INVALID_CONFLICT_LIST = 0x00000039;
        internal const uint CR_INVALID_INDEX = 0x0000003A;
        internal const uint CR_INVALID_STRUCTURE_SIZE = 0x0000003B;

        internal const uint TH32CS_SNAPPROCESS = 0x00000002;

        internal const uint DELETE = 0x00010000;
        internal const uint STANDARD_RIGHTS_REQUIRED = 0x000F0000;

        internal const uint DUPLICATE_CLOSE_SOURCE = 0x00000001;
        internal const uint DUPLICATE_SAME_ACCESS = 0x00000002;

        internal const uint INFINITE = 0xFFFFFFFF;

        internal enum DS_NAME_FLAGS : uint
        {
            DS_NAME_FLAG_SYNTACTICAL_ONLY = 0x1,
        }

        internal const uint SERVICE_NO_CHANGE = 0xffffffff;

        internal const uint SERVICE_CONFIG_DESCRIPTION = 1;
        internal const uint SERVICE_CONFIG_FAILURE_ACTIONS = 2;

        internal const uint DBT_DEVTYP_DEVICEINTERFACE = 0x00000005;

        internal const uint FACILITY_WIN32 = 7;

        internal const uint MAX_PATH = 260;

        internal const uint NERR_Success = 0;

        internal const uint CONNECT_UPDATE_PROFILE = 0x00000001;
        internal const uint CONNECT_TEMPORARY = 0x00000004;

        internal const uint RESOURCE_CONNECTED = 0x00000001;

        internal const uint RESOURCETYPE_ANY = 0x00000000;

        internal enum STORAGE_BUS_TYPE : uint
        {
        }

        internal const uint MAX_PREFERRED_LENGTH = 0xFFFFFFFF;

        internal const uint SPCRP_SECURITY_SDS = 0x00000018;
        internal const uint SPCRP_DEVTYPE = 0x00000019;

        internal const uint GENERIC_ALL = 0x10000000;

        internal const uint SE_PRIVILEGE_ENABLED = 0x00000002;

        internal const uint LOGON32_LOGON_INTERACTIVE = 2;

        internal const uint LOGON32_PROVIDER_DEFAULT = 0;

        internal const uint FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x00000100;
        internal const uint FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000;
        internal const uint FORMAT_MESSAGE_IGNORE_INSERTS = 0x00000200;

        internal const uint FILE_SHARE_DELETE = 0x00000004;
        internal const uint FILE_SHARE_READ = 0x00000001;
        internal const uint FILE_SHARE_WRITE = 0x00000002;

        internal const uint FILE_ATTRIBUTE_NORMAL = 0x00000080;
        internal const uint GENERIC_WRITE = 0x40000000;
        internal const uint OPEN_EXISTING = 3;
        internal const uint MAILSLOT_WAIT_FOREVER = 0xffffffff;

        internal static IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        internal static readonly IntPtr WTS_CURRENT_SERVER_HANDLE = IntPtr.Zero;

        internal enum DEVICE_TYPE : uint
        {
            FILE_DEVICE_MASS_STORAGE = 0x0000002d
        }

        internal const uint METHOD_BUFFERED = 0;
        internal const uint IOCTL_STORAGE_BASE = (uint)DEVICE_TYPE.FILE_DEVICE_MASS_STORAGE;
        internal const uint FILE_ANY_ACCESS = 0;
        internal const uint IOCTL_STORAGE_QUERY_PROPERTY = (IOCTL_STORAGE_BASE << 16) | (0x0500 << 2) | METHOD_BUFFERED | (FILE_ANY_ACCESS << 14);
        internal const uint IOCTL_STORAGE_GET_DEVICE_NUMBER = (IOCTL_STORAGE_BASE << 16) | (0x0420 << 2) | METHOD_BUFFERED | (FILE_ANY_ACCESS << 14);

        internal const uint DI_NEEDREBOOT = 0x00000100;

        internal enum WTS_INFO_CLASS
        {
            WTSUserName = 5,
            WTSDomainName = 7,
            WTSClientProtocolType = 16
        }

        internal enum NETSETUP_JOIN_STATUS : uint
        {
            NetSetupDomainName = 3
        }

        internal enum DSROLE_PRIMARY_DOMAIN_INFO_LEVEL : uint
        {
            DsRolePrimaryDomainInfoBasic = 1,
        }

        internal enum DSROLE_MACHINE_ROLE : uint
        {
        }

        internal enum TOKEN_INFORMATION_CLASS : uint
        {
            TokenUser = 1
        }

        internal enum SC_ACTION_TYPE : uint
        {
            SC_ACTION_NONE = 0,
            SC_ACTION_RESTART = 1,
            SC_ACTION_REBOOT = 2,
            SC_ACTION_RUN_COMMAND = 3
        }

        internal enum SYSTEM_INFORMATION_CLASS : uint
        {
            SystemExtendedHandleInformation = 64
        }

        internal static readonly IntPtr HKEY_CLASSES_ROOT = new IntPtr(unchecked((int)0x80000000));
        internal static readonly IntPtr HKEY_CURRENT_USER = new IntPtr(unchecked((int)0x80000001));
        internal static readonly IntPtr HKEY_CURRENT_CONFIG = new IntPtr(unchecked((int)0x80000005));
        internal static readonly IntPtr HKEY_CURRENT_USER_LOCAL_SETTINGS = new IntPtr(unchecked((int)0x80000007));
        internal static readonly IntPtr HKEY_LOCAL_MACHINE = new IntPtr(unchecked((int)0x80000002));
        internal static readonly IntPtr HKEY_USERS = new IntPtr(unchecked((int)0x80000003));
        internal static readonly IntPtr HKEY_PERFORMANCE_DATA = new IntPtr(unchecked((int)0x80000004));
        internal static readonly IntPtr HKEY_PERFORMANCE_TEXT = new IntPtr(unchecked((int)0x80000050));
        internal static readonly IntPtr HKEY_PERFORMANCE_NLSTEXT = new IntPtr(unchecked((int)0x80000060));

        internal const uint KEY_ENUMERATE_SUB_KEYS = 0x0008;
        internal const uint KEY_NOTIFY = 0x0010;
        internal const uint KEY_WOW64_32KEY = 0x0200;
        internal const uint KEY_WOW64_64KEY = 0x0100;

        internal const uint PROCESS_VM_READ = 0x0010;
        internal const uint PROCESS_DUP_HANDLE = 0x0040;
        internal const uint PROCESS_QUERY_INFORMATION = 0x0400;

        internal const uint POLICY_VIEW_LOCAL_INFORMATION = 1;

        internal enum DI_FUNCTION : uint
        {
            DIF_PROPERTYCHANGE = 0x00000012,
        }

        internal enum INSTANCE_INFORMATION_CLASS : uint
        {
            InstanceFullInformation = 2,
        }

        internal enum FILTER_VOLUME_INFORMATION_CLASS : uint
        {
            FilterVolumeBasicInformation = 0,
        }

        internal enum STORAGE_PROPERTY_ID : uint
        {
            StorageDeviceProperty = 0,
        }

        internal enum STORAGE_QUERY_TYPE : uint
        {
            PropertyStandardQuery = 0,
        }

        internal enum DS_NAME_FORMAT : uint
        {
            DS_FQDN_1779_NAME = 1,
            DS_CANONICAL_NAME = 7,
        }

        internal enum POLICY_INFORMATION_CLASS
        {
            PolicyAccountDomainInformation = 5,
            PolicyDnsDomainInformation = 12,
        }

        internal enum COMPUTER_NAME_FORMAT
        {
            ComputerNamePhysicalDnsFullyQualified = 7,
        }

        internal enum WTS_CONNECTSTATE_CLASS : uint
        {
            WTSActive,
            WTSConnected,
            WTSConnectQuery,
            WTSShadow,
            WTSDisconnected,
            WTSIdle,
            WTSListen,
            WTSReset,
            WTSDown,
            WTSInit
        }

        internal const uint IF_MAX_PHYS_ADDRESS_LENGTH = 32;

        internal enum NL_NEIGHBOR_STATE
        {
            NlnsUnreachable,
            NlnsIncomplete,
            NlnsProbe,
            NlnsDelay,
            NlnsStale,
            NlnsReachable,
            NlnsPermanent
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct LSA_OBJECT_ATTRIBUTES
        {
            internal uint Length;
            internal IntPtr RootDirectory;
            internal IntPtr ObjectName;
            internal uint Attributes;
            internal IntPtr SecurityDescriptor;
            internal IntPtr SecurityQualityOfService;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct NETRESOURCE
        {
            internal uint dwScope;
            internal uint dwType;
            internal uint dwDisplayType;
            internal uint dwUsage;
            internal IntPtr lpLocalName;
            internal IntPtr lpRemoteName;
            internal IntPtr lpComment;
            internal IntPtr lpProvider;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct SERVER_INFO_100
        {
            internal uint sv100_platform_id;
            internal IntPtr sv100_name;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct SERVER_INFO_101
        {
            internal uint sv101_platform_id;
            internal IntPtr sv101_name;
            internal uint sv101_version_major;
            internal uint sv101_version_minor;
            internal uint sv101_type;
            internal IntPtr sv101_comment;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct DSROLE_PRIMARY_DOMAIN_INFO_BASIC
        {
            [MarshalAs(UnmanagedType.U4)]
            internal DSROLE_MACHINE_ROLE MachineRole;
            internal uint Flags;
            internal IntPtr DomainNameFlat;
            internal IntPtr DomainNameDns;
            internal IntPtr DomainForestName;
            internal Guid DomainGuid;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct STORAGE_DEVICE_DESCRIPTOR
        {
            internal uint Version;
            internal uint Size;
            internal byte DeviceType;
            internal byte DeviceTypeModifier;
            [MarshalAs(UnmanagedType.U1)]
            internal bool RemovableMedia;
            [MarshalAs(UnmanagedType.U1)]
            internal bool CommandQueueing;
            internal uint VendorIdOffset;
            internal uint ProductIdOffset;
            internal uint ProductRevisionOffset;
            internal uint SerialNumberOffset;
            [MarshalAs(UnmanagedType.U4)]
            internal STORAGE_BUS_TYPE BusType;
            internal uint RawPropertiesLength;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            internal byte[] RawDeviceProperties;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct SYSTEM_EXTENDED_HANDLE_INFORMATION
        {
            internal IntPtr NumberOfHandles;
            internal IntPtr Reserved;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            internal SYSTEM_HANDLE_TABLE_ENTRY_INFO_EX[] Handles;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct WKSTA_INFO_100
        {
            internal uint wki100_platform_id;
            internal IntPtr wki100_computername;
            internal IntPtr wki100_langroup;
            internal uint wki100_ver_major;
            internal uint wki100_ver_minor;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct SERVICE_DESCRIPTION
        {
            internal IntPtr lpDescription;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct SC_ACTION
        {
            [MarshalAs(UnmanagedType.U4)]
            internal SC_ACTION_TYPE Type;
            internal uint Delay;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct SERVICE_FAILURE_ACTIONS
        {
            internal uint dwResetPeriod;
            internal IntPtr lpRebootMsg;
            internal IntPtr lpCommand;
            internal uint cActions;
            internal IntPtr lpsaActions;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct SP_DEVINSTALL_PARAMS
        {
            internal uint cbSize;
            internal uint Flags;
            internal uint FlagsEx;
            internal IntPtr hwndParent;
            internal IntPtr InstallMsgHandler;
            internal IntPtr InstallMsgHandlerContext;
            internal IntPtr FileQueue;
            internal IntPtr ClassInstallReserved;
            internal uint Reserved;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)MAX_PATH)]
            internal char[] DriverPath;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct SP_PROPCHANGE_PARAMS
        {
            internal SP_CLASSINSTALL_HEADER ClassInstallHeader;
            internal uint StateChange;
            internal uint Scope;
            internal uint HwProfile;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct SP_CLASSINSTALL_HEADER
        {
            internal uint cbSize;
            [MarshalAs(UnmanagedType.U4)]
            internal DI_FUNCTION InstallFunction;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct SERVICE_STATUS
        {
            internal uint dwServiceType;
            internal uint dwCurrentState;
            internal uint dwControlsAccepted;
            internal uint dwWin32ExitCode;
            internal uint dwServiceSpecificExitCode;
            internal uint dwCheckPoint;
            internal uint dwWaitHint;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct QUERY_SERVICE_CONFIG
        {
            internal uint dwServiceType;
            internal uint dwStartType;
            internal uint dwErrorControl;
            internal IntPtr lpBinaryPathName;
            internal IntPtr lpLoadOrderGroup;
            internal uint dwTagId;
            internal IntPtr lpDependencies;
            internal IntPtr lpServiceStartName;
            internal IntPtr lpDisplayName;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct LUID_AND_ATTRIBUTES
        {
            internal LUID Luid;
            internal uint Attributes;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct TOKEN_PRIVILEGES
        {
            internal uint PrivilegeCount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            internal LUID_AND_ATTRIBUTES[] Privileges;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct SERVICE_TABLE_ENTRY
        {
            internal IntPtr lpServiceName;
            internal IntPtr lpServiceProc;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct OVERLAPPED
        {
            internal IntPtr Internal;
            internal IntPtr InternalHigh;
            internal uint Offset;
            internal uint OffsetHigh;
            internal IntPtr hEvent;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct SP_DEVINFO_DATA
        {
            internal uint cbSize;
            internal Guid ClassGuid;
            internal uint DevInst;
            internal IntPtr Reserved;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct DEV_BROADCAST_DEVICEINTERFACE
        {
            internal uint dbcc_size;
            internal uint dbcc_devicetype;
            internal uint dbcc_reserved;
            internal Guid dbcc_classguid;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            internal char[] dbcc_name;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct SYSTEM_HANDLE_TABLE_ENTRY_INFO_EX
        {
            internal IntPtr Object;
            internal IntPtr UniqueProcessId;
            internal IntPtr HandleValue;
            internal uint GrantedAccess;
            internal ushort CreatorBackTraceIndex;
            internal ushort ObjectTypeIndex;
            internal uint HandleAttributes;
            internal uint Reserved;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct TOKEN_USER
        {
            internal SID_AND_ATTRIBUTES User;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct SID_AND_ATTRIBUTES
        {
            internal IntPtr Sid;
            internal uint Attributes;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct OBJECT_NAME_INFORMATION
        {
            internal UNICODE_STRING Name;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct STORAGE_DEVICE_NUMBER
        {
            [MarshalAs(UnmanagedType.U4)]
            internal DEVICE_TYPE DeviceType;
            internal uint DeviceNumber;
            internal uint PartitionNumber;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct IO_STATUS_BLOCK
        {
            internal uint Status;
            internal IntPtr Information;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct STORAGE_PROPERTY_QUERY
        {
            internal STORAGE_PROPERTY_ID PropertyId;
            internal STORAGE_QUERY_TYPE QueryType;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            internal byte[] AdditionalParameters;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct PROCESSENTRY32
        {
            internal uint dwSize;
            internal uint cntUsage;
            internal uint th32ProcessID;
            internal IntPtr th32DefaultHeapID;
            internal uint th32ModuleID;
            internal uint cntThreads;
            internal uint th32ParentProcessID;
            internal int pcPriClassBase;
            internal uint dwFlags;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = unchecked((int)MAX_PATH))]
            internal char[] szExeFile;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct FILTER_MESSAGE_HEADER
        {
            internal uint ReplyLength;
            internal ulong MessageId;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct INSTANCE_FULL_INFORMATION
        {
            internal uint NextEntryOffset;
            internal ushort InstanceNameLength;
            internal ushort InstanceNameBufferOffset;
            internal ushort AltitudeLength;
            internal ushort AltitudeBufferOffset;
            internal ushort VolumeNameLength;
            internal ushort VolumeNameBufferOffset;
            internal ushort FilterNameLength;
            internal ushort FilterNameBufferOffset;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
        internal struct SP_DEVICE_INTERFACE_DETAIL_DATA32
        {
            internal uint cbSize;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            internal char[] DevicePath;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct SP_DEVICE_INTERFACE_DETAIL_DATA64
        {
            internal uint cbSize;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            internal char[] DevicePath;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct SP_DEVICE_INTERFACE_DATA
        {
            internal uint cbSize;
            internal Guid InterfaceClassGuid;
            internal uint Flags;
            internal IntPtr Reserved;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct FILTER_VOLUME_BASIC_INFORMATION
        {
            internal ushort FilterVolumeNameLength;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            internal char[] FilterVolumeName;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct LUID
        {
            internal uint LowPart;
            internal int HighPart;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct OBJECT_ATTRIBUTES
        {
            internal uint Length;
            internal IntPtr RootDirectory;
            internal IntPtr ObjectName;
            internal uint Attributes;
            internal IntPtr SecurityDescriptor;
            internal IntPtr SecurityQualityOfService;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct UNICODE_STRING
        {
            internal ushort Length;
            internal ushort MaximumLength;
            internal IntPtr Buffer;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct SECURITY_LOGON_SESSION_DATA
        {
            internal uint Size;
            internal LUID LogonId;
            internal LSA_UNICODE_STRING UserName;
            internal LSA_UNICODE_STRING LogonDomain;
            internal LSA_UNICODE_STRING AuthenticationPackage;
            internal uint LogonType;
            internal uint Session;
            internal IntPtr Sid;
            internal long LogonTime;
            internal LSA_UNICODE_STRING LogonServer;
            internal LSA_UNICODE_STRING DnsDomainName;
            internal LSA_UNICODE_STRING Upn;

            internal uint UserFlags;
            internal LSA_LAST_INTER_LOGON_INFO LastLogonInfo;
            internal LSA_UNICODE_STRING LogonScript;
            internal LSA_UNICODE_STRING ProfilePath;
            internal LSA_UNICODE_STRING HomeDirectory;
            internal LSA_UNICODE_STRING HomeDirectoryDrive;
            internal long LogoffTime;
            internal long KickOffTime;
            internal long PasswordLastSet;
            internal long PasswordCanChange;
            internal long PasswordMustChange;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct LSA_LAST_INTER_LOGON_INFO
        {
            internal long LastSuccessfulLogon;
            internal long LastFailedLogon;
            internal uint FailedAttemptCountSinceLastSuccessfulLogon;
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct LSA_UNICODE_STRING
        {
            internal ushort Length;
            internal ushort MaximumLength;
            internal IntPtr Buffer;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct DS_NAME_RESULT
        {
            internal uint cItems;
            internal IntPtr rItems;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct DS_NAME_RESULT_ITEM
        {
            internal uint status;
            internal IntPtr pDomain;
            internal IntPtr pName;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct POLICY_DNS_DOMAIN_INFO
        {
            internal LSA_UNICODE_STRING Name;
            internal LSA_UNICODE_STRING DnsDomainName;
            internal LSA_UNICODE_STRING DnsForestName;
            internal Guid DomainGuid;
            internal IntPtr Sid;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct POLICY_ACCOUNT_DOMAIN_INFO
        {
            internal LSA_UNICODE_STRING DomainName;
            internal IntPtr DomainSid;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct WTS_SESSION_INFO
        {
            internal uint SessionId;
            internal IntPtr pWinStationName;
            [MarshalAs(UnmanagedType.U4)]
            internal WTS_CONNECTSTATE_CLASS State;
        }


        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct MIB_IPNET_ROW2
        {
            internal SOCKADDR_INET Address;
            internal uint InterfaceIndex;
            internal ulong InterfaceLuid;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)IF_MAX_PHYS_ADDRESS_LENGTH)]
            internal byte[] PhysicalAddress;
            internal uint PhysicalAddressLength;
            internal NL_NEIGHBOR_STATE State;
            internal byte Flags;
            internal uint LastReachable;
        }

        [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
        internal struct SOCKADDR_INET
        {
            [FieldOffset(0)]
            internal SOCKADDR_INET_DATA RawData;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct SOCKADDR_INET_DATA
        {
            internal byte b00;
            internal byte b01;
            internal byte b02;
            internal byte b03;
            internal byte b04;
            internal byte b05;
            internal byte b06;
            internal byte b07;
            internal byte b08;
            internal byte b09;
            internal byte b10;
            internal byte b11;
            internal byte b12;
            internal byte b13;
            internal byte b14;
            internal byte b15;
            internal byte b16;
            internal byte b17;
            internal byte b18;
            internal byte b19;
            internal byte b20;
            internal byte b21;
            internal byte b22;
            internal byte b23;
            internal byte b24;
            internal byte b25;
            internal byte b26;
            internal byte b27;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct SOCKADDR_IN6
        {
            internal short sin6_family;
            internal ushort sin6_port;
            internal uint sin6_flowinfo;
            internal IN6_ADDR sin6_addr;
            internal uint sin6_scope_id;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct SOCKADDR_IN
        {
            internal short sin_family;
            internal ushort sin_port;
            internal IN_ADDR sin_addr;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            internal byte[] sin_zero;
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct IN6_ADDR
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            internal byte[] Byte;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct IN_ADDR
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            internal byte[] S_un_b;
        }

        internal static uint HRESULT_FROM_WIN32(uint x)
        {
            return (x <= 0) ? x : ((x & 0x0000FFFF) | (FACILITY_WIN32 << 16) | 0x80000000);
        }

        internal static uint HRESULT_CODE(uint hr)
        {
            return (hr & 0xFFFF);
        }

        internal delegate void DIFLOGCALLBACK(
            [In, MarshalAs(UnmanagedType.U4)] DIFXAPI_LOG Event,
            [In] int Error,
            [In, MarshalAs(UnmanagedType.LPWStr)] string EventDescription,
            [In] IntPtr CallbackContext);

        [DllImport("advapi32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern int RegCloseKey(
            [In] IntPtr hKey);

        [DllImport("advapi32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern int RegNotifyChangeKeyValue(
            [In] IntPtr hKey,
            [In, MarshalAs(UnmanagedType.Bool)] bool bWatchSubtree,
            [In] uint dwNotifyFilter,
            [In, Optional] IntPtr hEvent,
            [In, MarshalAs(UnmanagedType.Bool)] bool fAsynchronous);

        [DllImport("advapi32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern int RegOpenKeyEx(
            [In] IntPtr hKey,
            [In, Optional, MarshalAs(UnmanagedType.LPWStr)] string lpSubKey,
            [In] uint ulOptions,
            [In] uint samDesired,
            [Out] IntPtr phkResult);

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint GetTickCount();

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool QueryPerformanceCounter(
            [Out] IntPtr lpPerformanceCount);

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool QueryPerformanceFrequency(
            [Out] IntPtr lpFrequency);

        [DllImport("Ntdsapi", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint DsCrackNames(
            [In] IntPtr hDS,
            [In, MarshalAs(UnmanagedType.U4)] DS_NAME_FLAGS flags,
            [In, MarshalAs(UnmanagedType.U4)] DS_NAME_FORMAT formatOffered,
            [In, MarshalAs(UnmanagedType.U4)] DS_NAME_FORMAT formatDesired,
            [In] uint cNames,
            [In] IntPtr rpNames,
            [Out] IntPtr ppResult);

        [DllImport("Ntdsapi", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern void DsFreeNameResult(
            [In] IntPtr pResult);

        [DllImport("Ntdsapi", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint DsBind(
            [In, Optional, MarshalAs(UnmanagedType.LPWStr)] string DomainControllerName,
            [In, Optional, MarshalAs(UnmanagedType.LPWStr)] string DnsDomainName,
            [Out] IntPtr phDS);

        [DllImport("Ntdsapi", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint DsBindWithCred(
            [In, Optional, MarshalAs(UnmanagedType.LPWStr)] string DomainControllerName,
            [In, Optional, MarshalAs(UnmanagedType.LPWStr)] string DnsDomainName,
            [In, Optional] IntPtr AuthIdentity,
            [Out] IntPtr phDS);

        [DllImport("Ntdsapi", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint DsMakePasswordCredentials(
            [In, MarshalAs(UnmanagedType.LPWStr)] string User,
            [In, MarshalAs(UnmanagedType.LPWStr)] string Domain,
            [In, MarshalAs(UnmanagedType.LPWStr)] string Password,
            [Out] IntPtr pAuthIdentity);

        [DllImport("Ntdsapi", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern void DsFreePasswordCredentials(
            [In] IntPtr AuthIdentity);

        [DllImport("Ntdsapi", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint DsUnBind(
            [In] IntPtr phDS);

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern IntPtr CreateIoCompletionPort(
            [In] IntPtr FileHandle,
            [In, Optional] IntPtr ExistingCompletionPort,
            [In] IntPtr CompletionKey,
            [In] uint NumberOfConcurrentThreads);

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetQueuedCompletionStatus(
            [In] IntPtr CompletionPort,
            [Out] IntPtr lpNumberOfBytes,
            [Out] IntPtr lpCompletionKey,
            [Out] IntPtr lpOverlapped,
            [In] uint dwMilliseconds);

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint FormatMessage(
            [In] uint dwFlags,
            [In, Optional] IntPtr lpSource,
            [In] uint dwMessageId,
            [In] uint dwLanguageId,
            [Out] IntPtr lpBuffer,
            [In] uint nSize,
            [In, Optional] IntPtr Arguments);

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint GetLastError();

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern void SetLastError(
            [In] uint dwErrCode);

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CloseHandle(
            [In] IntPtr hObject);

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DuplicateHandle(
            [In] IntPtr hSourceProcessHandle,
            [In] IntPtr hSourceHandle,
            [In] IntPtr hTargetProcessHandle,
            [Out] IntPtr lpTargetHandle,
            [In] uint dwDesiredAccess,
            [In, MarshalAs(UnmanagedType.Bool)] bool bInheritHandle,
            [In] uint dwOptions);

        [DllImport("advapi32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool ChangeServiceConfig(
            [In] IntPtr hService,
            [In] uint dwServiceType,
            [In] uint dwStartType,
            [In] uint dwErrorControl,
            [In, Optional, MarshalAs(UnmanagedType.LPWStr)] string lpBinaryPathName,
            [In, Optional, MarshalAs(UnmanagedType.LPWStr)] string lpLoadOrderGroup,
            [Out, Optional] IntPtr lpdwTagId,
            [In, Optional, MarshalAs(UnmanagedType.LPWStr)] string lpDependencies,
            [In, Optional, MarshalAs(UnmanagedType.LPWStr)] string lpServiceStartName,
            [In, Optional, MarshalAs(UnmanagedType.LPWStr)] string lpPassword,
            [In, Optional, MarshalAs(UnmanagedType.LPWStr)] string lpDisplayName);

        [DllImport("advapi32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool ChangeServiceConfig2(
            [In] IntPtr hService,
            [In] uint dwInfoLevel,
            [In, Optional] IntPtr lpInfo);

        [DllImport("advapi32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CloseServiceHandle(
            [In] IntPtr hSCObject);

        [DllImport("advapi32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool ControlService(
            [In] IntPtr hService,
            [In] uint dwControl,
            [Out] IntPtr lpServiceStatus);

        [DllImport("advapi32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern IntPtr CreateService(
            [In] IntPtr hSCManager,
            [In, MarshalAs(UnmanagedType.LPWStr)] string lpServiceName,
            [In, Optional, MarshalAs(UnmanagedType.LPWStr)] string lpDisplayName,
            [In] uint dwDesiredAccess,
            [In] uint dwServiceType,
            [In] uint dwStartType,
            [In] uint dwErrorControl,
            [In, Optional, MarshalAs(UnmanagedType.LPWStr)] string lpBinaryPathName,
            [In, Optional, MarshalAs(UnmanagedType.LPWStr)] string lpLoadOrderGroup,
            [Out, Optional] IntPtr lpdwTagId,
            [In, Optional, MarshalAs(UnmanagedType.LPWStr)] string lpDependencies,
            [In, Optional, MarshalAs(UnmanagedType.LPWStr)] string lpServiceStartName,
            [In, Optional, MarshalAs(UnmanagedType.LPWStr)] string lpPassword);

        [DllImport("advapi32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DeleteService(
            [In] IntPtr hService);

        [DllImport("advapi32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern IntPtr OpenSCManager(
            [In, Optional, MarshalAs(UnmanagedType.LPWStr)] string lpMachineName,
            [In, Optional, MarshalAs(UnmanagedType.LPWStr)] string lpDatabaseName,
            [In] uint dwDesiredAccess);

        [DllImport("advapi32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern IntPtr OpenService(
            [In] IntPtr hSCManager,
            [In, MarshalAs(UnmanagedType.LPWStr)] string lpServiceName,
            [In] uint dwDesiredAccess);

        [DllImport("advapi32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool QueryServiceConfig(
            [In] IntPtr hService,
            [Out, Optional] IntPtr lpServiceConfig,
            [In] uint cbBufSize,
            [Out] IntPtr pcbBytesNeeded);

        [DllImport("advapi32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool QueryServiceConfig2(
            [In] IntPtr hService,
            [In] uint dwInfoLevel,
            [Out, Optional] IntPtr lpBuffer,
            [In] uint cbBufSize,
            [Out] IntPtr pcbBytesNeeded);

        [DllImport("advapi32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool QueryServiceStatus(
            [In] IntPtr hService,
            [Out, Optional] IntPtr lpServiceStatus);

        [DllImport("advapi32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool StartService(
            [In] IntPtr hService,
            [In] uint dwNumServiceArgs,
            [In, Optional] IntPtr lpServiceArgVectors);

        [DllImport("advapi32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern IntPtr RegisterServiceCtrlHandlerEx(
            [In, MarshalAs(UnmanagedType.LPWStr)] string lpServiceName,
            [In] IntPtr lpHandlerProc,
            [In, Optional] IntPtr lpContext);

        [DllImport("advapi32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetServiceStatus(
            [In] IntPtr hServiceStatus,
            [In] IntPtr lpServiceStatus);

        [DllImport("advapi32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool StartServiceCtrlDispatcher(
            [In] IntPtr lpServiceTable);

        [DllImport("fltlib", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint FilterAttach(
            [In, MarshalAs(UnmanagedType.LPWStr)] string lpFilterName,
            [In, MarshalAs(UnmanagedType.LPWStr)] string lpVolumeName,
            [In, Optional, MarshalAs(UnmanagedType.LPWStr)] string lpInstanceName,
            [In, Optional] uint dwCreatedInstanceNameLength,
            [Out, Optional] IntPtr lpCreatedInstanceName);

        [DllImport("fltlib", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint FilterClose(
            [In] IntPtr hFilter);

        [DllImport("fltlib", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint FilterConnectCommunicationPort(
            [In, MarshalAs(UnmanagedType.LPWStr)] string lpPortName,
            [In] uint dwOptions,
            [In, Optional] IntPtr lpContext,
            [In] ushort dwSizeOfContext,
            [In, Optional] IntPtr lpSecurityAttributes,
            [Out] IntPtr hPort);

        [DllImport("fltlib", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint FilterCreate(
            [In, MarshalAs(UnmanagedType.LPWStr)] string lpFilterName,
            [Out] IntPtr hFilter);

        [DllImport("fltlib", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint FilterDetach(
            [In, MarshalAs(UnmanagedType.LPWStr)] string lpFilterName,
            [In, MarshalAs(UnmanagedType.LPWStr)] string lpVolumeName,
            [In, Optional, MarshalAs(UnmanagedType.LPWStr)] string lpInstanceName);

        [DllImport("fltlib", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint FilterFindClose(
            [In] IntPtr hFilterFind);

        [DllImport("fltlib", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint FilterGetMessage(
            [In] IntPtr hPort,
            [Out] IntPtr lpMessageBuffer,
            [In] uint dwMessageBufferSize,
            [In, Out] IntPtr lpOverlapped);

        [DllImport("fltlib", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint FilterInstanceFindClose(
            [In] IntPtr hFilterInstanceFind);

        [DllImport("fltlib", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint FilterInstanceFindFirst(
            [In, MarshalAs(UnmanagedType.LPWStr)] string lpFilterName,
            [In, MarshalAs(UnmanagedType.U4)] INSTANCE_INFORMATION_CLASS dwInformationClass,
            [Out] IntPtr lpBuffer,
            [In] uint dwBufferSize,
            [Out] IntPtr lpBytesReturned,
            [Out] IntPtr lpFilterInstanceFind);

        [DllImport("fltlib", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint FilterInstanceFindNext(
            [In] IntPtr hFilterInstanceFind,
            [In, MarshalAs(UnmanagedType.U4)] INSTANCE_INFORMATION_CLASS dwInformationClass,
            [Out] IntPtr lpBuffer,
            [In] uint dwBufferSize,
            [Out] IntPtr lpBytesReturned);

        [DllImport("fltlib", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint FilterLoad(
            [In, MarshalAs(UnmanagedType.LPWStr)] string lpFilterName);

        [DllImport("fltlib", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint FilterSendMessage(
            [In] IntPtr hPort,
            [In, Optional] IntPtr lpInBuffer,
            [In] uint dwInBufferSize,
            [Out] IntPtr lpOutBuffer,
            [In] uint dwOutBufferSize,
            [Out] IntPtr lpBytesReturned);

        [DllImport("fltlib", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint FilterUnload(
            [In, MarshalAs(UnmanagedType.LPWStr)] string lpFilterName);

        [DllImport("fltlib", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint FilterVolumeFindClose(
            [In] IntPtr hVolumeFind);

        [DllImport("fltlib", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint FilterVolumeFindFirst(
            [In, MarshalAs(UnmanagedType.U4)] FILTER_VOLUME_INFORMATION_CLASS dwInformationClass,
            [Out] IntPtr lpBuffer,
            [In] uint dwBufferSize,
            [Out] IntPtr lpBytesReturned,
            [Out] IntPtr lpFilterFind);

        [DllImport("fltlib", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint FilterVolumeFindNext(
            [In] IntPtr hFilterFind,
            [In, MarshalAs(UnmanagedType.U4)] FILTER_VOLUME_INFORMATION_CLASS dwInformationClass,
            [Out] IntPtr lpBuffer,
            [In] uint dwBufferSize,
            [Out] IntPtr lpBytesReturned);

        [DllImport("fltlib", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint FilterVolumeInstanceFindClose(
            [In] IntPtr hVolumeInstanceFind);

        [DllImport("setupapi", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetupDiCallClassInstaller(
            [In, MarshalAs(UnmanagedType.U4)] DI_FUNCTION InstallFunction,
            [In] IntPtr DeviceInfoSet,
            [In, Optional] IntPtr DeviceInfoData);

        [DllImport("setupapi", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern IntPtr SetupDiCreateDeviceInfoListEx(
            [In, Optional] IntPtr ClassGuid,
            [In, Optional] IntPtr hwndParent,
            [In, MarshalAs(UnmanagedType.LPWStr)] string MachineName,
            [In] IntPtr Reserved);

        [DllImport("setupapi", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetupDiDestroyDeviceInfoList(
            [In] IntPtr DeviceInfoSet);

        [DllImport("setupapi", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetupDiEnumDeviceInfo(
            [In] IntPtr DeviceInfoSet,
            [In] uint MemberIndex,
            [Out] IntPtr DeviceInfoData);

        [DllImport("setupapi", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetupDiEnumDeviceInterfaces(
            [In] IntPtr DeviceInfoSet,
            [In, Optional] IntPtr DeviceInfoData,
            [In] IntPtr InterfaceClassGuid,
            [In] uint MemberIndex,
            [Out] IntPtr DeviceInterfaceData);

        [DllImport("setupapi", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern IntPtr SetupDiGetClassDevsEx(
            [In, Optional] IntPtr ClassGuid,
            [In, Optional, MarshalAs(UnmanagedType.LPWStr)] string Enumerator,
            [In, Optional] IntPtr hwndParent,
            [In] uint Flags,
            [In, Optional] IntPtr DeviceInfoSet,
            [In, Optional, MarshalAs(UnmanagedType.LPWStr)] string MachineName,
            [In] IntPtr Reserved);

        [DllImport("setupapi", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetupDiGetClassRegistryProperty(
            [In] IntPtr ClassGuid,
            [In] uint Property,
            [Out, Optional] IntPtr PropertyRegDataType,
            [Out] IntPtr PropertyBuffer,
            [In] uint PropertyBufferSize,
            [Out, Optional] IntPtr RequiredSize,
            [In, Optional, MarshalAs(UnmanagedType.LPWStr)] string MachineName,
            [In] IntPtr Reserved);

        [DllImport("setupapi", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetupDiGetDeviceInstallParams(
            [In] IntPtr DeviceInfoSet,
            [In, Optional] IntPtr DeviceInfoData,
            [Out] IntPtr DeviceInstallParams);

        [DllImport("setupapi", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetupDiGetDeviceInstanceId(
            [In] IntPtr DeviceInfoSet,
            [In] IntPtr DeviceInfoData,
            [Out, Optional] IntPtr DeviceInstanceId,
            [In] uint DeviceInstanceIdSize,
            [Out, Optional] IntPtr RequiredSize);

        [DllImport("setupapi", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetupDiGetDeviceInterfaceDetail(
            [In] IntPtr DeviceInfoSet,
            [In] IntPtr DeviceInterfaceData,
            [Out, Optional] IntPtr DeviceInterfaceDetailData,
            [In] uint DeviceInterfaceDetailDataSize,
            [Out, Optional] IntPtr RequiredSize,
            [Out, Optional] IntPtr DeviceInfoData);

        [DllImport("setupapi", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetupDiGetDeviceRegistryProperty(
            [In] IntPtr DeviceInfoSet,
            [In] IntPtr DeviceInfoData,
            [In] uint Property,
            [Out, Optional] IntPtr PropertyRegDataType,
            [Out, Optional] IntPtr PropertyBuffer,
            [In] uint PropertyBufferSize,
            [Out, Optional] IntPtr RequiredSize);

        [DllImport("setupapi", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetupDiInstallClass(
            [In, Optional] IntPtr hwndParent,
            [In, MarshalAs(UnmanagedType.LPWStr)] string InfFileName,
            [In] uint Flags,
            [In, Optional] IntPtr FileQueue);

        [DllImport("setupapi", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern IntPtr SetupDiOpenClassRegKey(
            [In, Optional] IntPtr ClassGuid,
            [In] uint samDesired);

        [DllImport("setupapi", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetupDiOpenDeviceInfo(
            [In] IntPtr DeviceInfoSet,
            [In, MarshalAs(UnmanagedType.LPWStr)] string DeviceInstanceId,
            [In, Optional] IntPtr hwndParent,
            [In] uint OpenFlags,
            [Out, Optional] IntPtr DeviceInfoData);

        [DllImport("setupapi", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetupDiSetClassInstallParams(
            [In] IntPtr DeviceInfoSet,
            [In, Optional] IntPtr DeviceInfoData,
            [In, Optional] IntPtr ClassInstallParams,
            [In] uint ClassInstallParamsSize);

        [DllImport("setupapi", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetupDiSetClassRegistryProperty(
            [In] IntPtr ClassGuid,
            [In] uint Property,
            [In, Optional] IntPtr PropertyBuffer,
            [In] uint PropertyBufferSize,
            [In, Optional, MarshalAs(UnmanagedType.LPWStr)] string MachineName,
            [In] IntPtr Reserved);

        [DllImport("cfgmgr32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint CM_Connect_Machine(
            [In, Optional, MarshalAs(UnmanagedType.LPWStr)] string UNCServerName,
            [Out] IntPtr phMachine);

        [DllImport("cfgmgr32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint CM_Disconnect_Machine(
            [In, Optional] IntPtr hMachine);

        [DllImport("cfgmgr32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint CM_Get_DevNode_Status_Ex(
            [Out] IntPtr pulStatus,
            [Out] IntPtr pulProblemNumber,
            [In] uint dnDevInst,
            [In] uint ulFlags,
            [In, Optional] IntPtr hMachine);

        [DllImport("cfgmgr32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint CM_Get_Parent_Ex(
            [Out] IntPtr pdnDevInst,
            [In] uint dnDevInst,
            [In] uint ulFlags,
            [In, Optional] IntPtr hMachine);

        [DllImport("netapi32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint NetGetJoinInformation(
            [In, MarshalAs(UnmanagedType.LPWStr)] string lpServer,
            [Out] IntPtr lpNameBuffer,
            [Out] IntPtr BufferType);

        [DllImport("netapi32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint NetApiBufferFree(
            [In] IntPtr Buffer);

        [DllImport("mpr", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint WNetAddConnection2(
            [In] IntPtr lpNetResource,
            [In, MarshalAs(UnmanagedType.LPWStr)] string lpPassword,
            [In, MarshalAs(UnmanagedType.LPWStr)] string lpUsername,
            [In] uint dwFlags);

        [DllImport("mpr", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint WNetCancelConnection2(
            [In, MarshalAs(UnmanagedType.LPWStr)] string lpName,
            [In] uint dwFlags,
            [In, MarshalAs(UnmanagedType.Bool)] bool fForce);

        [DllImport("mpr", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint WNetOpenEnum(
            [In] uint dwScope,
            [In] uint dwType,
            [In] uint dwUsage,
            [In] IntPtr lpNetResource,
            [Out] IntPtr lphEnum);

        [DllImport("mpr", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint WNetCloseEnum(
            [In] IntPtr hEnum);

        [DllImport("mpr", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint WNetEnumResource(
            [In] IntPtr hEnum,
            [In, Out] IntPtr lpcCount,
            [Out] IntPtr lpBuffer,
            [In, Out] IntPtr lpBufferSize);

        [DllImport("netapi32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint NetServerEnum(
            [In, Optional, MarshalAs(UnmanagedType.LPWStr)] string servername,
            [In] uint level,
            [Out] IntPtr bufptr,
            [In] uint prefmaxlen,
            [Out] IntPtr entriesread,
            [Out] IntPtr totalentries,
            [In] uint servertype,
            [In, Optional, MarshalAs(UnmanagedType.LPWStr)] string domain,
            [In, Out] IntPtr resume_handle);

        [DllImport("netapi32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint NetWkstaGetInfo(
            [In, Optional, MarshalAs(UnmanagedType.LPWStr)] string servername,
            [In] uint level,
            [Out] IntPtr bufptr);

        [DllImport("netapi32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint NetServerGetInfo(
            [In, Optional, MarshalAs(UnmanagedType.LPWStr)] string servername,
            [In] uint level,
            [Out] IntPtr bufptr);

        [DllImport("netapi32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint DsRoleGetPrimaryDomainInformation(
            [In, Optional, MarshalAs(UnmanagedType.LPWStr)] string lpServer,
            [In, MarshalAs(UnmanagedType.U4)] DSROLE_PRIMARY_DOMAIN_INFO_LEVEL InfoLevel,
            [Out] IntPtr Buffer);

        [DllImport("netapi32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern void DsRoleFreeMemory(
            [In] IntPtr Buffer);

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DeviceIoControl(
            [In] IntPtr hDevice,
            [In] uint dwIoControlCode,
            [In, Optional] IntPtr lpInBuffer,
            [In] uint nInBufferSize,
            [Out, Optional] IntPtr lpOutBuffer,
            [In] uint nOutBufferSize,
            [Out, Optional] IntPtr lpBytesReturned,
            [In, Out, Optional] IntPtr lpOverlapped);

        [DllImport("user32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern IntPtr RegisterDeviceNotification(
            [In] IntPtr hRecipient,
            [In] IntPtr NotificationFilter,
            [In] uint Flags);

        [DllImport("user32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool UnregisterDeviceNotification(
            [In] IntPtr Handle);

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern IntPtr LocalFree(
            [In] IntPtr hMem);

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern void RtlZeroMemory(
            [In] IntPtr dest,
            [In] IntPtr size);

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint WTSGetActiveConsoleSessionId();

        [DllImport("advapi32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool RevertToSelf();

        [DllImport("advapi32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool LookupAccountName(
            [In, Optional, MarshalAs(UnmanagedType.LPWStr)] string lpSystemName,
            [In, MarshalAs(UnmanagedType.LPWStr)] string lpAccountName,
            [Out, Optional] IntPtr Sid,
            [In, Out] IntPtr cbSid,
            [Out, Optional] IntPtr ReferencedDomainName,
            [In, Out] IntPtr cchReferencedDomainName,
            [Out] IntPtr peUse);

        [DllImport("advapi32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool LogonUser(
            [In, MarshalAs(UnmanagedType.LPWStr)] string lpszUsername,
            [In, Optional, MarshalAs(UnmanagedType.LPWStr)] string lpszDomain,
            [In, Optional, MarshalAs(UnmanagedType.LPWStr)] string lpszPassword,
            [In] uint dwLogonType,
            [In] uint dwLogonProvider,
            [Out] IntPtr phToken);

        [DllImport("advapi32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool ImpersonateLoggedOnUser(
            [In] IntPtr hToken);

        [DllImport("secur32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint LsaEnumerateLogonSessions(
            [Out] IntPtr LogonSessionCount,
            [Out] IntPtr LogonSessionList);

        [DllImport("secur32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint LsaFreeReturnBuffer(
            [In] IntPtr Buffer);

        [DllImport("secur32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint LsaGetLogonSessionData(
            [In] IntPtr LogonId,
            [Out] IntPtr ppLogonSessionData);

        [DllImport("advapi32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool AdjustTokenPrivileges(
            [In] IntPtr TokenHandle,
            [In, MarshalAs(UnmanagedType.Bool)] bool DisableAllPrivileges,
            [In, Optional] IntPtr NewState,
            [In] uint BufferLength,
            [Out, Optional] IntPtr PreviousState,
            [Out, Optional] IntPtr ReturnLength);

        [DllImport("advapi32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool LookupPrivilegeValue(
            [In, Optional, MarshalAs(UnmanagedType.LPWStr)] string lpSystemName,
            [In, MarshalAs(UnmanagedType.LPWStr)] string lpName,
            [Out] IntPtr lpLuid);

        [DllImport("advapi32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool OpenProcessToken(
            [In] IntPtr ProcessHandle,
            [In] uint DesiredAccess,
            [Out] IntPtr TokenHandle);

        [DllImport("advapi32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool ConvertSidToStringSid(
            [In] IntPtr Sid,
            [Out] IntPtr StringSid);

        [DllImport("advapi32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetTokenInformation(
            [In] IntPtr TokenHandle,
            [In, MarshalAs(UnmanagedType.U4)] TOKEN_INFORMATION_CLASS TokenInformationClass,
            [Out, Optional] IntPtr TokenInformation,
            [In] uint TokenInformationLength,
            [Out] IntPtr ReturnLength);

        [DllImport("advapi32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint LsaClose(
            [In] IntPtr ObjectHandle);

        [DllImport("advapi32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint LsaFreeMemory(
            [In] IntPtr Buffer);

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern IntPtr GetCurrentProcess();

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern ushort GlobalAddAtom(
            [In, MarshalAs(UnmanagedType.LPWStr)] string lpString);

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern ushort GlobalDeleteAtom(
            [In] ushort nAtom);

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern ushort GlobalFindAtom(
            [In, MarshalAs(UnmanagedType.LPWStr)] string lpString);

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetComputerName(
            [Out] IntPtr lpBuffer,
            [In, Out] IntPtr lpnSize);

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetComputerNameEx(
        [In, MarshalAs(UnmanagedType.U4)] COMPUTER_NAME_FORMAT NameType,
        [Out] IntPtr lpBuffer,
        [In, Out] IntPtr lpnSize);

        [DllImport("ntdll", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint NtClose(
            [In] IntPtr Handle);

        [DllImport("wtsapi32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern void WTSFreeMemory(
            [In] IntPtr pMemory);

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool FreeLibrary(
            [In] IntPtr hModule);

        [DllImport("psapi", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint GetModuleFileNameEx(
            [In] IntPtr hProcess,
            [In, Optional] IntPtr hModule,
            [Out] IntPtr lpFilename,
            [In] uint nSize);

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, BestFitMapping = false)]
        internal static extern IntPtr GetProcAddress(
            [In] IntPtr hModule,
            [In, MarshalAs(UnmanagedType.LPStr)] string lpProcName);

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern IntPtr LoadLibrary(
            [In, MarshalAs(UnmanagedType.LPWStr)] string lpFileName);

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern IntPtr CreateEvent(
            [In, Optional] IntPtr lpEventAttributes,
            [In, MarshalAs(UnmanagedType.Bool)] bool bManualReset,
            [In, MarshalAs(UnmanagedType.Bool)] bool bInitialState,
            [In, Optional, MarshalAs(UnmanagedType.LPWStr)] string lpName);

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetEvent(
            [In] IntPtr hEvent);

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint WaitForSingleObject(
            [In] IntPtr hHandle,
            [In] uint dwMilliseconds);

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern IntPtr OpenProcess(
            [In] uint dwDesiredAccess,
            [In, MarshalAs(UnmanagedType.Bool)] bool bInheritHandle,
            [In] uint dwProcessId);

        [DllImport("ntdll", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint NtQuerySystemInformation(
            [In, MarshalAs(UnmanagedType.U4)] SYSTEM_INFORMATION_CLASS SystemInformationClass,
            [In, Out] IntPtr SystemInformation,
            [In] uint SystemInformationLength,
            [Out, Optional] IntPtr ReturnLength);

        [DllImport("ntdll", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint NtQueryObject(
            [In, Optional] IntPtr Handle,
            [In, MarshalAs(UnmanagedType.U4)] OBJECT_INFORMATION_CLASS ObjectInformationClass,
            [Out, Optional] IntPtr ObjectInformation,
            [In] uint ObjectInformationLength,
            [Out, Optional] IntPtr ReturnLength);

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern IntPtr CreateToolhelp32Snapshot(
            [In] uint dwFlags,
            [In] uint th32ProcessID);

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool Process32First(
            [In] IntPtr hSnapshot,
            [In, Out] IntPtr lppe);

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool Process32Next(
            [In] IntPtr hSnapshot,
            [Out] IntPtr lppe);

        [DllImport("ntdll", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern void RtlInitUnicodeString(
            [Out] IntPtr DestinationString,
            [In, Optional, MarshalAs(UnmanagedType.LPWStr)] string SourceString);

        [DllImport("ntdll", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint NtOpenFile(
            [Out] IntPtr FileHandle,
            [In] uint DesiredAccess,
            [In] IntPtr ObjectAttributes,
            [Out] IntPtr IoStatusBlock,
            [In] uint ShareAccess,
            [In] uint OpenOptions);

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern IntPtr CreateMailslot(
            [In, MarshalAs(UnmanagedType.LPWStr)] string lpName,
            [In] uint nMaxMessageSize,
            [In] uint lReadTimeout,
            [In, Optional] IntPtr lpSecurityAttributes);

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern IntPtr CreateFile(
            [In, MarshalAs(UnmanagedType.LPWStr)] string lpFileName,
            [In] uint dwDesiredAccess,
            [In] uint dwShareMode,
            [In, Optional] IntPtr lpSecurityAttributes,
            [In] uint dwCreationDisposition,
            [In] uint dwFlagsAndAttributes,
            [In, Optional] IntPtr hTemplateFile);

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool WriteFile(
            [In] IntPtr hFile,
            [In] IntPtr lpBuffer,
            [In] uint nNumberOfBytesToWrite,
            [Out, Optional] IntPtr lpNumberOfBytesWritten,
            [In, Out, Optional] IntPtr lpOverlapped);

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool ReadFile(
            [In] IntPtr hFile,
            [Out] IntPtr lpBuffer,
            [In] uint nNumberOfBytesToRead,
            [Out, Optional] IntPtr lpNumberOfBytesRead,
            [In, Out, Optional] IntPtr lpOverlapped);

        [DllImport("advapi32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint LsaOpenPolicy(
            [In] IntPtr SystemName,
            [In] IntPtr ObjectAttributes,
            [In] uint DesiredAccess,
            [In, Out] IntPtr PolicyHandle);

        [DllImport("advapi32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint LsaQueryInformationPolicy(
            [In] IntPtr PolicyHandle,
            [In, MarshalAs(UnmanagedType.U4)] POLICY_INFORMATION_CLASS InformationClass,
            [Out] IntPtr Buffer);
        
        [DllImport("wtsapi32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool WTSEnumerateSessions(
            [In()] IntPtr hServer,
            [In()] uint Reserved,
            [In()] uint Version,
            [Out()] IntPtr ppSessionInfo,
            [Out()] IntPtr pCount);

        [DllImport("wtsapi32", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool WTSQuerySessionInformation(
            [In()] IntPtr hServer,
            [In()] uint SessionId,
            [In(), MarshalAs(UnmanagedType.U4)] WTS_INFO_CLASS WTSInfoClass,
            [Out()] IntPtr ppBuffer,
            [Out()] IntPtr pBytesReturned);

        [DllImport("iphlpapi", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint ResolveIpNetEntry2(IntPtr Row, IntPtr SourceAddress);

        [DllImport("iphlpapi", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint ConvertInterfaceGuidToLuid(IntPtr InterfaceGuid, IntPtr InterfaceLuid);
        
        [DllImport("iphlpapi", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal static extern uint SendARP(
            [In()] uint DestIP,
            [In()] uint SrcIP,
            [Out()] IntPtr pMacAddr,
            [In(), Out()] IntPtr PhyAddrLen);
    }
}

// ReSharper restore InconsistentNaming
// ReSharper restore FieldCanBeMadeReadOnly.Global
// ReSharper restore MemberCanBePrivate.Global
