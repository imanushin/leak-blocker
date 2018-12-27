using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Settings;
using LeakBlocker.Libraries.SystemTools.Drivers;

namespace LeakBlocker.Agent.Core
{
    internal interface IAgentServiceController
    {
        void Uninstall();

        void QueryMainTask();

        void QueryDelayedNetworkTask();
    }

    internal interface IAgentTaskManager
    {
        void OverrideConfiguration();
        
        void QueryTask();

        void SystemStartTask();

        void SystemShutdownTask();

        void ServiceStopTask();

        void NetworkTask(bool onlyUploadAudit = false);

        void MainTask();

        void DriverAttachedToVolume(IVolumeAttachMessage message);

        void DriverDetachedFromVolume(IVolumeDetachMessage message);

        void FileAccessNotification(IFileNotificationMessage message);

        void UnblockAllDevices();
    }
}
