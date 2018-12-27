using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Server.Service.InternalTools.SingletonImplementation.LocalStorages;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.Server.Service.InternalTools.SingletonImplementation
{
    internal sealed class AgentSetupPasswordManager : BaseConfigurationManager<AgentSetupPassword>, IAgentSetupPasswordManager
    {
        private static readonly string file = SharedObjects.Constants.UserDataFolder + "\\AgentManagement.dat"; 

        internal AgentSetupPasswordManager()
            : base(file)
        {
            if (base.Current == null)
            {
                var value = AgentSetupPassword.Generate();
                base.Save(value);
            }
        }

        public override void Save(AgentSetupPassword obj)
        {
            throw new InvalidOperationException("Cannot modify setup password.");
        }

        public override AgentSetupPassword Current
        {
            get
            {
                var result = base.Current;

                Check.ObjectIsNotNull(result);

                return result;
            }
        }
    }
}
