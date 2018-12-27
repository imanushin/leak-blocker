using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.SystemTools;

namespace LeakBlocker.Agent.Core.Implementations.AgentInstallerObjects
{
    internal sealed class ServiceUninstallMode : BaseInstallerMode
    {
        protected override IEnumerable<InstallerArgument> Arguments
        {
            get
            {
                return ReadOnlySet<InstallerArgument>.Empty;
            }
        }

        protected override IEnumerable<InstallerCondition> Conditions
        {
            get
            {
                return ReadOnlySet<InstallerCondition>.Empty;
            }
        }

        protected override void PerformActions()
        {
            AgentObjects.VersionIndependentPrivateStorage.DecrementCounter();

            TransactionalAction.RunSequence(new List<TransactionalAction>
            {            
                new TransactionalAction(SystemObjects.FileTools.RemoveCurrentExecutable, null, true),
                new TransactionalAction(DeletePrivateData, null, true), 
                new TransactionalAction(ProductRegistrator.UnregisterProduct, null, true)
            });
        }
    }
}
