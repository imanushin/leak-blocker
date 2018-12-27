using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Network;
using LeakBlocker.Server.Service.Generated;
using LeakBlocker.Server.Service.InternalTools;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.Server.Service.Network.AdminView
{
    internal sealed class ReportToolsServer : GeneratedReportTools
    {
        public ReportToolsServer()
            : base((ISecuritySessionManager)InternalObjects.AdminViewSecuritySessionManager)
        {
        }

        protected override ReportConfiguration LoadSettings()
        {
            return InternalObjects.ReportConfigurationStorage.GetCurrentOrDefaultConfiguration();
        }

        protected override void SaveSettings(ReportConfiguration settings)
        {
            Check.ObjectIsNotNull(settings, "settings");

            InternalObjects.ReportConfigurationStorage.Save(settings);
        }

        protected override string TrySendTestReport(ReportConfiguration configuration)
        {
            Check.ObjectIsNotNull(configuration, "configuration");

            try
            {
                InternalObjects.ReportMailer.SendTestReport(configuration);
            }
            catch (Exception ex)
            {
                return ex.GetExceptionMessage();
            }

            return string.Empty;
        }
    }
}
