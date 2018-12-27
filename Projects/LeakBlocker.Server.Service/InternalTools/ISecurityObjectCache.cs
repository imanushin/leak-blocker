using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.SystemTools;
using LeakBlocker.Libraries.SystemTools.Entities;

namespace LeakBlocker.Server.Service.InternalTools
{
    /// <summary>
    /// Cached information about accessible domains. 
    /// </summary>
    public interface ISecurityObjectCache
    {
        /// <summary>
        /// Event that is triggered when objects are added or removed.
        /// </summary>
        event Action Updated;
        
        /// <summary>
        /// All data that is currently cached.
        /// </summary>
        IDomainSnapshot Data
        {
            get;
        }

        /// <summary>
        /// Start updating the cache in the separate thread.
        /// </summary>
        /// <param name="credentials">Credentials for authorization on the remote system.</param>
        /// <returns>Session identifier that can be passed to</returns>
        Guid RequestUpdate(Credentials credentials);

        /// <summary>
        /// Checks if the specified update request is completed.  
        /// </summary>
        /// <param name="session">Sesion identifier returned by RequestUpdate function.</param>
        /// <returns>Domain snapshot or null if request is still pending.</returns>
        IDomainSnapshot CheckUpdateRequestResult(Guid session);

        /// <summary>
        /// Returns domain account by name. Result is not cached, so server must be accessible.
        /// </summary>
        /// <param name="name">Domain or machine name.</param>
        /// <param name="options">Options for accessing the remote system.</param>
        /// <returns>Domain account.</returns>
        BaseDomainAccount GetBaseDomainAccountByNameImmediately(string name, SystemAccessOptions options = default(SystemAccessOptions));
    }
}
