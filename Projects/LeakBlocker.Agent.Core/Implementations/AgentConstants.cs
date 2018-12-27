using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Resources;

namespace LeakBlocker.Agent.Core.Implementations
{
    internal sealed class AgentConstants : IAgentConstants
    {
        private const string serviceCommandLineMode = "-s";
        private const string installerCommandLineMode = "-i";
        private const string standaloneServerAddress = "localhost:0";
        private static readonly string serviceName = "LeakBlockerAgent_" + SharedObjects.Constants.VersionString;
        private static readonly string serviceDisplayedName = string.Format(CultureInfo.InvariantCulture, AgentServiceStrings.AgentServiceDisplayName, SharedObjects.Constants.Version.ToString(3));
        private static readonly string serviceDescription = AgentServiceStrings.AgentServiceDescription;
        private static readonly string serviceModuleFolder = SharedObjects.Constants.CurrentVersionProgramFilesFolder;
        private static readonly string serviceModulePath = serviceModuleFolder + "LeakBlocker.Agent.exe";
        private static readonly TimeSpan installerTimeout = TimeSpan.FromMinutes(1);
        private static readonly string databaseFile = serviceModuleFolder + "LeakBlocker.Agent.Database";
        private static readonly string configurationFile = serviceModuleFolder + "LeakBlocker.Agent.Configuration";
        private static readonly string agentConfigurationOverrideFile = serviceModuleFolder + "AgentConfiguration.xml";
#if DEBUG
        private static readonly TimeSpan networkTaskInterval = TimeSpan.FromMinutes(1);
#else
        private static readonly TimeSpan networkTaskInterval = TimeSpan.FromMinutes(15);
#endif
        private static readonly string unexpectedTerminationFlagFile = SharedObjects.Constants.MainModuleFolder + "d23cbu0wZ6Ec5bITUq5O" + SharedObjects.Constants.VersionString;
        private static readonly string configurationAllowedGlobalFlag = "LeakBlockerAgentConfigurationFlag_" + SharedObjects.Constants.VersionString;

        string IAgentConstants.ConfigurationAllowedGlobalFlag
        {
            get
            {
                return configurationAllowedGlobalFlag;
            }
        }

        string IAgentConstants.UnexpectedTerminationFlagFile
        {
            get
            {
                return unexpectedTerminationFlagFile;
            }
        }

        string IAgentConstants.ServiceCommandLineMode
        {
            get
            {
                return serviceCommandLineMode;
            }
        }

        string IAgentConstants.InstallerCommandLineMode
        {
            get
            {
                return installerCommandLineMode;
            }
        }

        string IAgentConstants.StandaloneServerAddress
        {
            get
            {
                return standaloneServerAddress;
            }
        }

        TimeSpan IAgentConstants.NetworkTaskInterval
        {
            get
            {
                return networkTaskInterval;
            }
        }

        string IAgentConstants.ServiceName
        {
            get
            {
                return serviceName;
            }
        }

        string IAgentConstants.ServiceDisplayedName
        {
            get
            {
                return serviceDisplayedName;
            }
        }

        string IAgentConstants.ServiceDescription
        {
            get
            {
                return serviceDescription;
            }
        }

        string IAgentConstants.ServiceModuleFolder
        {
            get
            {
                return serviceModuleFolder;
            }
        }

        string IAgentConstants.ServiceModulePath
        {
            get
            {
                return serviceModulePath;
            }
        }

        TimeSpan IAgentConstants.InstallerTimeout
        {
            get
            {
                return installerTimeout;
            }
        }

        string IAgentConstants.DatabaseFile
        {
            get
            {
                return databaseFile;
            }
        }

        string IAgentConstants.AgentDataFile
        {
            get
            {
                return configurationFile;
            }
        }

        string IAgentConstants.AgentConfigurationOverrideFile
        {
            get
            {
                return agentConfigurationOverrideFile;
            }
        }
    }
}
