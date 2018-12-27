using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using LeakBlocker.AdminView.Desktop.Common.Collections;
using LeakBlocker.AdminView.Desktop.Controls.Standard.Audit;
using LeakBlocker.AdminView.Desktop.Resources;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Resources;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.AdminView.Desktop
{
    internal sealed partial class App
    {
        private static string preferedDomain = null;
        private static IWaitHandle preferedDomainGetProcess = null;
        private static IUiAuditManager uiAuditManager;

        internal App()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;

            RunHostIfNeeded();

            UiObjects.UiConfigurationManager.LoadFromServer();

            preferedDomainGetProcess = SharedObjects.AsyncInvoker.Invoke(UpdatePreferedDomainGetProcess);

            CollectionContainer.AvailableComputerScopes.UpdateAsync(Dispatcher);
            CollectionContainer.AvailableComputers.UpdateAsync(Dispatcher);
            CollectionContainer.AvailableUserScopes.UpdateAsync(Dispatcher);
            CollectionContainer.ServerDevices.UpdateAsync(Dispatcher);
            CollectionContainer.AuditDevicesCollection.UpdateAsync(Dispatcher);
            
            InitializeComponent();
        }

        private static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var error = e.ExceptionObject as Exception;

            if (error != null)
            {
                Log.Write(e.ExceptionObject as Exception);

                MessageBox.Show(AdminViewResources.ErrorWasOccured.Combine(error.GetExceptionMessage()), CommonStrings.ProductName, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static void RunHostIfNeeded()
        {
            string[] args = Environment.GetCommandLineArgs();

            if (args.Length > 1 && args[1].ToUpperInvariant() == "/runHost".ToUpperInvariant())
            {
                Process.Start("LeakBlocker.Server.Service.exe", "/noservice " + Process.GetCurrentProcess().Id);
            }
        }

        private static void UpdatePreferedDomainGetProcess()
        {
            using (IAccountTools client = UiObjects.CreateAccountToolsClient())
            {
                preferedDomain = client.GetPreferableDomain();
            }
        }

        internal static string PreferedDomain
        {
            get
            {
                if (preferedDomain == null)
                {
                    if (preferedDomainGetProcess != null)
                        preferedDomainGetProcess.Wait();
                }

                return preferedDomain;
            }
        }

        internal static void RegisterAuditManager(IUiAuditManager auditManager)
        {
            if (uiAuditManager != null)
                throw new InvalidOperationException("Audit manager has already been initialized");

            uiAuditManager = auditManager;
        }

        internal static IUiAuditManager AuditManager
        {
            get
            {
                Check.ObjectIsNotNull(uiAuditManager);

                return uiAuditManager;
            }
        }
    }
}
