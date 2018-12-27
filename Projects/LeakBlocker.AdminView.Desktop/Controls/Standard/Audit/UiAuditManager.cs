using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Audit;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.AdminView.Desktop.Controls.Standard.Audit
{
    internal sealed class UiAuditManager : IUiAuditManager
    {
        public event Action<ICollection<BaseUserAccount>> UserAuditRequested;
        public event Action<ICollection<DeviceDescription>> DeviceAuditRequested;
        public event Action<ICollection<BaseComputerAccount>> ComputerAuditRequested;
        public event Action<ICollection<AuditItemGroupType>> TypedAuditRequested;

        public void OpenComputersAudit(ICollection<BaseComputerAccount> computers)
        {
            Check.CollectionIsNotNullOrEmpty(computers, "computers");
            Check.ObjectIsNotNull(ComputerAuditRequested);

            ComputerAuditRequested(computers);
        }

        public void OpenUsersAudit(ICollection<BaseUserAccount> users)
        {
            Check.CollectionIsNotNullOrEmpty(users, "users");
            Check.ObjectIsNotNull(UserAuditRequested);

            UserAuditRequested(users);
        }

        public void OpenDevicesAudit(ICollection<DeviceDescription> devices)
        {
            Check.CollectionIsNotNullOrEmpty(devices, "devices");
            Check.ObjectIsNotNull(DeviceAuditRequested);

            DeviceAuditRequested(devices);
        }

        public void OpenTypes(ICollection<AuditItemGroupType> types)
        {
            Check.CollectionIsNotNullOrEmpty(types, "types");
            Check.ObjectIsNotNull(TypedAuditRequested);

            TypedAuditRequested(types);
        }
    }
}