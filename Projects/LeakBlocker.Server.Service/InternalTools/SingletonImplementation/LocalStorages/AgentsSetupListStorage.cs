using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Server.Service.InternalTools.LocalStorages;

namespace LeakBlocker.Server.Service.InternalTools.SingletonImplementation.LocalStorages
{
    internal sealed class AgentsSetupListStorage : BaseConfigurationManager<ReadOnlySet<BaseComputerAccount>>, IAgentsSetupListStorage
    {
        private const string fileName = "AgentsSetupListStorage.lbConfig";

        public AgentsSetupListStorage()
            : base(fileName)
        {
        }

        public override ReadOnlySet<BaseComputerAccount> Current
        {
            get
            {
                return base.Current ?? ReadOnlySet<BaseComputerAccount>.Empty;
            }
        }

        public void AddAndSave(BaseComputerAccount computer)
        {
            SaveIfDifferent(Current.UnionWith(computer).ToReadOnlySet());
        }
    }
}
