using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.Agent.Core.Settings
{
    internal sealed class AgentComputerState : BaseReadOnlyObject
    {
        public BaseComputerAccount TargetComputer
        {
            get;
            private set;
        }

        public ReadOnlySet<AccountSecurityIdentifier> ComputerGroups
        {
            get;
            private set;
        }

        /// <summary>
        /// Map из пользователей на группы
        /// </summary>
        public ReadOnlyDictionary<BaseUserAccount, ReadOnlySet<AccountSecurityIdentifier>> LoggedOnUsers
        {
            get;
            private set;
        }

        public ReadOnlySet<DeviceDescription> ConnectedDevices
        {
            get;
            private set;
        }

        public AgentComputerState(
            BaseComputerAccount targetComputer,
            IReadOnlyCollection<AccountSecurityIdentifier> computerGroups,
            ReadOnlyDictionary<BaseUserAccount, ReadOnlySet<AccountSecurityIdentifier>> loggedOnUsers,
            IReadOnlyCollection<DeviceDescription> connectedDevices)
        {
            Check.ObjectIsNotNull(targetComputer, "targetComputer");
            Check.CollectionHasNoDefaultItems(computerGroups, "computerGroups");
            Check.CollectionHasNoDefaultItems(loggedOnUsers, "loggedOnUsers");
            Check.CollectionHasNoDefaultItems(connectedDevices, "connectedDevices");

            TargetComputer = targetComputer;
            ComputerGroups = computerGroups.ToReadOnlySet();
            LoggedOnUsers = loggedOnUsers.ToReadOnlyDictionary();
            ConnectedDevices = connectedDevices.ToReadOnlySet();
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return TargetComputer;
            yield return ComputerGroups;
            yield return LoggedOnUsers;
            yield return ConnectedDevices;
        }
    }
}
