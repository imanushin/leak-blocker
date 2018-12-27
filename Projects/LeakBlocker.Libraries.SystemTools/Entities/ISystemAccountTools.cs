using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.Libraries.SystemTools.Entities
{
    /// <summary>
    /// Provides advanced account information for services. This class can be used only by LocalSystem
    /// </summary>
    public interface ISystemAccountTools
    {
        /// <summary>
        /// Returns domain account by name. Result is not cached, so server must be accessible.
        /// </summary>
        /// <param name="name">Domain or machine name.</param>
        /// <param name="options">Options for accessing the target system.</param>
        /// <returns>Domain account.</returns>
        BaseDomainAccount GetBaseDomainAccountByName(string name, SystemAccessOptions options = default(SystemAccessOptions));
        
        /// <summary>
        /// Gets user by name.
        /// </summary>
        /// <param name="identifier">Security identifier.</param>
        /// <returns>User.</returns>
        BaseUserAccount GetUserByIdentifier(AccountSecurityIdentifier identifier);

        /// <summary>
        /// Current commputer.
        /// </summary>
        BaseComputerAccount LocalComputer
        {
            get;
        }

        /// <summary>
        /// Запрашивает домен и выдает информацию о пользователе: как зовут, какой email, в какой компании работает и пр.
        /// </summary>
        UserContactInformation GetUserContactInformation(DomainUserAccount user, Credentials credentials);
    }
}
