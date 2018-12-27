using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.Server.Service.InternalTools.LocalStorages
{
    internal interface IReportConfigurationStorage : IBaseConfigurationManager<ReportConfiguration>
    {
        ReportConfiguration GetCurrentOrDefaultConfiguration();
    }
}
