using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Audit;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Storage.Entities;
using LeakBlocker.Libraries.Storage.InternalTools;

namespace LeakBlocker.Libraries.Storage
{
    internal sealed class AuditItemsManager : IAuditItemsManager
    {
        private const int maxExpressionCount = 128;

        private readonly DisplacingCache<ReadOnlySet<BaseUserAccount>, Expression<Func<DbAuditItem, bool>>> usersFiltersCache = new DisplacingCache<ReadOnlySet<BaseUserAccount>, Expression<Func<DbAuditItem, bool>>>(maxExpressionCount);
        private readonly DisplacingCache<ReadOnlySet<DeviceDescription>, Expression<Func<DbAuditItem, bool>>> devicesFiltersCache = new DisplacingCache<ReadOnlySet<DeviceDescription>, Expression<Func<DbAuditItem, bool>>>(maxExpressionCount);
        private readonly DisplacingCache<ReadOnlySet<BaseComputerAccount>, Expression<Func<DbAuditItem, bool>>> computersFiltersCache = new DisplacingCache<ReadOnlySet<BaseComputerAccount>, Expression<Func<DbAuditItem, bool>>>(maxExpressionCount);
        private readonly DisplacingCache<ReadOnlySet<AuditItemGroupType>, Expression<Func<DbAuditItem, bool>>> eventTypesFiltersCache = new DisplacingCache<ReadOnlySet<AuditItemGroupType>, Expression<Func<DbAuditItem, bool>>>(maxExpressionCount);
        private readonly DisplacingCache<ReadOnlySet<ReportType>, Expression<Func<DbAuditItem, bool>>> reportTypesFiltersCache = new DisplacingCache<ReadOnlySet<ReportType>, Expression<Func<DbAuditItem, bool>>>(maxExpressionCount);

        private readonly Expression<Func<DbAuditItem, long>> queryOrder = CreateQueryOrder();

        private readonly Expression<Func<DbAuditItem, bool>> errorFilter = CreateErrorFilter();


        public void AddItems(IEnumerable<AuditItem> items)
        {
            Check.CollectionHasNoDefaultItems(items, "items");

            using (IDatabaseModel model = StorageObjects.CreateDatabaseModel())
            {
                List<DbAuditItem> newItems = items.Select(item => DbAuditItem.ConvertFromAuditItem(item, model)).ToList();

                newItems.ForEach(item => model.AuditItemSet.Add(item));

                model.SaveChanges();
            }
        }

        public void AddItem(AuditItem item)
        {
            Check.ObjectIsNotNull(item, "item");

            AddItems(new[] { item });
        }

        public ReadOnlyList<AuditItem> GetItems(AuditFilter filter, int topCount)
        {
            using (IDatabaseModel model = StorageObjects.CreateDatabaseModel())
            {
                IQueryable<DbAuditItem> query = model.AuditItemSet;

                if (filter.StartTime != Time.Unknown && filter.EndTime != Time.Unknown)
                    query = DbAuditItem.FilterByTime(query, filter.StartTime, filter.EndTime);

                query = ApplyCommonFilter(filter.Users, query, usersFiltersCache, UserExpressionCreator, model);
                query = ApplyCommonFilter(filter.Devices, query, devicesFiltersCache, DeviceExpressionCreator, model);
                query = ApplyCommonFilter(filter.Computers, query, computersFiltersCache, ComputerExpressionCreator, model);

                query = ApplyCommonFilter(filter.Types, query, eventTypesFiltersCache, TypeExpressionCreator, model);

                if (filter.OnlyError)
                    query = query.Where(errorFilter);

                query = query.OrderBy(queryOrder);

                if (topCount != -1)
                    query = query.Take(topCount);

                IEnumerable<AuditItem> result = query.ToList().Select(item => item.GetAuditItem());

                return result.ToReadOnlyList();
            }
        }

