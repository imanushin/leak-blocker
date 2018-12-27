using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.SystemTools.Win32;
using LeakBlocker.Libraries.SystemTools.Win32.UnmanagedTypes;

namespace LeakBlocker.Libraries.SystemTools.ProcessTools
{
    /// <summary>
    /// The type of the events that are sent to the service by system.
    /// </summary>
    public enum ServiceEventType
    {
        /// <summary>
        /// Incorrect value.
        /// </summary>
        [ForbiddenToUse]
        [Obsolete("Incorrect value.", true)]
        [UsedImplicitly]
        None = 0,

        /// <summary>
        /// Session changed (user has logged on or off).
        /// </summary>
        Session,

        /// <summary>
        /// Device configuration changed.
        /// </summary>
        Device,

        /// <summary>
        /// Service is being stopped.
        /// </summary>
        Stop,

        /// <summary>
        /// Service parameters were changed.
        /// </summary>
        ParameterChange,

        /// <summary>
        /// Service is being paused.
        /// </summary>
        Pause,

        /// <summary>
        /// Service is being resumed from paused state.
        /// </summary>
        Continue,

        /// <summary>
        /// System is being suspended.
        /// </summary>
        SystemSuspend,

        /// <summary>
        /// System is resumed from suspended state.
        /// </summary>
        SystemResume,

        /// <summary>
        /// System is being shut down.
        /// </summary>
        SystemShutdown,

        /// <summary>
        /// System was started.
        /// </summary>
        SystemStart
    }

    /// <summary>
    /// Service event handler. This object will receive events sent by the system.
    /// </summary>
    public interface IServiceHandler
    {
        /// <summary>
        /// Triggered when application requests stopping the service.
        /// </summary>
        event Action StopRequested;

        /// <summary>
        /// Triggered when application requests service removal.
        /// </summary>
        event Action UninstallRequested;

        /// <summary>
        /// Callback for event handling. Called in a separate thread, so long processing will not cause system delays.
        /// </summary>
        /// <param name="eventType">Event type.</param>
        void HandleEvent(ServiceEventType eventType);
    }

    /// <summary>
    /// Class for running windows services.
    /// </summary>
    public static class WindowsServiceApplication
    {
        private static IntPtr statusHandle;
        private static IntPtr deviceNotification;
        private static NativeMethods.SERVICE_STATUS status;
        private static string name;
        private static IServiceHandler handler;

        private static uint acceptedControls = NativeMethods.SERVICE_ACCEPT_NETBINDCHANGE | NativeMethods.SERVICE_ACCEPT_PARAMCHANGE |
            NativeMethods.SERVICE_ACCEPT_SHUTDOWN | NativeMethods.SERVICE_ACCEPT_HARDWAREPROFILECHANGE |
            NativeMethods.SERVICE_ACCEPT_POWEREVENT | NativeMethods.SERVICE_ACCEPT_SESSIONCHANGE;

        // NativeMethods.SERVICE_ACCEPT_PRESHUTDOWN | 
        // NativeMethods.SERVICE_ACCEPT_TRIGGEREVENT | NativeMethods.SERVICE_ACCEPT_TIMECHANGE |
        // NativeMethods.SERVICE_ACCEPT_USERMODEREBOOT

        [NativeCallback]
        private delegate uint ServiceHandlerCallback(uint dwControl, uint dwEventType, IntPtr lpEventData, IntPtr lpContext);

        [NativeCallback]
        private delegate void ServiceMainCallback(uint argumentsCount, IntPtr argumentPointersArray);

        private static readonly ServiceHandlerCallback serviceHandlerCallback = ServiceHandler;
        private static readonly ServiceMainCallback serviceMainCallback = ServiceMain;
        
