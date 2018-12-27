using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using LeakBlocker.AdminView.Desktop.Generated;
using LeakBlocker.Libraries.Common.Entities.Audit;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.AdminView.Desktop.Controls.Common.Converters
{
    internal sealed class AuditItemTypeToImageTemplateConverter : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null)
                return null;

            var baseValue = (AuditItemType) item;

            AuditItemSeverityType severityType = LinkedEnumHelper<AuditItemType, AuditItemSeverityType>.GetLinkedEnum(baseValue);

            switch (severityType)
            {
                case AuditItemSeverityType.Information:
                    return AuditSeverityInformation.ImageTemplate;

                case AuditItemSeverityType.Warning:
                    return AuditSeverityWarning.ImageTemplate;

                case AuditItemSeverityType.Error:
                    return AuditSeverityError.ImageTemplate;

                default:
                    throw new InvalidOperationException("Unable to find severity {0}".Combine(severityType));
            }
        }
    }
}
