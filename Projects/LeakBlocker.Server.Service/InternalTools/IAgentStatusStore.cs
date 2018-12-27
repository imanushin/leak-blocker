using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.Server.Service.InternalTools
{
    internal interface IAgentStatusStore
    {
        void SetComputerData(BaseComputerAccount computer, ManagedComputerData data);

        ManagedComputerData GetComputerData(BaseComputerAccount computer);

        ReadOnlyDictionary<BaseComputerAccount, ManagedComputerData> GetManagedScope();

        Time GetLastCommunicationTimeUtc(BaseComputerAccount computer);

        void UpdateLastCommunicationTimeAsCurrent(BaseComputerAccount computer);

        void UpdateFromScope();

        bool IsUnderLicense(BaseComputerAccount computer);

        void ResetLastCommunicationTime(BaseComputerAccount computer);
    }
}