        /// <summary>
        /// Starts the service using the specified event handler.
        /// </summary>
        /// <param name="serviceName">Service name.</param>
        /// <param name="eventHandler">Event handler.</param>
        /// <param name="acceptStop">False means that service will not acceps stop commands from user applications.</param>
        /// <param name="acceptPause">False means that service will not acceps pause commands from user applications.</param>
        public static void Start(string serviceName, IServiceHandler eventHandler, bool acceptStop = false, bool acceptPause = false)
        {
            Check.ObjectIsNotNull(eventHandler, "eventHandler");

            if (acceptStop)
                acceptedControls |= NativeMethods.SERVICE_ACCEPT_STOP;
            if (acceptPause)
                acceptedControls |= NativeMethods.SERVICE_ACCEPT_PAUSE_CONTINUE;

            name = serviceName;

            if (handler != null)
                Exceptions.Throw(ErrorMessage.ServiceAlreadyRegistered);

            handler = eventHandler;
            handler.StopRequested += StopService;
            handler.UninstallRequested += UninstallRequested;
            using (var serviceNameBuffer = new UnmanagedUnicodeString(name))
            {
                var tableEntries = new [] 
                { 
                    new NativeMethods.SERVICE_TABLE_ENTRY
                        {
                        lpServiceName = +serviceNameBuffer,
                        lpServiceProc = DelegateTools.GetPointer(serviceMainCallback)
                    },
                    new NativeMethods.SERVICE_TABLE_ENTRY()                
                };

                using (var serviceTable = new UnmanagedArray<NativeMethods.SERVICE_TABLE_ENTRY>(tableEntries))
                {
                    if (!NativeMethods.StartServiceCtrlDispatcher(+serviceTable))
                        Log.Write(NativeErrors.GetLastErrorMessage("StartServiceCtrlDispatcher"));
                }
            }
        }

        private static void UninstallRequested()
        {
            SystemObjects.CreateWindowsService(name).Delete();
        }

        [NativeCallback]
        private static void ServiceMain(uint argumentsCount, IntPtr argumentPointersArray)
        {
            statusHandle = NativeMethods.RegisterServiceCtrlHandlerEx(name, DelegateTools.GetPointer(serviceHandlerCallback), IntPtr.Zero);

            if (statusHandle == IntPtr.Zero)
            {
                Log.Write(NativeErrors.GetLastErrorMessage("RegisterServiceCtrlHandlerEx"));
                SetServiceStatus(NativeMethods.SERVICE_STOPPED, 0);
                return;
            }
            status.dwServiceType = NativeMethods.SERVICE_WIN32_OWN_PROCESS;

            var deviceNotificationStructure = new NativeMethods.DEV_BROADCAST_DEVICEINTERFACE
            {
                dbcc_size = UnmanagedStructure<NativeMethods.DEV_BROADCAST_DEVICEINTERFACE>.GetSize(),
                dbcc_devicetype = NativeMethods.DBT_DEVTYP_DEVICEINTERFACE
            };

            using (var structure = new UnmanagedStructure<NativeMethods.DEV_BROADCAST_DEVICEINTERFACE>(deviceNotificationStructure))
            {
                deviceNotification = NativeMethods.RegisterDeviceNotification(statusHandle, +structure,
                    NativeMethods.DEVICE_NOTIFY_SERVICE_HANDLE | NativeMethods.DEVICE_NOTIFY_ALL_INTERFACE_CLASSES);
                if (deviceNotification == IntPtr.Zero)
                {
                    Log.Write(NativeErrors.GetLastErrorMessage("RegisterDeviceNotification"));
                    SetServiceStatus(NativeMethods.SERVICE_STOPPED, 0);
                    return;
                }
            }

            SetServiceStatus(NativeMethods.SERVICE_START_PENDING, 60000);
            SetServiceStatus(NativeMethods.SERVICE_RUNNING, 0);

            if (CheckSystemStartedEvent())
                SafeEventCaller(ServiceEventType.SystemStart);
        }

