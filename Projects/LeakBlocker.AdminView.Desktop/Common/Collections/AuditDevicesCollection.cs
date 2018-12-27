using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.AdminView.Desktop.Common.Collections
{
    internal sealed class AuditDevicesCollection : SortedObservableCollection<DeviceDescription>
    {
        protected override void Update(Dispatcher dispatcher)
        {
            using (IAuditTools client = UiObjects.CreateAuditToolsClient())
            {
                List<DeviceDescription> devices = client.GetAuditDevices().Select(device => device).ToList();

                dispatcher.BeginInvoke(new Action(() => AddItems(devices)));
            }
        }
    }
}
