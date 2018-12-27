using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Agent.Core.Implementations.AgentInstallerObjects;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.SystemTools;
using LeakBlocker.ServerShared.AgentCommunication;

namespace LeakBlocker.Agent.Core.Implementations
{
    internal sealed class AgentInstaller : IAgentInstaller
    {        
        private const string installLocalCommandLineMode = "-il"; // -pwd <password>
        private const string installRemoteCommandLineMode = "-ir"; // -pwh <hash> -srv <address> -key <keyrequest>
        private const string updateRemoteCommandLineMode = "-iu"; // -srv <address> -ver <verification>
        private const string uninstallLocalCommandLineMode = "-ul"; // -pwd <password>
        private const string uninstallSelfCommandLineMode = "-us"; // -enc <secretkey>
        private const string changeServerAddressCommandLineMode = "-cs"; // -pwd <password> -srv <address>
        private const string changeConfigurationCommandLineMode = "-cc"; // -pwd <password> -cfg <file>
        private const string userUninstallCommandLineMode = "-uu"; 

        private static readonly ReadOnlyDictionary<string, BaseInstallerMode> installerModes = new Dictionary<string, BaseInstallerMode>
        {
            { installLocalCommandLineMode, new LocalInstallMode() },
            { installRemoteCommandLineMode, new RemoteInstallMode() },
            { updateRemoteCommandLineMode, new RemoteUpdateMode() },
            { uninstallLocalCommandLineMode, new LocalUninstallMode() },
            { uninstallSelfCommandLineMode, new SelfUninstallMode() },
            { changeServerAddressCommandLineMode, new ChangeServerAddressMode() },
            { changeConfigurationCommandLineMode, new ChangeConfigurationMode() },
            { userUninstallCommandLineMode, new UserUninstallMode() },
        }.ToReadOnlyDictionary();
        
        int IAgentInstaller.Start()
        {
            using (new TimeMeasurement("Agent installer"))
            {
                try
                {
                    foreach (KeyValuePair<string, BaseInstallerMode> currentItem in installerModes.Where(currentItem => SharedObjects.CommandLine.Contains(currentItem.Key)))
                    {
                        currentItem.Value.Start();
                        return (int)AgentInstallerStatus.Success;
                    }
                }
                catch (Exception exception)
                {
                    Log.Write(exception);

                    int? error = ParseException(exception);
                    if (error.HasValue)
                    {
                        if(error.Value != 0)
                            Log.Write("Installer failed with error {0}.".Combine((AgentInstallerStatus)error.Value));
                        return error.Value;
                    }
                }

                Log.Write("Unknown mode or error.");
                return (int)AgentInstallerStatus.InternalFailure;
            }
        }

        private static int? ParseException(Exception exception)
        {
            var aggregateException = exception as AggregateException;
            if (aggregateException != null)
            {
                IEnumerable<int> values = aggregateException.InnerExceptions.Select(ParseException).Where(item => item.HasValue).Select(item => item.Value);

                if (values.Any())
                    return values.First();
                return null;
            }

            IDictionaryEnumerator enumerator = exception.Data.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Value is AgentInstallerStatus)
                    return (int)((AgentInstallerStatus)enumerator.Value);
            }
            return null;
        }

        bool IAgentInstaller.WaitForInstaller(TimeSpan timeout)
        {
            using (new TimeMeasurement("Waiting for installer"))
            {
                return PeriodicCheck.WaitUntilSuccess(() => AgentObjects.VersionIndependentPrivateStorage.Version ==
                    SharedObjects.Constants.VersionString, TimeSpan.FromSeconds(1), timeout);
            }
        }

        void IAgentInstaller.UninstallSelf()
        {
            using (new TimeMeasurement("Self uninstalling"))
            {
                new ServiceUninstallMode().Start();
            }
        }
    }
}
