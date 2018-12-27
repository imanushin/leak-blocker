using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Network;
using LeakBlocker.Libraries.Storage;
using LeakBlocker.Server.Service.InternalTools;
using LeakBlocker.Server.Service.Network.AdminView;
using LeakBlocker.Server.Service.Network.Agent;

namespace LeakBlocker.Server.Service.Engine
{
    internal static class MainInvoker
    {
        private static readonly IBaseNetworkHost serviceHost = SharedObjects.CreateBaseNetworkHost(SharedObjects.Constants.DefaultTcpPort, SharedObjects.Constants.DefaultTcpTimeout);

        private static readonly AccountToolsServer accountToolsServer = new AccountToolsServer();
        private static readonly DeviceToolsServer deviceToolsServer = new DeviceToolsServer();
        private static readonly ConfigurationToolsServer configurationToolsServer = new ConfigurationToolsServer();
        private static readonly StatusToolsServer statusToolsServer = new StatusToolsServer();
        private static readonly AuditToolsServer auditToolsServer = new AuditToolsServer();
        private static readonly ReportToolsServer reportsToolsServer = new ReportToolsServer();
        private static readonly LicenseToolsServer licenseToolsServer = new LicenseToolsServer();
        private static readonly AgentSetupPasswordToolsServer agentSetupPasswordTools = new AgentSetupPasswordToolsServer();
        private static readonly AgentInstallationToolsServer agentInstallationToolsServer = new AgentInstallationToolsServer();

        private static readonly AgentControlServer agentControlServer = new AgentControlServer();
        private static readonly LocalKeyAgreementTools localKeyAgreementTools = new LocalKeyAgreementTools();

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Start()
        {
            Log.Write("Starting service...");

            Log.Add("Local configuration folder: {0}".Combine(SharedObjects.Constants.UserDataFolder));

            try
            {
                StorageObjects.DatabaseInitializer.DatabasePassword = InternalObjects.AgentSetupPasswordManager.Current.Value;

                InternalObjects.AgentStatusObserver.EnqueueObserving();
                InternalObjects.ReportMailer.StartObserving();

                serviceHost.RegisterServer(agentControlServer);

                serviceHost.RegisterServer(localKeyAgreementTools);

                serviceHost.RegisterServer(auditToolsServer);
                serviceHost.RegisterServer(accountToolsServer);
                serviceHost.RegisterServer(deviceToolsServer);
                serviceHost.RegisterServer(configurationToolsServer);
                serviceHost.RegisterServer(statusToolsServer);
                serviceHost.RegisterServer(reportsToolsServer);
                serviceHost.RegisterServer(licenseToolsServer);
                serviceHost.RegisterServer(agentSetupPasswordTools);
                serviceHost.RegisterServer(agentInstallationToolsServer);
            }
            catch
            {
                try
                {
                    serviceHost.Dispose();
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                }

                throw;
            }

            SharedObjects.AsyncInvoker.Invoke(InternalObjects.AuditItemHelper.MainServiceStarted);

            Log.Write("Service started.");
        }

        public static void Stop()
        {
            InternalObjects.AuditItemHelper.MainServiceStopped();

            Disposable.DisposeSafe(serviceHost);
        }
    }
}
