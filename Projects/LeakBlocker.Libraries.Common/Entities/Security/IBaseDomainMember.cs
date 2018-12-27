
using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common.Entities.Security
{
    /// <summary>
    /// Base domain member.
    /// </summary>
    public interface IBaseDomainMember
    {
        /// <summary>
        /// Object name.
        /// </summary>
        string FullName
        {
            get;
        }

        /// <summary>
        /// Object canonical name. At each moment there cannot be two objects with same canonical names. However canonical name changes after renaming object.
        /// </summary>
        string CanonicalName
        {
            get;
        }

        /// <summary>
        /// Domain.
        /// </summary>
        BaseDomainAccount Parent
        {
            get;
        }
    }
}
