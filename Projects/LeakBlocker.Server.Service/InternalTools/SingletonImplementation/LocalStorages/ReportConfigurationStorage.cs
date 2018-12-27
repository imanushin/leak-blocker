using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Audit;
using LeakBlocker.Server.Service.InternalTools.LocalStorages;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.Server.Service.InternalTools.SingletonImplementation.LocalStorages
{
    internal sealed class ReportConfigurationStorage : BaseConfigurationManager<ReportConfiguration>, IReportConfigurationStorage
    {
        private const string fileName = "reports.lbConfig";

        public ReportConfigurationStorage()
            : base(fileName)
        {
        }

        public ReportConfiguration GetCurrentOrDefaultConfiguration()
        {
            ReportConfiguration result = Current;

            if (Current == null)
                return CreateDefaultReportConfiguration();

            return result;
        }

        private static ReportConfiguration CreateDefaultReportConfiguration()
        {
            return new ReportConfiguration(
                false,
                ReportFilter.Default,
                CreateEmailSettings());
        }

        private static EmailSettings CreateEmailSettings()
        {
            return new EmailSettings("", "", "", 25, false, false, String.Empty, String.Empty);
        }
    }
}
