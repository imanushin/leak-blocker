using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.AdminView.Desktop.Common
{
    internal interface IUiConfigurationManager
    {
        event Action ConfigurationChanged;

        SimpleConfiguration Configuration
        {
            get;
        }

        ReportConfiguration CurrentReportConfiguration
        {
            get;
        }

        bool WasPreviousConfig
        {
            get;
        }

        void LoadFromServer();
        void SaveNewConfiguration(SimpleConfiguration newConfiguration);
        void SaveReportConfiguration(ReportConfiguration newConfiguration);
    }
}