using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Server.Service.InternalTools.LocalStorages;

namespace LeakBlocker.Server.Service.InternalTools.AdminUsersStorage
{
    internal interface IAdminKeysStorage : IBaseConfigurationManager<AdminUsers>
    {
        [NotNull]
        new AdminUsers Current
        {
            get;
        }
    }
}
