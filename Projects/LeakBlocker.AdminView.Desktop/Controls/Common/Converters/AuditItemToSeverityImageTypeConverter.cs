using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LeakBlocker.AdminView.Desktop.Generated;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Audit;

namespace LeakBlocker.AdminView.Desktop.Controls.Common.Converters
{
    internal sealed class AuditItemToSeverityImageTypeConverter : AbstractConverter<AuditItem,FrameworkElement>
    {
        protected override FrameworkElement Convert(AuditItem baseValue)
        {
            return GetImageInstanse(baseValue);
        }

        public static FrameworkElement GetImageInstanse(AuditItem baseValue)
        {
            switch (LinkedEnumHelper<AuditItemType, AuditItemSeverityType>.GetLinkedEnum(baseValue.EventType))
            {
                case AuditItemSeverityType.Information:
                    return new AuditSeverityInformation();
                case AuditItemSeverityType.Warning:
                    return new AuditSeverityWarning();
                case AuditItemSeverityType.Error:
                    return new AuditSeverityError();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
