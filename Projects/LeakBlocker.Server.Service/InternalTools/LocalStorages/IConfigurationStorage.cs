using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Settings;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.Server.Service.InternalTools.LocalStorages
{
    internal interface IConfigurationStorage : IBaseConfigurationManager<SimpleConfiguration>
    {
        ProgramConfiguration CurrentFullConfiguration
        {
            get;
        }

        SimpleConfiguration CurrentOrDefault
        {
            get;
        }
    }
}
