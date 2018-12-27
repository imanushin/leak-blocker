using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Resources;

namespace LeakBlocker.Libraries.Common.Entities.Audit
{
    internal sealed class ReportCreator : IReportCreator
    {
        private const string emailTemplate = "<html><body>{0}</body></html>";
        private const string htmlTemplate = "<html><title>{0}</title><body>{1}</body></html>";
        private const string topTemplate = "<table border=\"1\"><tbody><tr>{0}</tr>{1}</tbody></table>";
        private const string openRow = "<tr>";
        private const string closeRow = "</tr>";
        private const string cellTemplate = "<td>{0}</td>";
        private const string auditBodyTemplate = "{0}<br>{1}";
        private const string auditFilterDescriptionTemplate =
@"Audit filter {0}<br>
Computers: {1}<br>
Users: {2}<br>
Devices: {3}<br>
Time restrictions: {4}<br>
OnlyError: {5}<br>
Types filter: {6}<br>";


        public string CreateReportHtml(IReadOnlyCollection<AuditItem> auditItems, Time startTime, Time endTime)
        {
            string body;

            if (auditItems.Any())
            {
                string innerTable = CreateReportTable(auditItems);

                body = String.Format(CultureInfo.CurrentCulture, ReportStrings.EventsBetween, startTime.ToUtcDateTime(), endTime.ToUtcDateTime(), innerTable);
            }
            else
            {
                body = String.Format(CultureInfo.CurrentCulture, ReportStrings.NoEvents, startTime.ToUtcDateTime(), endTime.ToUtcDateTime());
            }

            return emailTemplate.Combine(body);
        }

        public string ExportAuditToHtml(IReadOnlyCollection<AuditItem> auditItems, AuditFilter filter)
        {
            string filterDescription = GetAuditFilterDescription(filter);

            string body;

            if (auditItems.Any())
            {
                string innerTable = CreateReportTable(auditItems);

                body = auditBodyTemplate.Combine(filterDescription, innerTable);
            }
            else
            {
                body = auditBodyTemplate.Combine(filterDescription, ReportStrings.NoEvents);
            }

            return htmlTemplate.Combine(filter.Name, body);
        }

        private static string GetAuditFilterDescription(AuditFilter filter)
        {
            string timeRestriction;

            if (filter.EndTime == Time.Unknown || filter.StartTime == Time.Unknown)
                timeRestriction = AuditStrings.All;
            else
                timeRestriction = string.Format(CultureInfo.CurrentCulture, AuditStrings.FromToTimeTemplate, filter.StartTime.ToUtcDateTime(), filter.EndTime.ToUtcDateTime());


            return auditFilterDescriptionTemplate.Combine(
                filter.Name,
                GetCollectionString(filter.Computers),
                GetCollectionString(filter.Users),
                GetCollectionString(filter.Devices),
                timeRestriction,
                filter.OnlyError ? AuditStrings.Yes : AuditStrings.No,
                GetCollectionString(filter.Types.Cast<object>().ToReadOnlySet()));

        }

        private static string GetCollectionString(IReadOnlyCollection<object> objects)
        {
            return !objects.Any() ? AuditStrings.All : string.Join(", ", objects);
        }

        private static string CreateReportTable(IReadOnlyCollection<AuditItem> auditItems)
        {
            Check.CollectionHasOnlyMeaningfulData(auditItems, "auditItems");

            string headers = CreateHeaders();

            var dataEntry = new StringBuilder();

            foreach (AuditItem item in auditItems)
            {
                AuditItemGroupType group = LinkedEnumHelper<AuditItemType, AuditItemGroupType>.GetLinkedEnum(item.EventType);

                string type = group.GetValueDescription();
                string info = item.ToString();
                string time = item.Time.ToUtcDateTime().ToString(CultureInfo.CurrentCulture);
                string computer = ConvertToString(item.Computer);
                string user = ConvertToString(item.User);
                string device = ConvertToString(item.Device);

                dataEntry.Append(openRow);

                dataEntry.AppendFormat(cellTemplate, type);
                dataEntry.AppendFormat(cellTemplate, info);
                dataEntry.AppendFormat(cellTemplate, time);
                dataEntry.AppendFormat(cellTemplate, computer);
                dataEntry.AppendFormat(cellTemplate, user);
                dataEntry.AppendFormat(cellTemplate, device);

                dataEntry.Append(closeRow);
            }

            return topTemplate.Combine(headers, dataEntry);
        }

        private static string ConvertToString(BaseReadOnlyObject obj)
        {
            return (obj == null) ? "-" : obj.ToString();
        }

        private static string CreateHeaders()
        {
            var result = new StringBuilder();

            result.AppendFormat(cellTemplate, ReportStrings.ItemType);
            result.AppendFormat(cellTemplate, ReportStrings.Description);
            result.AppendFormat(cellTemplate, ReportStrings.Time);
            result.AppendFormat(cellTemplate, ReportStrings.Computer);
            result.AppendFormat(cellTemplate, ReportStrings.User);
            result.AppendFormat(cellTemplate, ReportStrings.Device);

            return result.ToString();
        }
    }
}
