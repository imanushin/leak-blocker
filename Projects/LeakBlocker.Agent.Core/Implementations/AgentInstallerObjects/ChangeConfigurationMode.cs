using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Settings;
using LeakBlocker.Libraries.SystemTools;

namespace LeakBlocker.Agent.Core.Implementations.AgentInstallerObjects
{
    internal sealed class ChangeConfigurationMode : BaseInstallerMode
    {
        protected override IEnumerable<InstallerArgument> Arguments
        {
            get
            {
                yield return InstallerArgument.ConfigurationFile;
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
            string filePath = GetArgument(InstallerArgument.ConfigurationFile);

            Log.Write("Loading configuration. Path is {0}.".Combine(filePath));

            SystemObjects.FileTools.CopyFile(filePath, AgentObjects.AgentConstants.AgentConfigurationOverrideFile);

            string secretKey = AgentObjects.VersionIndependentPrivateStorage.SecretKey;

            IGlobalFlag globalFlag = SystemObjects.CreateGlobalFlag(AgentObjects.AgentConstants.ConfigurationAllowedGlobalFlag);

            if (!PeriodicCheck.WaitUntilSuccess(() => globalFlag.Exists, TimeSpan.FromSeconds(10), TimeSpan.FromMinutes(5)))
                Exceptions.Throw(ErrorMessage.ServiceCannotAcceptControls, "Service is in incorrect state.");

            using (ILocalControlClient controlClient = AgentObjects.CreateLocalControlClient())
            {
                controlClient.SetConfiguration(secretKey);
            }
        }
    }
}
