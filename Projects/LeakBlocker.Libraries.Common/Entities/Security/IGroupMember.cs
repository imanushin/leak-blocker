using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common.Entities.Security
{
    /// <summary>
    /// Object that can be a member of a group.
    /// </summary>
    public interface IGroupMember
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
        /// Security identifier that uniquely identifies the object.
        /// </summary>
        AccountSecurityIdentifier Identifier
        {
            get;
        }
    }
}