        private static bool CheckSystemStartedEvent()
        {
            bool result = false;

            try
            {
                IGlobalFlag globalFlag = SystemObjects.CreateGlobalFlag("LeakBlocker_Agent_RebootTrackingFlag_" + SharedObjects.Constants.VersionString);

                if (!globalFlag.Exists)
                {
                    globalFlag.Create();
                    result = true;
                }

                if (SystemObjects.TimeProvider.SystemUptime > TimeSpan.FromMinutes(5))
                    result = false;
            }
            catch (Exception exception)
            {
                Log.Write(exception);
            }

            return result;
        }

        [NativeCallback]
        private static uint ServiceHandler(uint dwControl, uint dwEventType, IntPtr lpEventData, IntPtr lpContext)
        {
            switch (dwControl)
            {
                case NativeMethods.SERVICE_CONTROL_SHUTDOWN:
                    SafeEventCaller(ServiceEventType.SystemShutdown);
                    StopService();
                    return NativeMethods.NO_ERROR;

                case NativeMethods.SERVICE_CONTROL_STOP:
                    StopService();
                    return NativeMethods.NO_ERROR;

                case NativeMethods.SERVICE_CONTROL_CONTINUE:
                    SetServiceStatus(NativeMethods.SERVICE_CONTINUE_PENDING, 60000);
                    SafeEventCaller(ServiceEventType.Continue);
                    SetServiceStatus(NativeMethods.SERVICE_RUNNING, 0);
                    return NativeMethods.NO_ERROR;

                case NativeMethods.SERVICE_CONTROL_PAUSE:
                    SetServiceStatus(NativeMethods.SERVICE_PAUSE_PENDING, 60000);
                    SafeEventCaller(ServiceEventType.Pause);
                    SetServiceStatus(NativeMethods.SERVICE_PAUSED, 0);
                    return NativeMethods.NO_ERROR;

                case NativeMethods.SERVICE_CONTROL_PARAMCHANGE:
                    SafeEventCaller(ServiceEventType.ParameterChange);
                    return NativeMethods.NO_ERROR;

                case NativeMethods.SERVICE_CONTROL_POWEREVENT:
                    switch (dwEventType)
                    {
                        case NativeMethods.PBT_APMRESUMESUSPEND:
                            SafeEventCaller(ServiceEventType.SystemResume);
                            break;
                        case NativeMethods.PBT_APMSUSPEND:
                            SafeEventCaller(ServiceEventType.SystemSuspend);
                            break;
                        case NativeMethods.PBT_APMBATTERYLOW:
                        case NativeMethods.PBT_APMOEMEVENT:
                        case NativeMethods.PBT_APMPOWERSTATUSCHANGE:
                        case NativeMethods.PBT_APMQUERYSUSPEND:
                        case NativeMethods.PBT_APMQUERYSUSPENDFAILED:
                        case NativeMethods.PBT_APMRESUMEAUTOMATIC:
                        case NativeMethods.PBT_APMRESUMECRITICAL:
                        case NativeMethods.PBT_POWERSETTINGCHANGE:
                            break;
                    }
                    return NativeMethods.NO_ERROR;

                case NativeMethods.SERVICE_CONTROL_SESSIONCHANGE:
                    switch (dwEventType)
                    {
                        case NativeMethods.WTS_CONSOLE_CONNECT:
                        case NativeMethods.WTS_CONSOLE_DISCONNECT:
                        case NativeMethods.WTS_REMOTE_CONNECT:
                        case NativeMethods.WTS_REMOTE_DISCONNECT:
                        case NativeMethods.WTS_SESSION_LOGON:
                        case NativeMethods.WTS_SESSION_LOGOFF:
                        case NativeMethods.WTS_SESSION_LOCK:
                        case NativeMethods.WTS_SESSION_UNLOCK:
                        case NativeMethods.WTS_SESSION_REMOTE_CONTROL:
                        case NativeMethods.WTS_SESSION_CREATE:
                        case NativeMethods.WTS_SESSION_TERMINATE:
                            SafeEventCaller(ServiceEventType.Session);
                            break;
                    }
                    return NativeMethods.NO_ERROR;

                case NativeMethods.SERVICE_CONTROL_DEVICEEVENT:
                    switch (dwEventType)
                    {
                        case NativeMethods.DBT_DEVICEARRIVAL:
                        case NativeMethods.DBT_DEVICEREMOVECOMPLETE:
                        case NativeMethods.DBT_DEVICEQUERYREMOVE:
                        case NativeMethods.DBT_DEVICEQUERYREMOVEFAILED:
                        case NativeMethods.DBT_DEVICEREMOVEPENDING:
                        case NativeMethods.DBT_CUSTOMEVENT:
                            SafeEventCaller(ServiceEventType.Device);
                            break;
                    }
                    return NativeMethods.NO_ERROR;

                case NativeMethods.SERVICE_CONTROL_HARDWAREPROFILECHANGE:
                    switch (dwEventType)
                    {
                        case NativeMethods.DBT_CONFIGCHANGED:
                        case NativeMethods.DBT_QUERYCHANGECONFIG:
                        case NativeMethods.DBT_CONFIGCHANGECANCELED:
                            break;
                    }
                    return NativeMethods.NO_ERROR;

                case NativeMethods.SERVICE_CONTROL_TIMECHANGE:
                case NativeMethods.SERVICE_CONTROL_TRIGGEREVENT:
                case NativeMethods.SERVICE_CONTROL_USERMODEREBOOT:
                case NativeMethods.SERVICE_CONTROL_INTERROGATE:
                case NativeMethods.SERVICE_CONTROL_NETBINDADD:
                case NativeMethods.SERVICE_CONTROL_NETBINDDISABLE:
                case NativeMethods.SERVICE_CONTROL_NETBINDENABLE:
                case NativeMethods.SERVICE_CONTROL_NETBINDREMOVE:
                case NativeMethods.SERVICE_CONTROL_PRESHUTDOWN:
                    return NativeMethods.NO_ERROR;

                default:
                    SafeEventCaller((ServiceEventType)dwEventType);
                    return NativeMethods.NO_ERROR;
            }
        }

