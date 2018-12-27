using System;
using System.Collections.Concurrent;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Storage;
using LeakBlocker.Libraries.SystemTools;
using LeakBlocker.ServerShared.AdminViewCommunication;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Resources;
using LeakBlocker.ServerShared.AgentCommunication;

namespace LeakBlocker.Server.Service.InternalTools.SingletonImplementation
{
    internal sealed class AgentManager : IAgentManager
    {
        private readonly ConcurrentDictionary<BaseComputerAccount, object> syncRoots = new ConcurrentDictionary<BaseComputerAccount, object>();

        bool IAgentManager.IsComputerTurnedOn(BaseComputerAccount computer)
        {
            try
            {
                bool result = SystemObjects.Prerequisites.ComputerIsAccessible(computer.FullName);

                Log.Add("Is computer turned on({0}) = {1}", computer, result);

                return result;
            }
            catch (Exception ex)
            {
                Log.Write("Unable to check computer status {0}: {1}", computer, ex);

                InternalObjects.AuditItemHelper.ErrorInAgentCommunications(ex.GetExceptionMessage(), computer);

                return false;
            }
        }

        void IAgentManager.InstallAgent(BaseComputerAccount computer)
        {
            Log.Write("Agent installation started on computer {0}", computer);

            object computerSyncRoot = syncRoots.GetOrAdd(computer, c => new object());

            lock (computerSyncRoot)
            {
                IAuditItemHelper auditHelper = InternalObjects.AuditItemHelper;

                auditHelper.NotifyInstallationStarted(computer);

                try
                {
                    ManagedComputerData currentData = InternalObjects.AgentStatusStore.GetComputerData(computer);

                    InternalObjects.AgentStatusStore.SetComputerData(computer, currentData.CreateFromCurrent(ManagedComputerStatus.AgentInstalling));

                    Credentials credentials = StorageObjects.CredentialsManager.TryGetCredentials(computer);
                    Check.ObjectIsNotNull(credentials);
                    AgentInstallerStatus installResult = InternalObjects.AgentInstaller.Install(computer, credentials);

                    bool alreadyInstalled = (installResult == AgentInstallerStatus.AgentAlreadyInstalled) || (installResult == AgentInstallerStatus.SameVersionAlreadyInstalled);

                    if ((installResult != AgentInstallerStatus.Success) && !alreadyInstalled)
                        Exceptions.Throw(ErrorMessage.ThrowDirectly, AgentInstallerStrings.TemplateError.Combine(AgentInstallerStrings.ModuleNameAgent, installResult.GetValueDescription()));

                    currentData = InternalObjects.AgentStatusStore.GetComputerData(computer);
                    if (currentData.Status == ManagedComputerStatus.AgentInstalling)
                        InternalObjects.AgentStatusStore.SetComputerData(computer, currentData.CreateFromCurrent(ManagedComputerStatus.Unknown));

                    if (alreadyInstalled)
                        auditHelper.AgentInstallationWasNotRequired(computer);
                    else
                        auditHelper.AgentInstallationSucceded(computer);

                    Log.Write("Agent installation on computer {0} completed with result {1}.", computer, installResult);
                }
                catch (Exception ex)
                {
                    if (ex.GetType().Name == "ExpectationViolationException")//Это значит, что какие-то объекты не подменили
                        throw;

                    string message = ex.GetExceptionMessage();
                    using (new TimeMeasurement("Firewall check on computer {0}".Combine(computer)))
                    {
                        if (SystemObjects.Prerequisites.FirewallIsActive(computer.FullName))
                            message = AgentInstallerStrings.FirewallSuggestion.Combine(computer, message);
                    }
                    auditHelper.AgentInstallationFailed(computer, message);


                    InternalObjects.AgentStatusStore.SetComputerData(computer, new ManagedComputerData(ManagedComputerStatus.AgentInstallationFailed, DeviceAccessMap.Empty));

                    Log.Write("Agent installation on computer {0} finished with error: {1}.", computer, ex);
                }

                InternalObjects.AgentStatusStore.UpdateLastCommunicationTimeAsCurrent(computer);
            }
        }
    }
}
