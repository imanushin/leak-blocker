using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeakBlocker.Libraries.Security.Accounts;

namespace LeakBlocker.ServerShared.AdminViewCommunication
{
    /// <summary>
    /// Отвечает за обработку паролей в доменах
    /// </summary>
    public interface ICredentialsManager
    {
        bool HasCredentials(BaseDomainAccount account);

        string SetCredentials(BaseDomainAccount account);
    }
}