        private static void SetServiceStatus(uint statusCode, uint time)
        {
            if ((statusCode == NativeMethods.SERVICE_START_PENDING) || (statusCode == NativeMethods.SERVICE_STOP_PENDING))
            {
                status.dwControlsAccepted = 0;
                status.dwCheckPoint++;
            }
            else
            {
                status.dwControlsAccepted = acceptedControls;
                status.dwCheckPoint = 0;
            }
            status.dwWaitHint = time;
            status.dwCurrentState = statusCode;

            using (var statusStructure = new UnmanagedStructure<NativeMethods.SERVICE_STATUS>(status))
            {
                if (NativeMethods.SetServiceStatus(statusHandle, +statusStructure))
                    Log.Write("Service status was set to {0}.", statusCode);
                else
                    Log.Write(NativeErrors.GetLastErrorMessage("SetServiceStatus"));
            }
        }

        private static void SafeEventCaller(ServiceEventType eventType, TimeSpan timeout = default(TimeSpan))
        {
            var thread = new Thread(() => SharedObjects.ExceptionSuppressor.Run(() => handler.HandleEvent(eventType)))
            {
                IsBackground = true
            };

            thread.Start();

            if (timeout == default(TimeSpan))
                return;

            bool threadCompleted = thread.Join(timeout);

            if (!threadCompleted)
                thread.Abort();
        }

        private static void StopService()
        {
            SafeEventCaller(ServiceEventType.Stop, new TimeSpan(0, 0, 10));

            SetServiceStatus(NativeMethods.SERVICE_STOP_PENDING, 0);

            if (deviceNotification != IntPtr.Zero)
            {
                if (!NativeMethods.UnregisterDeviceNotification(deviceNotification))
                    Log.Write(NativeErrors.GetLastErrorMessage("UnregisterDeviceNotification"));
                deviceNotification = IntPtr.Zero;
            }

            SetServiceStatus(NativeMethods.SERVICE_STOPPED, 0);
            Log.Write("Service stopped.");
        }
    }
}
