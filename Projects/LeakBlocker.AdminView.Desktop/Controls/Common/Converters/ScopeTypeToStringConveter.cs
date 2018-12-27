using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.AdminView.Desktop.Controls.Common.Converters
{
    internal sealed class ScopeTypeToStringConveter : AbstractConverter<ScopeType, string>
    {
        protected override string Convert(ScopeType baseValue)
        {
            return baseValue.GetValueDescription();
        }
    }
}
