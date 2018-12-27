using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Resources;
using LeakBlocker.Libraries.SystemTools;
using LeakBlocker.Libraries.SystemTools.ProcessTools;

namespace LeakBlocker.Server.Installer.InternalTools
{
    internal sealed class ServiceHelper : IServiceHelper
    {
        private const string displayNameTemplate = "Leak Blocker Server v{0}";
        private const string serviceNameTemplate = "LeakBlockerServer{0}";

        private static readonly string serviceName = serviceNameTemplate.Combine(SharedObjects.Constants.VersionString);
        private static readonly string displayName = displayNameTemplate.Combine(SharedObjects.Constants.Version.ToString(3));

        public void Install()
        {
            CreateService();

            SharedObjects.ExceptionSuppressor.Run(InternalObjects.FirewallExceptionHelper.AddExceptionsForCurrentProcess);
        }

        private static void CreateService()
        {
            IWindowsService service = SystemObjects.CreateWindowsService(serviceName);

            if (service.Exists)
                return;

            service.Create(InternalObjects.FileSystemConstants.ServicePath);

            service.Description = ServerStrings.ServiceDescription;
            service.DisplayedName = displayName;
            service.StartType = ServiceStartType.Automatic;

            service.Start();
        }

        private static void RemoveService()
        {
            IWindowsService service = SystemObjects.CreateWindowsService(serviceName);

            if (!service.Exists)
                return;

            if (service.State != ServiceState.StopPending && service.State != ServiceState.Stopped)
                service.QueryStop();

            while (service.State == ServiceState.StopPending)
                Thread.Sleep(1000);

            service.Delete();
        }

        public void Uninstall()
        {
            SharedObjects.ExceptionSuppressor.Run(InternalObjects.FirewallExceptionHelper.RemoveExceptionsForCurrentProcess);

            RemoveService();
        }
    }
}
