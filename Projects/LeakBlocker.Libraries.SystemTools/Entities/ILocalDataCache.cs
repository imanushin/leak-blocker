using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.SystemTools.Entities.Management;

namespace LeakBlocker.Libraries.SystemTools.Entities
{
    /// <summary>
    /// Cached information about objects on the current computer.
    /// </summary>
    public interface ILocalDataCache
    {
        /// <summary>
        /// Logged on users.
        /// </summary>
        ReadOnlySet<CachedUserData> Users
        {
            get;
        }

        /// <summary>
        /// Current computer.
        /// </summary>
        CachedComputerData Computer
        {
            get;
        }

        /// <summary>
        /// Active console user or null if there is no sessions.
        /// </summary>
        BaseUserAccount ConsoleUser
        {
            get;
        }

        /// <summary>
        /// Updates the information.
        /// </summary>
        void Update();
    }

    /// <summary>
    /// Information about user.
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    public sealed class CachedUserData : BaseReadOnlyObject
    {
        /// <summary>
        /// User account.
        /// </summary>
        [DataMember]
        public BaseUserAccount User
        {
            get;
            private set;
        }

        /// <summary>
        /// Groups to which the user belongs to.
        /// </summary>
        [DataMember]
        public ReadOnlySet<AccountSecurityIdentifier> Groups
        {
            get;
            private set;
        }

        /// <summary>
        /// True if the corresponding user is a built-in service account.
        /// </summary>
        [IgnoreDataMember]
        public bool ServiceAccount
        {
            get
            {
                return (User.Identifier.GetParentIdentifier() == null);
            }
        }

        internal CachedUserData(BaseUserAccount user, IReadOnlyCollection<AccountSecurityIdentifier> groups)
        {
            Check.ObjectIsNotNull(user, "user");
            Check.CollectionHasNoDefaultItems(groups, "groups");

            User = user;
            Groups = groups.ToReadOnlySet();
        }

        /// <summary>
        /// Returns all object that should be involved in hash code calculation and equalirty checks.
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return User;
        }
    }

    /// <summary>
    /// Information about computer.
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    public sealed class CachedComputerData : BaseReadOnlyObject
    {
        /// <summary>
        /// Computer account.
        /// </summary>
        [DataMember]
        public BaseComputerAccount Computer
        {
            get;
            private set;
        }

        /// <summary>
        /// Groups to which the computer belongs to.
        /// </summary>
        [DataMember]
        public ReadOnlySet<AccountSecurityIdentifier> Groups
        {
            get;
            private set;
        }

        internal CachedComputerData(BaseComputerAccount computer, IReadOnlyCollection<AccountSecurityIdentifier> groups)
        {
            Check.ObjectIsNotNull(computer, "computer");
            Check.CollectionHasNoDefaultItems(groups, "groups");

            Computer = computer;
            Groups = groups.ToReadOnlySet();
        }

        /// <summary>
        /// Returns all object that should be involved in hash code calculation and equalirty checks.
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return Computer;
        }
    }
}
