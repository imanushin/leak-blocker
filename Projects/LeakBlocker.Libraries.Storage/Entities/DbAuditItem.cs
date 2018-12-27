using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Audit;

namespace LeakBlocker.Libraries.Storage.Entities
{
    partial class DbAuditItem
    {
        private AuditItem ForceGetAuditItem()
        {
            return new AuditItem(
                EventType,
                Time,
                Computer.GetBaseComputerAccount(),
                User == null ? null : User.GetBaseUserAccount(),
                TextData,
                AdditionalTextData,
                Device == null ? null : Device.GetDeviceDescription(),
                Configuration,
                Number);
        }

        internal static IQueryable<DbAuditItem> FilterByTime(IQueryable<DbAuditItem> baseQuery, Time startTime, Time endTime)
        {
            long minTicks = startTime.Ticks;
            long maxTicks = endTime.Ticks;

            return baseQuery.Where(item => item.TimeTicks > minTicks && item.TimeTicks < maxTicks);
        }
    }
}
