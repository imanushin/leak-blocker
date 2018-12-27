using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.Server.Service.InternalTools
{
    internal interface IReportMailer
    {
        void StartObserving();

        void SendTestReport(ReportConfiguration config);
    }
}
