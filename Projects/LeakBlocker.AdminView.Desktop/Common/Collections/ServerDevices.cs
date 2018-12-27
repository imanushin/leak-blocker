using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.AdminView.Desktop.Common.Collections
{
    internal sealed class ServerDevices : SortedObservableCollection<DeviceDescription>
    {
        protected override void Update(Dispatcher dispatcher)
        {
            using (IDeviceTools client = UiObjects.CreateDeviceToolsClient())
            {
                ReadOnlySet<DeviceDescription> items = client.GetConnectedDevices().ToReadOnlySet();
                
                dispatcher.BeginInvoke(new Action(() => AddItems(items)));
            }
        }
    }
}
