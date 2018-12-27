using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Entities.Settings.TemporaryAccess;

namespace LeakBlocker.Agent.Core.Settings
{
    internal interface ITemporaryAccessConditionsChecker
    {
        bool IsMatched(DeviceDescription device, BaseComputerAccount computer, BaseUserAccount user, BaseTemporaryAccessCondition condition);

    }
}
