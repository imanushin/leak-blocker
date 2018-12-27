using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Settings.TemporaryAccess;

namespace LeakBlocker.Libraries.Storage.Entities
{
    partial class DbUserTemporaryAccessCondition
    {
        private UserTemporaryAccessCondition ForceGetUserTemporaryAccessCondition()
        {
            return new UserTemporaryAccessCondition(User.GetBaseUserAccount(), EndTime, ReadOnlyAccess);
        }
    }
}
