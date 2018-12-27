using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Agent.Core.Implementations.AgentInstallerObjects
{
    internal sealed class RemoteUpdateMode : BaseInstallerMode
    {
        protected override IEnumerable<InstallerArgument> Arguments
        {
            get
            {
                yield return InstallerArgument.ServerAddress;
                yield return InstallerArgument.VerificationKey;
            }
        }

        protected override IEnumerable<InstallerCondition> Conditions
        {
            get
            {
                yield return InstallerCondition.AnyVersionWasInstalled;
                yield return InstallerCondition.VerificationDataIsCorrect;
            }
        }

        protected override void PerformActions()
        {
            if ((AgentObjects.VersionIndependentPrivateStorage.InstalledVersionsCounter != 0) && 
                AgentObjects.VersionIndependentPrivateStorage.Version.Equals(SharedObjects.Constants.VersionString, StringComparison.Ordinal))
            {
                UpdatePrivateData(true);
            }
            else
            {
                TransactionalAction.RunSequence(new List<TransactionalAction>
                {            
                    new TransactionalAction(CopyFiles, DeleteFiles),
                    new TransactionalAction(InstallService, UninstallService),  
                    new TransactionalAction(() => UpdatePrivateData(), DeletePrivateData),
                              
                    new TransactionalAction(StartService, null),
            
                    new TransactionalAction(UninstallPreviousVersion, null),

                    new TransactionalAction(SaveVersionIndependentData, null, true),
                });
            }
        }
    }
}
