using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.SystemTools.Win32;

namespace LeakBlocker.Libraries.SystemTools.ProcessTools
{
    /// <summary>
    /// Service start types.
    /// </summary>
    public enum ServiceStartType
    {
        /// <summary>
        /// Incorrect value.
        /// </summary>
        [ForbiddenToUse]
        [Obsolete("Incorrect value.", true)]
        None,

        /// <summary>
        /// Service is started automatically when Windows starts.
        /// </summary>
        Automatic = (int)NativeMethods.SERVICE_AUTO_START,

        /// <summary>
        /// Service must be started manually.
        /// </summary>
        Manual = (int)NativeMethods.SERVICE_DEMAND_START,

        /// <summary>
        /// Service is disabled and cannot be started.
        /// </summary>
        Disabled = (int)NativeMethods.SERVICE_DISABLED
    }

    /// <summary>
    /// Service state.
    /// </summary>
    public enum ServiceState
    {
        /// <summary>
        /// Incorrect value.
        /// </summary>
        [ForbiddenToUse]
        [Obsolete("Incorrect value.", true)]
        None,

        /// <summary>
        /// Resume operation in progress.
        /// </summary>
        ContinuePending = (int)NativeMethods.SERVICE_CONTINUE_PENDING,

        /// <summary>
        /// Suspend operation in progress.
        /// </summary>
        PausePending = (int)NativeMethods.SERVICE_PAUSE_PENDING,

        /// <summary>
        /// Service is suspended.
        /// </summary>
        Paused = (int)NativeMethods.SERVICE_PAUSED,

        /// <summary>
        /// Service is running.
        /// </summary>
        Running = (int)NativeMethods.SERVICE_RUNNING,

        /// <summary>
        /// Start operation in progress.
        /// </summary>
        StartPending = (int)NativeMethods.SERVICE_START_PENDING,

        /// <summary>
        /// Stop operation in progress.
        /// </summary>
        StopPending = (int)NativeMethods.SERVICE_STOP_PENDING,

        /// <summary>
        /// Service is not running.
        /// </summary>
        Stopped = (int)NativeMethods.SERVICE_STOPPED
    }

    /// <summary>
    /// Service failure action types.
    /// </summary>
    public enum ServiceFailureAction
    {
        /// <summary>
        /// No action is performed.
        /// </summary>
        None = (int)NativeMethods.SC_ACTION_TYPE.SC_ACTION_NONE,

        /// <summary>
        /// Request system reboot.
        /// </summary>
        Reboot = (int)NativeMethods.SC_ACTION_TYPE.SC_ACTION_REBOOT,

        /// <summary>
        /// Restart the service.
        /// </summary>
        Restart = (int)NativeMethods.SC_ACTION_TYPE.SC_ACTION_RESTART,

        /// <summary>
        /// Run custom command.
        /// </summary>
        RunCommand = (int)NativeMethods.SC_ACTION_TYPE.SC_ACTION_RUN_COMMAND
    }

    /// <summary>
    /// Service error control types.
    /// </summary>
    public enum ServiceErrorControl
    {
        /// <summary>
        /// The startup program logs the error in the event log, if possible. If the last-known-good configuration is being started, 
        /// the startup operation fails. Otherwise, the system is restarted with the last-known good configuration.
        /// </summary>
        Critical = (int)NativeMethods.SERVICE_ERROR_CRITICAL,

        /// <summary>
        /// The startup program ignores the error and continues the startup operation.
        /// </summary>
        Ignore = (int)NativeMethods.SERVICE_ERROR_IGNORE,

        /// <summary>
        /// The startup program logs the error in the event log but continues the startup operation.
        /// </summary>
        Normal = (int)NativeMethods.SERVICE_ERROR_NORMAL,

        /// <summary>
        /// The startup program logs the error in the event log. If the last-known-good configuration is being started, the startup operation continues. 
        /// Otherwise, the system is restarted with the last-known-good configuration.
        /// </summary>
        Severe = (int)NativeMethods.SERVICE_ERROR_SEVERE
    }

    /// <summary>
    /// The service type. 
    /// </summary>
    public enum SystemServiceType
    {
        /// <summary>
        /// Incorrect value.
        /// </summary>
        [ForbiddenToUse]
        [Obsolete("Incorrect value.", true)]
        None = 0,

        /// <summary>
        /// File system driver service.
        /// </summary>
        FileSystemDriver = (int)NativeMethods.SERVICE_FILE_SYSTEM_DRIVER,

        /// <summary>
        /// Driver service.
        /// </summary>
        KernelDriver = (int)NativeMethods.SERVICE_KERNEL_DRIVER,

