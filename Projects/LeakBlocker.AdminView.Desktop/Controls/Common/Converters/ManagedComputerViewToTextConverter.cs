using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.AdminView.Desktop.Controls.Standard.Views;
using LeakBlocker.AdminView.Desktop.Resources;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Resources;

namespace LeakBlocker.AdminView.Desktop.Controls.Common.Converters
{
    internal sealed class ManagedComputerViewToTextConverter : AbstractConverter<ManagedComputerView, string>
    {
        public ManagedComputerViewToTextConverter()
            :base(true)
        {
        }

        protected override string Convert(ManagedComputerView computer)
        {
            if (computer == null)
                return AdminViewResources.SelectSingleComputerFirst;
            
            return AdminViewResources.ManagedComputerTextTemplate.Combine(computer.Computer.DnsName, computer.Computer.Data.Status.GetValueDescription());
        }
    }
}
