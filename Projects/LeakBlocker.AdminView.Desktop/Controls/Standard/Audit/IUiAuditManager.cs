using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Audit;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.AdminView.Desktop.Controls.Standard.Audit
{
    internal interface IUiAuditManager
    {
        void OpenComputersAudit(ICollection<BaseComputerAccount> computers);

        void OpenUsersAudit(ICollection<BaseUserAccount> users);

        void OpenDevicesAudit(ICollection<DeviceDescription> devices);

        void OpenTypes(ICollection<AuditItemGroupType> types);
    }
}
