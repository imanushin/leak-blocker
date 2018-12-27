﻿using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.AdminView.Desktop.Controls.Common.Converters
{
    internal sealed class ManagedComputerStatusToStringConverter : AbstractConverter<ManagedComputerStatus, string>
    {
        protected override string Convert(ManagedComputerStatus baseValue)
        {
            return baseValue.GetValueDescription();
        }
    }
}
