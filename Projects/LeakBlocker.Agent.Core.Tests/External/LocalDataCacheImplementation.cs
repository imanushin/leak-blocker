using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.SystemTools.Entities;
using LeakBlocker.Libraries.SystemTools.Tests.Entities;

namespace LeakBlocker.Agent.Core.Tests.External
{
    class LocalDataCacheImplementation : ILocalDataCache
    {
        public ReadOnlySet<CachedUserData> Users
        {
            get;
            private set;
        }

        public CachedComputerData Computer
        {
            get;
            private set;
        }

        public BaseUserAccount ConsoleUser
        {
            get;
            private set;
        }

        public void Update()
        {
            Users = CachedUserDataTest.objects.ToReadOnlySet();
            ConsoleUser = Users.First().User;
        }

        public LocalDataCacheImplementation()
        {
            Users = ReadOnlySet<CachedUserData>.Empty;
            Computer = CachedComputerDataTest.First;
        }

    }
}