        /// <summary>
        /// Service that runs in its own process.
        /// </summary>
        OwnProcess = (int)NativeMethods.SERVICE_WIN32_OWN_PROCESS,

        /// <summary>
        /// Service that shares a process with one or more other services. 
        /// </summary>
        ShareProcess = (int)NativeMethods.SERVICE_WIN32_SHARE_PROCESS,

        /// <summary>
        /// Reserved.
        /// </summary>
        Adapter = (int)NativeMethods.SERVICE_ADAPTER,

        /// <summary>
        /// Reserved.
        /// </summary>
        RecognizedDriver = (int)NativeMethods.SERVICE_RECOGNIZER_DRIVER,
    }
    
    /// <summary>
    /// Windows service.
    /// </summary>
    public interface IWindowsService
    {
        /// <summary>
        /// Current service state.
        /// </summary>
        ServiceState State
        {
            get;
        }

        /// <summary>
        /// Service executable path with arguments.
        /// </summary>
        string CommandLine
        {
            get;
            set;
        }

        /// <summary>
        /// Service start type.
        /// </summary>
        ServiceStartType StartType
        {
            get;
            set;
        }

        /// <summary>
        /// Service type.
        /// </summary>
        SystemServiceType ServiceType
        {
            get;
            set;
        }

        /// <summary>
        /// Service error control type.
        /// </summary>
        ServiceErrorControl ErrorControl
        {
            get;
            set;
        }

        /// <summary>
        /// Service user-friendly name.
        /// </summary>
        string DisplayedName
        {
            get;
            set;
        }

        /// <summary>
        /// Service description.
        /// </summary>
        string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Load order group.
        /// </summary>
        string LoadOrderGroup
        {
            get;
            set;
        }

        /// <summary>
        /// Dependencies.
        /// </summary>
        ReadOnlySet<string> Dependencies
        {
            get;
        }

        /// <summary>
        /// The time after which to reset the failure count to zero if there are no failures.
        /// </summary>
        TimeSpan FailureCounterResetInterval
        {
            get;
            set;
        }

        /// <summary>
        /// Command that should be executed when the service fails.
        /// </summary>
        string FailureCommand
        {
            get;
            set;
        }

        /// <summary>
        /// Message that is displayed before reboot if reboot is requested after failure.
        /// </summary>
        string FailureRebootMessage
        {
            get;
            set;
        }

        /// <summary>
        /// List of actions that should be performed on failure.
        /// </summary>
        ReadOnlyList<ServiceFailureAction> FailureActions
        {
            get;
        }

        /// <summary>
        /// The name of the account under which the service should run.
        /// </summary>
        string UserName
        {
            get;
            set;
        }

        /// <summary>
        /// Account password.
        /// </summary>
        string Password
        {
            get;
            set;
        }

        /// <summary>
        /// Returns true if the service exists.
        /// </summary>
        bool Exists
        {
            get;
        }

        /// <summary>
        /// Creates the service.
        /// </summary>
        /// <param name="commandLine">Service command line.</param>
        /// <param name="serviceType">Service type.</param>
        void Create(string commandLine, SystemServiceType serviceType = SystemServiceType.OwnProcess);

        /// <summary>
        /// Removes the service.
        /// </summary>
        void Delete();

        /// <summary>
        /// Starts the service with the specified arguments.
        /// </summary>
        /// <param name="arguments">Service start arguments.</param>
        void Start(IList<string> arguments);

        /// <summary>
        /// Starts the service with optional arguments.
        /// </summary>
        /// <param name="arguments">Service start arguments.</param>
        void Start(params string[] arguments);

        /// <summary>
        /// Stops the service.
        /// </summary>
        void QueryStop();

        /// <summary>
        /// Sends custom command to the service.
        /// </summary>
        /// <param name="controlCode">Command identifier (value between 128 and 255).</param>
        void SendCustomCommand(int controlCode);

        /// <summary>
        /// Sets serivice dependencies.
        /// </summary>
        /// <param name="dependencies">Service dependencies.</param>
        void SetDependencies(IReadOnlyCollection<string> dependencies);

        /// <summary>
        /// Sets list of actions that should be performed on service failure.
        /// </summary>
        /// <param name="actions">List of actions.</param>
        void SetFailureActions(ReadOnlyList<ServiceFailureAction> actions);

        /// <summary>
        /// Waits until the service enters the specified state.
        /// Warning! There are some limitations: method checks service state periodically, so if between the checks service changes its state 
        /// to the specified value and then to some other value, such event will not be detected. Also service must exist.
        /// </summary>
        /// <param name="state">Required service state.</param>
        /// <param name="timeout">How long to wait.</param>
        bool WaitForState(ServiceState state, TimeSpan timeout = default(TimeSpan));
    }
}
