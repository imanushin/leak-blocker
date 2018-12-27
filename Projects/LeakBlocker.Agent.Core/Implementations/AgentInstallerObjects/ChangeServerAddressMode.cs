using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Agent.Core.Implementations.AgentInstallerObjects
{
    internal sealed class ChangeServerAddressMode : BaseInstallerMode
    {
        protected override IEnumerable<InstallerArgument> Arguments
        {
            get
            {
                yield return InstallerArgument.ServerAddress;
                yield return InstallerArgument.Password;
            }
        }

        protected override IEnumerable<InstallerCondition> Conditions
        {
            get
            {
                yield return InstallerCondition.SameVersionIsInstalled;
                yield return InstallerCondition.PasswordIsCorrect;
            }
        }

        protected override void PerformActions()
        {
            UpdatePrivateData(true);
        }
    }
}
