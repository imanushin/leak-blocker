using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Agent.Core
{
    internal interface IAgentConstants
    {
        string ServiceCommandLineMode
        {
            get;
        }

        string InstallerCommandLineMode
        {
            get;
        }

        string StandaloneServerAddress
        {
            get;
        }

        TimeSpan NetworkTaskInterval
        {
            get;
        }

        string ServiceName
        {
            get;
        }

        string ServiceDisplayedName
        {
            get;
        }

        string ServiceDescription
        {
            get;
        }

        string ServiceModuleFolder
        {
            get;
        }

        string ServiceModulePath
        {
            get;
        }

        TimeSpan InstallerTimeout
        {
            get;
        }

        string DatabaseFile
        {
            get;
        }

        string AgentDataFile
        {
            get;
        }

        string AgentConfigurationOverrideFile
        {
            get;
        }

        string UnexpectedTerminationFlagFile
        {
            get;
        }

        string ConfigurationAllowedGlobalFlag
        {
            get;
        }
    }
}
