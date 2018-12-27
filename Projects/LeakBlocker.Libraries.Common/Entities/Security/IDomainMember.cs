
using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common.Entities.Security
{
    /// <summary>
    /// Active directory domain member.
    /// </summary>
    public interface IDomainMember : IBaseDomainMember
    {
        /// <summary>
        /// Domain.
        /// </summary>
        new DomainAccount Parent
        {
            get;
        }
    }
}
