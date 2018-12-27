using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.Server.Service.InternalTools.LocalStorages
{
    internal interface IAgentsSetupListStorage : IBaseConfigurationManager<ReadOnlySet<BaseComputerAccount>>
    {
        [NotNull]
        new ReadOnlySet<BaseComputerAccount> Current
        {
            get;
        }

        void AddAndSave(BaseComputerAccount computer);
    }
}
