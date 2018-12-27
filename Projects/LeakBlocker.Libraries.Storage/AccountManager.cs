using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Storage.Entities;
using LeakBlocker.Libraries.Storage.InternalTools;

namespace LeakBlocker.Libraries.Storage
{
    internal sealed class AccountManager : IAccountManager
    {
        public ReadOnlySet<BaseComputerAccount> GetSavedComputers()
        {
            using (IDatabaseModel model = StorageObjects.CreateDatabaseModel())
            {
                return model.AccountSet.OfType<DbBaseComputerAccount>().ToList().Select(item=>item.ForceGetBaseComputerAccount()).ToReadOnlySet();
            }
        }

        public ReadOnlySet<BaseUserAccount> GetSavedUsers()
        {
            using (IDatabaseModel model = StorageObjects.CreateDatabaseModel())
            {
                return model.AccountSet.OfType<DbBaseUserAccount>().ToList().Select(item => item.ForceGetBaseUserAccount()).ToReadOnlySet();
            }
        }
    }
}
