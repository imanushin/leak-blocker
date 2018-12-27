using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Audit;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Storage;
using LeakBlocker.Libraries.SystemTools;
using LeakBlocker.Libraries.SystemTools.Entities;

namespace LeakBlocker.Server.Service.InternalTools.SingletonImplementation
{
    internal sealed class AuditItemHelper : IAuditItemHelper
    {
        private static BaseComputerAccount ServerComputer
        {
            get
            {
                return SystemObjects.SystemAccountTools.LocalComputer;
            }
        }

        private static void AddItem(BaseComputerAccount computer, AuditItemType itemType, string textData = null, string additionalTextData = null, int configurationVersion = 0, BaseUserAccount user = null)
        {
            var item = new AuditItem(itemType, Time.Now, computer, user, textData, additionalTextData, null, configurationVersion);

            StorageObjects.AuditItemsManager.AddItem(item);
        }

        public void AgentUninstalled(BaseComputerAccount computer)
        {
            Check.ObjectIsNotNull(computer, "computer");

            AddItem(computer, AuditItemType.AgentUninstalled);
        }

        public void NotifyInstallationStarted(BaseComputerAccount computer)
        {
            Check.ObjectIsNotNull(computer, "computer");

            AddItem(computer, AuditItemType.AgentSetupStarted);
        }

        public void AgentIsNotResponding(BaseComputerAccount computer)
        {
            Check.ObjectIsNotNull(computer, "computer");

            AddItem(computer, AuditItemType.AgentIsNotResponding);
        }

        public void AgentInstallationSucceded(BaseComputerAccount computer)
        {
            Check.ObjectIsNotNull(computer, "computer");

            AddItem(computer, AuditItemType.AgentInstallationSucceeded);
        }

        public void AgentInstallationFailed(BaseComputerAccount computer, string errorText)
        {
            Check.ObjectIsNotNull(computer, "computer");
            Check.StringIsMeaningful(errorText, "errorText");

            AddItem(computer, AuditItemType.AgentInstallationFailed, errorText);
        }

        public void AgentInstallationWasNotRequired(BaseComputerAccount computer)
        {
            Check.ObjectIsNotNull(computer, "computer");

            AddItem(computer, AuditItemType.AgentInstallationWasNotRequired);
        }

        public void ConfigurationChanged(BaseUserAccount user, int configurationVersion)
        {
            Check.ObjectIsNotNull(user, "user");
            Check.IntegerIsGreaterThanZero(configurationVersion, "configurationVersion");

            AddItem(ServerComputer, AuditItemType.ConfigurationChanged, configurationVersion: configurationVersion, user: user);
        }

        public void DomainConnectionFailed(DomainAccount domain, string user, Exception error)
        {
            Check.ObjectIsNotNull(domain, "domain");
            Check.ObjectIsNotNull(user, "user");
            Check.ObjectIsNotNull(error, "error");

            ReadOnlyDictionary<string, GetString> map = new Dictionary<string, GetString>
            {
                {"User", ()=>user}
            }.ToReadOnlyDictionary();
            string resultText = StringNamedFormatter.ApplyTemplate(error.GetExceptionMessage(), map);

            AddItem(ServerComputer, AuditItemType.UnableToRefreshDomainData, resultText, domain.FullName);
        }

        public void DomainMemberIsNotAccessible(IDomainMember domainMember)
        {
            Check.ObjectIsNotNull(domainMember, "domainMember");

            AddItem(
                ServerComputer,
                AuditItemType.DomainMemberIsNotAccessible,
                domainMember.FullName,
                domainMember.Parent.FullName,
                InternalObjects.ConfigurationStorage.CurrentFullConfiguration.ConfigurationVersion);
        }

        public void ReportsDoesNotConfigured()
        {
            AddItem(ServerComputer, AuditItemType.ReportsAreNotConfigured);
        }

        public void SendingReportFailed(string shortMessage)
        {
            AddItem(ServerComputer, AuditItemType.SendingReportFailed, shortMessage);
        }

        public void ErrorInAgentCommunications(string errorMessage, BaseComputerAccount computer)
        {
            AddItem(computer, AuditItemType.ErrorInAgentCommunications, errorMessage);
        }

        public void ErrorInStatusUpdate(string errorMessage)
        {
            AddItem(ServerComputer, AuditItemType.ErrorInStatusUpdate, errorMessage);
        }

        public void MainServiceStarted()
        {
            AddItem(ServerComputer, AuditItemType.MainServiceStarted);
        }

        public void MainServiceStopped()
        {
            AddItem(ServerComputer, AuditItemType.MainServiceStopped);
        }
    }
}
