using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LeakBlocker.AdminView.Desktop.Common;
using LeakBlocker.AdminView.Desktop.Generated;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities;

namespace LeakBlocker.AdminView.Desktop.Controls.Common.Converters
{
    internal sealed class DeviceToImageConverter : AbstractConverter<DeviceDescription,FrameworkElement>
    {
        public static FrameworkElement GetTemplate(object input)
        {
            return new ObjectTypeDevice();
        }

        protected override FrameworkElement Convert(DeviceDescription baseValue)
        {
            return GetTemplate(baseValue);
        }
    }
}
