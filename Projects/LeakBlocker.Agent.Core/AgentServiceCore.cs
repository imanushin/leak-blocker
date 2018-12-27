using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Entities.Settings;
using LeakBlocker.Libraries.SystemTools;
using LeakBlocker.Libraries.SystemTools.Devices;
using LeakBlocker.Libraries.SystemTools.Drivers;
using LeakBlocker.Libraries.SystemTools.ProcessTools;

namespace LeakBlocker.Agent.Core
{
    internal sealed class AgentServiceCore : Disposable, IServiceHandler, ILocalControlServerHandler, IDriverControllerHandler, 
        IDriverController, IAgentServiceController
    {
        private const int driverMessageQueueOverflowThreshold = 65536;

        private readonly IScheduler networkScheduler;
        private readonly IScheduler mainScheduler;
        private readonly IScheduler queryScheduler;
        private readonly IScheduler driverScheduler;
        private readonly IScheduler driverInstallerScheduler;
        private readonly IScheduler delayedNetworkTaskScheduler;

        private readonly ConcurrentQueue<object> driverEvents = new ConcurrentQueue<object>();
        private readonly ManualResetEvent processingAllowed = new ManualResetEvent(false);

        private readonly IStackStorage stackStorage;
        private IAgentDataStorage dataStorage;

        private ILocalControlServer localControlServer;
        private IAgentTaskManager agentTaskManager;
        
        private IDriverController fileSystemDriverController;

        private readonly ConcurrentQueue<ServiceEventType> unhandledEvents = new ConcurrentQueue<ServiceEventType>();

        public event Action StopRequested;
        public event Action UninstallRequested;

        private AgentServiceCore()
        {
            using (new TimeMeasurement("Initialization"))
            {
                try
                {
                    stackStorage = AgentObjects.CreateStackStorage(AgentObjects.AgentConstants.DatabaseFile);
                }
                catch (Exception exception)
                {
                    Log.Write(exception);
                    stackStorage = AgentObjects.CreateAlternativeStackStorage();
                }

                driverScheduler = SharedObjects.CreateScheduler(DriverTask, TimeSpan.MaxValue, true);
                driverInstallerScheduler = SharedObjects.CreateScheduler(InstallDriver, TimeSpan.MaxValue, true);

                networkScheduler = SharedObjects.CreateScheduler(NetworkTask, AgentObjects.AgentConstants.NetworkTaskInterval, true);
                mainScheduler = SharedObjects.CreateScheduler(MainTask, TimeSpan.MaxValue, true);
                queryScheduler = SharedObjects.CreateScheduler(QueryTask, TimeSpan.MaxValue, true);
                delayedNetworkTaskScheduler = SharedObjects.CreateScheduler(DelayedNetworkTask, TimeSpan.MaxValue, true);

                SharedObjects.AsyncInvoker.Invoke(InitialConfiguration);
            }
        }

        private void DelayedNetworkTask()
        {
            //Thread.Sleep(TimeSpan.FromMinutes(1));

            networkScheduler.RunNow();
        }

        void IAgentServiceController.QueryDelayedNetworkTask()
        {
            delayedNetworkTaskScheduler.RunNow();
        }

        private void InstallDriver()
        {
            using (new TimeMeasurement("Driver installation task"))
            {
                if (fileSystemDriverController != null)
                    DisposeSafe(fileSystemDriverController);
                try
                {
                    using (IDriverController controller = SystemObjects.CreateDriverController())
                    {
                        controller.Install();
                    }

                    Log.Write("Initializing driver controller.");

                    fileSystemDriverController = SystemObjects.CreateDriverController(this);
                }
                catch (Exception exception)
                {
                    Log.Write(exception);
                }
            }
        }

        private void StartProcessing()
        {
            Log.Write("Allowing processing.");
            processingAllowed.Set();

            IGlobalFlag globalFlag = SystemObjects.CreateGlobalFlag(AgentObjects.AgentConstants.ConfigurationAllowedGlobalFlag);
            globalFlag.Create();
        }

        private void StopProcessing()
        {
            Log.Write("Forbidding processing.");
            processingAllowed.Reset();

            IGlobalFlag globalFlag = SystemObjects.CreateGlobalFlag(AgentObjects.AgentConstants.ConfigurationAllowedGlobalFlag);
            globalFlag.Delete();
        }

        protected override void DisposeManaged()
        {
            using (new TimeMeasurement("Releasing resources"))
            {
                DisposeSafe(localControlServer);
                DisposeSafe(fileSystemDriverController);
                DisposeSafe(driverScheduler);
                DisposeSafe(networkScheduler);
                DisposeSafe(mainScheduler);
                DisposeSafe(queryScheduler);
                DisposeSafe(stackStorage);
                DisposeSafe(processingAllowed);
                DisposeSafe(driverInstallerScheduler);
            }
        }

        private void InitialConfiguration()
        {
            using (new TimeMeasurement("Initial configuration"))
            {
                if (AgentObjects.AgentPrivateStorage.FirstRun)
                {
                    Log.Write("First run. Waiting for installer.");

                    if (!AgentObjects.AgentInstaller.WaitForInstaller(AgentObjects.AgentConstants.InstallerTimeout))
                        Exceptions.Throw(ErrorMessage.Timeout, "Cannot complete initial configuration. Installer is not responding.");

                    AgentObjects.AgentPrivateStorage.FirstRun = false;
                }

                driverInstallerScheduler.RunNow();

                IAuditStorage auditStorage;
                using (new TimeMeasurement("Initializing data storage"))
                {
                    dataStorage = AgentObjects.CreateAgentDataStorage(AgentObjects.AgentConstants.AgentDataFile);
                }
                
                using (new TimeMeasurement("Initializing audit storage"))
                {
                    auditStorage = AgentObjects.CreateAuditStorage(stackStorage, dataStorage);
                }

                using (new TimeMeasurement("Initializing task manager"))
                {
                    agentTaskManager = AgentObjects.CreateAgentTaskManager(auditStorage, this, dataStorage, this);
                }

                using (new TimeMeasurement("Initializing local control service"))
                {
                    localControlServer = AgentObjects.CreateLocalControlServer(this);
                }

                StartProcessing();

                queryScheduler.RunNow();
                networkScheduler.RunNow();
            }
        }

        private void MainTask()
        {
            using (new TimeMeasurement("Main task"))
            {
                using (new TimeMeasurement("Waiting for permission."))
                {
                    processingAllowed.WaitOne();
                }

                if (agentTaskManager == null)
                {
                    Log.Write("Agent task manager is not yet initialized.");
                    return;
                }

                ServiceEventType unhandledEvent;
                while (unhandledEvents.TryDequeue(out unhandledEvent))
                {
                    switch (unhandledEvent)
                    {
                        case ServiceEventType.SystemStart:
                        case ServiceEventType.SystemResume:
                            agentTaskManager.SystemStartTask();
                            break;
                        case ServiceEventType.SystemShutdown:
                           // agentTaskManager.ServiceStopTask();
                            agentTaskManager.SystemShutdownTask();
                            break;

                        case ServiceEventType.SystemSuspend:
                            agentTaskManager.SystemShutdownTask();
                            break;
                    }
                }

                agentTaskManager.MainTask();
            }
        }

        private void QueryTask()
        {
            using (new TimeMeasurement("Query task"))
            {
                using (new TimeMeasurement("Waiting for permission"))
                {
                    processingAllowed.WaitOne();
                }

                if (agentTaskManager == null)
                {
                    Log.Write("Agent task manager is not yet initialized.");
                    return;
                }

                agentTaskManager.QueryTask();

                Log.Write("Scheduling main task.");
                mainScheduler.RunNow();
            }
        }

        private void NetworkTask()
        {
            using (new TimeMeasurement("Network task"))
            {
                using (new TimeMeasurement("Waiting for permission"))
                {
                    processingAllowed.WaitOne();
                }

                if (AgentObjects.AgentPrivateStorage.StandaloneMode)
                {
                    Log.Write("Standalone mode.");
                    return;
                }

                if (agentTaskManager == null)
                {
                    Log.Write("Agent task manager is not yet initialized.");
                    return;
                }

                agentTaskManager.NetworkTask();
            }
        }

        void IAgentServiceController.Uninstall()
        {
            using (new TimeMeasurement("Uninstall request processing"))
            {
                using (new TimeMeasurement("Waiting for permission"))
                {
                    processingAllowed.WaitOne();
                }

                Log.Write("Forbidding tasks.");
                StopProcessing();

                if (agentTaskManager != null)
                {
                    Log.Write("Unblocking all devices.");
                    SharedObjects.ExceptionSuppressor.Run(() => agentTaskManager.UnblockAllDevices());
                }

                if (UninstallRequested != null)
                {
                    Log.Write("Requesting service removal.");
                    UninstallRequested();
                }

                try
                {
                    Log.Write("Sending uninstall notification.");
                    SharedObjects.ExceptionSuppressor.Run(AgentObjects.AgentControlServiceClient.SendUninstallNotification);

                    agentTaskManager.NetworkTask(true);

                    DisposeSafe(fileSystemDriverController);
                    DisposeSafe(driverScheduler);
                    DisposeSafe(networkScheduler);
                    DisposeSafe(mainScheduler);
                    DisposeSafe(queryScheduler);
                    DisposeSafe(driverInstallerScheduler);

                    Log.Write("Uninstalling driver");
                    using (IDriverController controller = SystemObjects.CreateDriverController())
                    {
                        controller.Remove();
                    }
                    
                    if (StopRequested != null)
                        StopRequested();

                    if (dataStorage != null)
                        SharedObjects.ExceptionSuppressor.Run(dataStorage.Delete);
                    SharedObjects.ExceptionSuppressor.Run(stackStorage.Delete);
                    SharedObjects.ExceptionSuppressor.Run(SystemObjects.FileTools.Delete, AgentObjects.AgentConstants.UnexpectedTerminationFlagFile, default(SystemAccessOptions));

                    Log.Write("Uninstalling agent data.");
                    AgentObjects.AgentInstaller.UninstallSelf();
                }
                catch (Exception exception)
                {
                    Log.Write(exception);
                }
            }
        }

        void IAgentServiceController.QueryMainTask()
        {
            mainScheduler.RunNow();
        }

        void ILocalControlServerHandler.SetConfiguration()
        {
            using (new TimeMeasurement("Configuration update"))
            {
                if (agentTaskManager == null)
                {
                    Log.Write("Agent task manager is not yet initialized.");
                    return;
                }

                agentTaskManager.OverrideConfiguration();
            }
        }

        void ILocalControlServerHandler.RequestUninstall()
        {
            (this as IAgentServiceController).Uninstall();   
        }

        void IServiceHandler.HandleEvent(ServiceEventType eventType)
        {
            using (new TimeMeasurement("Service event processing (Event type: {0})".Combine(eventType)))
            {
                if (eventType == ServiceEventType.Stop)
                    StopProcessing();

                if (agentTaskManager == null)
                {
                    Log.Write("Agent task manager is not yet initialized.");
                    unhandledEvents.Enqueue(eventType);
                    return;
                }

                switch (eventType)
                {
                    case ServiceEventType.SystemStart:
                    case ServiceEventType.SystemResume:
                        agentTaskManager.SystemStartTask();
                        break;

                    case ServiceEventType.SystemShutdown:
                        //agentTaskManager.ServiceStopTask();
                        agentTaskManager.SystemShutdownTask();
                        break;

                    case ServiceEventType.SystemSuspend:
                        agentTaskManager.SystemShutdownTask();
                        break;

                    case ServiceEventType.Stop:
                        agentTaskManager.ServiceStopTask();
                        break;

                    case ServiceEventType.Device:
                        mainScheduler.RunNow();
                        break;

                    case ServiceEventType.Session:
                        queryScheduler.RunNow();
                        break;
                }
            }
        }

        private void DriverTask()
        {
            using (new TimeMeasurement("Driver task", true))
            {
                if (agentTaskManager == null)
                    return;

                object driverMessage;
                driverEvents.TryDequeue(out driverMessage);

                var volumeAttachMessage = driverMessage as IVolumeAttachMessage;
                if (volumeAttachMessage != null)
                    agentTaskManager.DriverAttachedToVolume(volumeAttachMessage);

                var volumeDetachMessage = driverMessage as IVolumeDetachMessage;
                if (volumeDetachMessage != null)
                    agentTaskManager.DriverDetachedFromVolume(volumeDetachMessage);

                var fileNotificationMessage = driverMessage as IFileNotificationMessage;
                if (fileNotificationMessage != null)
                    agentTaskManager.FileAccessNotification(fileNotificationMessage);
            }
        }

        void IDriverControllerHandler.VolumeAttachMessageReceived(IVolumeAttachMessage message)
        {
            using (new TimeMeasurement("Volume attach message procesing", true))
            {
                EnqueueDriverMessage(message);
            }
        }

        void IDriverControllerHandler.VolumeDetachMessageReceived(IVolumeDetachMessage message)
        {
            using (new TimeMeasurement("Volume detach message procesing", true))
            {
                EnqueueDriverMessage(message);
            }
        }

        void IDriverControllerHandler.FileNotificationMessageReceived(IFileNotificationMessage message)
        {
            using (new TimeMeasurement("File access message procesing", true))
            {
                EnqueueDriverMessage(message);
            }
        }

        void IDriverControllerHandler.VolumeListUpdateMessageReceived()
        {
            using (new TimeMeasurement("Volume list update message procesing", true))
            {
                mainScheduler.RunNow();
            }
        }

        bool IDriverController.Available
        {
            get
            {
                if (fileSystemDriverController == null)
                    return false;

                if (!fileSystemDriverController.Available)
                    driverInstallerScheduler.RunNow();

                return fileSystemDriverController.Available;
            }
        }

        void IDriverController.SetManagedVolumes(IReadOnlyCollection<VolumeName> volumes)
        {
            using (new TimeMeasurement("Managed volumes update", true))
            {
                if (fileSystemDriverController != null)
                    fileSystemDriverController.SetManagedVolumes(volumes);
            }
        }

        void IDriverController.SetInstanceConfiguration(long instanceIdentifier, ReadOnlyDictionary<AccountSecurityIdentifier, FileAccessType> rules)
        {
            using (new TimeMeasurement("Instance configuration update", true))
            {
                if (fileSystemDriverController != null)
                    fileSystemDriverController.SetInstanceConfiguration(instanceIdentifier, rules);
            }
        }

        void IDriverController.Install()
        {
            if (fileSystemDriverController != null)
                fileSystemDriverController.Install();
        }

        void IDriverController.Remove()
        {
            if (fileSystemDriverController != null)
                fileSystemDriverController.Remove();
        }

        private void EnqueueDriverMessage(object message)
        {
            using (new TimeMeasurement("Enqueuing driver message", true))
            {
                driverEvents.Enqueue(message);

                while (driverEvents.Count > driverMessageQueueOverflowThreshold)
                {
                    object driverMessage;
                    driverEvents.TryDequeue(out driverMessage);
                    Log.Write("Driver message queue overflow.");
                }

                driverScheduler.RunNow();
            }
        }

        [UsedImplicitly]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        private static int Main()
        {
            AppDomain.CurrentDomain.UnhandledException += delegate(object sender, UnhandledExceptionEventArgs e)
            {
                try
                {
                    Log.Write(e.ExceptionObject.ToString());
                }
                catch
                {
                }
            };

            using (new TimeMeasurement("Program"))
            {
                if (SharedObjects.CommandLine.Contains(AgentObjects.AgentConstants.InstallerCommandLineMode))
                {
                    Log.Write("Program started as installer.");

                    return AgentObjects.AgentInstaller.Start();
                }

                if (SharedObjects.CommandLine.Contains(AgentObjects.AgentConstants.ServiceCommandLineMode))
                {
                    Log.Write("Program started as service.");

                    using (var serviceCore = new AgentServiceCore())
                    {
                        WindowsServiceApplication.Start(AgentObjects.AgentConstants.ServiceName, serviceCore);
                        return 0;
                    }
                }

                return 0;
            }
        }
    }
}
