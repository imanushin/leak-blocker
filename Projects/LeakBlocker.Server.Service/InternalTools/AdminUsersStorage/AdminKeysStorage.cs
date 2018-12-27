using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Server.Service.InternalTools.SingletonImplementation;
using LeakBlocker.Server.Service.InternalTools.SingletonImplementation.LocalStorages;

namespace LeakBlocker.Server.Service.InternalTools.AdminUsersStorage
{
    internal sealed class AdminKeysStorage : BaseConfigurationManager<AdminUsers>, IAdminKeysStorage
    {
        private const string fileName = "AdminKeysStorage.lbKeysStorage";

        public AdminKeysStorage()
            : base(fileName)
        {
        }

        [NotNull]
        public override AdminUsers Current
        {
            get
            {
                return base.Current ?? AdminUsers.Empty;
            }
        }
    }
}
