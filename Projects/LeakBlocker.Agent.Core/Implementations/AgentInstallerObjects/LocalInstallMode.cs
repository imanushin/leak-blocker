using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Agent.Core.Implementations.AgentInstallerObjects
{
    internal sealed class LocalInstallMode : BaseInstallerMode
    {
        protected override IEnumerable<InstallerArgument> Arguments
        {
            get 
            {
                yield return InstallerArgument.Password;
            }
        }

        protected override IEnumerable<InstallerCondition> Conditions
        {
            get 
            {
                yield return InstallerCondition.SameVersionIsNotInstalled;
                yield return InstallerCondition.PasswordIsCorrect;
            }
        }

        protected override void PerformActions()
        {
            TransactionalAction.RunSequence(new List<TransactionalAction>
            {            
                new TransactionalAction(CopyFiles, DeleteFiles),
                new TransactionalAction(InstallService, UninstallService),  
                new TransactionalAction(SaveLocalPrivateData, DeletePrivateData),
                              
                new TransactionalAction(StartService, null),
            
                new TransactionalAction(UninstallPreviousVersion, null),

                new TransactionalAction(SaveVersionIndependentData, null, true),                
                new TransactionalAction(ProductRegistrator.RegisterProduct, null, true)
            });
        }
    }
}
