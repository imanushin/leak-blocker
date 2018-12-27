using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Agent.Core.Implementations.AgentInstallerObjects
{
    internal sealed class SelfUninstallMode : BaseInstallerMode
    {
        protected override IEnumerable<InstallerArgument> Arguments
        {
            get
            {
                yield return InstallerArgument.SecretKey;
            }
        }

        protected override IEnumerable<InstallerCondition> Conditions
        {
            get
            {
                yield return InstallerCondition.SameVersionIsInstalled;
                yield return InstallerCondition.SecretKeyIsCorrect;
            }
        }

        protected override void PerformActions()
        {
            using (ILocalControlClient controlClient = AgentObjects.CreateLocalControlClient())
            {
                controlClient.RequestUninstall(AgentObjects.AgentPrivateStorage.SecretKey);
            }
        }
    }
}
