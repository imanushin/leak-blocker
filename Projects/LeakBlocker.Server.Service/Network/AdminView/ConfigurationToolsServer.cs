using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Entities.Settings;
using LeakBlocker.Libraries.Common.Network;
using LeakBlocker.Server.Service.Generated;
using LeakBlocker.Server.Service.InternalTools;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.Server.Service.Network.AdminView
{
    internal sealed class ConfigurationToolsServer : GeneratedConfigurationTools
    {
        public ConfigurationToolsServer()
            : base((ISecuritySessionManager)InternalObjects.AdminViewSecuritySessionManager)
        {
        }

        protected override SimpleConfiguration LastConfiguration()
        {
            return InternalObjects.ConfigurationStorage.CurrentOrDefault;
        }

        protected override void SaveConfiguration(SimpleConfiguration configuration)
        {
            Check.ObjectIsNotNull(configuration, "configuration");

            InternalObjects.ConfigurationStorage.Save(configuration);

            AccountSecurityIdentifier userIdentifier = InternalObjects.AdminViewSecuritySessionManager.CurrentUserIdentifier;

            ProgramConfiguration fullConfiguration = InternalObjects.ConfigurationStorage.CurrentFullConfiguration;

            BaseUserAccount user = InternalObjects.AccountResolver.ResolveUser(userIdentifier);

            InternalObjects.AuditItemHelper.ConfigurationChanged(user, fullConfiguration.ConfigurationVersion);

            InternalObjects.ScopeManager.ForceUpdateScope();
            InternalObjects.AgentStatusObserver.EnqueueObserving();
        }

        protected override bool HasConfiguration()
        {
            return InternalObjects.ConfigurationStorage.Current != null;
        }
    }
}
