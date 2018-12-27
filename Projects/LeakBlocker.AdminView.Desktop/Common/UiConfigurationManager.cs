using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.AdminView.Desktop.Common
{
    internal sealed class UiConfigurationManager : IUiConfigurationManager
    {
        private IWaitHandle configurationGetProcess;
        private SimpleConfiguration configuration;
        private bool? wasPreviousConfig;
        private ReportConfiguration reportConfiguration;

        public event Action ConfigurationChanged;

        public UiConfigurationManager()
        {
            configurationGetProcess = SharedObjects.AsyncInvoker.Invoke(UpdatePreviousConfig);
        }

        public void LoadFromServer()
        {
            configurationGetProcess = SharedObjects.AsyncInvoker.Invoke(UpdatePreviousConfig);
        }

        private void UpdatePreviousConfig()
        {
            using (IConfigurationTools client = UiObjects.CreateConfigurationToolsClient())
            {
                configuration = client.LastConfiguration();
                wasPreviousConfig = client.HasConfiguration();
            }

            using (IReportTools client = UiObjects.CreateReportToolsClient())
            {
               reportConfiguration = client.LoadSettings();
            }
        }

        public void SaveNewConfiguration(SimpleConfiguration newConfiguration)
        {
            using (IConfigurationTools client = UiObjects.CreateConfigurationToolsClient())
            {
                client.SaveConfiguration(newConfiguration);

                configuration = newConfiguration;
            }

            if (ConfigurationChanged != null)
                ConfigurationChanged();
        }

        public void SaveReportConfiguration(ReportConfiguration newConfiguration)
        {
            using (IReportTools client = UiObjects.CreateReportToolsClient())
            {
                client.SaveSettings(newConfiguration);

                reportConfiguration = newConfiguration;
            }
        }

        public SimpleConfiguration Configuration
        {
            get
            {
                if (configuration == null)
                {
                    if (configurationGetProcess != null)
                        configurationGetProcess.Wait();
                }

                return configuration;
            }
        }

        public ReportConfiguration CurrentReportConfiguration
        {
            get
            {
                if (reportConfiguration == null)
                {
                    if (configurationGetProcess != null)
                        configurationGetProcess.Wait();
                }

                return reportConfiguration;
            }
        }

        public bool WasPreviousConfig
        {
            get
            {
                if (!wasPreviousConfig.HasValue)
                {
                    if (configurationGetProcess != null)
                        configurationGetProcess.Wait();
                }

                return wasPreviousConfig.Value;
            }
        }
    }
}
