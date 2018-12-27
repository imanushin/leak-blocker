using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LeakBlocker.AdminView.Desktop.Controls.Standard.Views;
using LeakBlocker.AdminView.Desktop.Generated;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.AdminView.Desktop.Controls.Common.Converters
{
    internal sealed class AttachedDeviceStateToImageConveter : AbstractConverter<DeviceDescriptionState, FrameworkElement>
    {
        protected override FrameworkElement Convert(DeviceDescriptionState baseValue)
        {
            switch (baseValue)
            {
                case DeviceDescriptionState.Allowed:
                    return new DeviceStatusAllowed();

                case DeviceDescriptionState.Blocked:
                    return new DeviceStatusBlocked();

                case DeviceDescriptionState.PartiallyBlocked:
                    return new DeviceStatusPartiallyBlocked();

                case DeviceDescriptionState.TemporaryAllowed:
                    return new DeviceStatusTemporarilyAllowed();

                default:
                    throw new InvalidOperationException("Unable to find picrute for item {0}".Combine(baseValue));
            }
        }
    }
}