        public ReadOnlyList<AuditItem> GetItems(ReportFilter filter, Time startTime, Time endTime)
        {
            if (startTime > endTime)
                throw new ArgumentException("Start time should be less then end time. Start time: {0}, end time: {1}".Combine(startTime, endTime));


            ReadOnlySet<ReportType> types = filter.ReportTypes;

            if (!types.Any())
                return ReadOnlyList<AuditItem>.Empty;

            using (IDatabaseModel model = StorageObjects.CreateDatabaseModel())
            {
                IQueryable<DbAuditItem> query = model.AuditItemSet;

                query = DbAuditItem.FilterByTime(query, startTime, endTime);

                query = ApplyCommonFilter(types, query, reportTypesFiltersCache, ReportTypeExpressionCreator, model);

                query = query.OrderBy(queryOrder);

                IEnumerable<AuditItem> result = query.ToList().Select(item => item.GetAuditItem());

                return result.ToReadOnlyList();
            }
        }

        private static Expression<Func<DbAuditItem, bool>> ReportTypeExpressionCreator(ReadOnlySet<ReportType> types, IDatabaseModel model)
        {
            int[] filterTypes =
                types
                    .SelectMany(LinkedEnumHelper<AuditItemType, ReportType>.GetAllLinkedEnums)
                    .Cast<int>()
                    .ToArray();

            return item => filterTypes.Contains(item.EventTypeIntValue);
        }

        private static Expression<Func<DbAuditItem, bool>> TypeExpressionCreator(ReadOnlySet<AuditItemGroupType> types, IDatabaseModel model)
        {
            int[] filterTypes =
                types
                    .SelectMany(LinkedEnumHelper<AuditItemType, AuditItemGroupType>.GetAllLinkedEnums)
                    .Cast<int>()
                    .ToArray();

            return item => filterTypes.Contains(item.EventTypeIntValue);
        }

        private static Expression<Func<DbAuditItem, bool>> UserExpressionCreator(ReadOnlySet<BaseUserAccount> users, IDatabaseModel model)
        {
            int[] ids = users.Select(item => DbBaseUserAccount.ConvertFromBaseUserAccount(item, model).Id).ToArray();

            return item => item.User != null && ids.Contains(item.User.Id);
        }

        private static Expression<Func<DbAuditItem, bool>> ComputerExpressionCreator(ReadOnlySet<BaseComputerAccount> computers, IDatabaseModel model)
        {
            int[] ids = computers.Select(item => DbBaseComputerAccount.ConvertFromBaseComputerAccount(item, model).Id).ToArray();

            return item => ids.Contains(item.Computer.Id);
        }

        private static Expression<Func<DbAuditItem, bool>> DeviceExpressionCreator(ReadOnlySet<DeviceDescription> devices, IDatabaseModel model)
        {
            int[] ids = devices.Select(item => DbDeviceDescription.ConvertFromDeviceDescription(item, model).Id).ToArray();

            return item => item.Device != null && ids.Contains(item.Device.Id);
        }


        private static IQueryable<DbAuditItem> ApplyCommonFilter<TEntity>(
            ReadOnlySet<TEntity> whereInItems,
            IQueryable<DbAuditItem> items,
            DisplacingCache<ReadOnlySet<TEntity>, Expression<Func<DbAuditItem, bool>>> cache,
            Func<ReadOnlySet<TEntity>, IDatabaseModel, Expression<Func<DbAuditItem, bool>>> expressionCreator,
            IDatabaseModel model)
        {
            if (!whereInItems.Any())
                return items;

            Expression<Func<DbAuditItem, bool>> expression = cache.Get(whereInItems);

            if (expression != null)
                return items.Where(expression);

            expression = expressionCreator(whereInItems, model);

            cache.Push(whereInItems, expression);

            return items.Where(expression);
        }

        private static Expression<Func<DbAuditItem, bool>> CreateErrorFilter()
        {
            int[] errorTypes =
                 LinkedEnumHelper<AuditItemType, AuditItemSeverityType>
                 .GetAllLinkedEnums(AuditItemSeverityType.Error)
                 .Cast<int>()
                 .ToArray();

            return item => errorTypes.Contains(item.EventTypeIntValue);
        }

        private static Expression<Func<DbAuditItem, long>> CreateQueryOrder()
        {
            return item => item.TimeTicks;
        }

    }
}