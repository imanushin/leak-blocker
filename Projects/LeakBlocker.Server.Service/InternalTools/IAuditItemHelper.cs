using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.SystemTools.Entities;

namespace LeakBlocker.Server.Service.InternalTools
{
    internal interface IAuditItemHelper
    {
        /// <summary>
        /// Начата установка на агента
        /// </summary>
        void NotifyInstallationStarted(BaseComputerAccount computer);

        /// <summary>
        /// Агент не отвечает: запланирована повторная установка
        /// </summary>
        void AgentIsNotResponding(BaseComputerAccount computer);

        /// <summary>
        /// Установка агента успешна завершена
        /// </summary>
        void AgentInstallationSucceded(BaseComputerAccount computer);

        /// <summary>
        /// Установка агента провалилась (текст в <paramref name="errorText"/>)
        /// </summary>
        void AgentInstallationFailed(BaseComputerAccount computer, string errorText);

        /// <summary>
        /// Установка агента провалилась
        /// </summary>
        void AgentInstallationWasNotRequired(BaseComputerAccount computer);

        /// <summary>
        /// Конфигурация изменена администратором
        /// </summary>
        void ConfigurationChanged(BaseUserAccount user, int configurationVersion);

        void DomainConnectionFailed(DomainAccount domain, string user, Exception error);

        void DomainMemberIsNotAccessible(IDomainMember domainMember);

        void ReportsDoesNotConfigured();

        void SendingReportFailed(string shortMessage);

        void ErrorInAgentCommunications(string errorMessage, BaseComputerAccount computer);

        void ErrorInStatusUpdate(string errorMessage);

        void AgentUninstalled(BaseComputerAccount computer);

        void MainServiceStarted();

        void MainServiceStopped();
    }
}
