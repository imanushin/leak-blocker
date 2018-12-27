using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Agent.Core.Implementations.AgentInstallerObjects
{
    internal sealed class RemoteInstallMode : BaseInstallerMode
    {
        protected override IEnumerable<InstallerArgument> Arguments
        {
            get
            {
                yield return InstallerArgument.PasswordHash;
                yield return InstallerArgument.ServerAddress;
                yield return InstallerArgument.KeyRequest;
            }
        }

        protected override IEnumerable<InstallerCondition> Conditions
        {
            get
            {
                yield return InstallerCondition.AnyVersionIsNotInstalled;
            }
        }

        protected override void PerformActions()
        {
            TransactionalAction.RunSequence(new List<TransactionalAction>
            {            
                new TransactionalAction(CopyFiles, DeleteFiles),
                new TransactionalAction(InstallService, UninstallService),  
                new TransactionalAction(SaveRemotePrivateData, DeletePrivateData),
                              
                new TransactionalAction(StartService, null),
            
                new TransactionalAction(SaveVersionIndependentData, null, true),
                new TransactionalAction(ProductRegistrator.RegisterProduct, null, true)
            });
        }
    }
}
