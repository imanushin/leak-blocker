using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Audit;
using LeakBlocker.Libraries.Common.Network;
using LeakBlocker.Libraries.Storage;
using LeakBlocker.Server.Service.Generated;
using LeakBlocker.Server.Service.InternalTools;

namespace LeakBlocker.Server.Service.Network.AdminView
{
    internal sealed class AuditToolsServer : GeneratedAuditTools
    {
        private const string filterExtension = "lbAuditFilter";

        private readonly string filterDirectory = Path.Combine(SharedObjects.Constants.UserDataFolder, "AuditFilter");

        private readonly object syncRoot = new object();

        public AuditToolsServer()
            : base((ISecuritySessionManager)InternalObjects.AdminViewSecuritySessionManager)
        {
        }

        protected override void SaveFilterSet(ReadOnlyList<AuditFilter> filters)
        {
            lock (syncRoot)
            {
                if (!Directory.Exists(filterDirectory))
                    Directory.CreateDirectory(filterDirectory);

                IEnumerable<string> files = GetSavedFilters();

                files.ForEach(File.Delete);

                int index = 0;

                filters.ForEach(file => SaveFile(file, "{0}.{1}".Combine(index++, filterExtension)));
            }
        }

        protected override ReadOnlyList<AuditFilter> LoadFilters()
        {
            lock (syncRoot)
            {
                if (!Directory.Exists(filterDirectory))
                    Directory.CreateDirectory(filterDirectory);

                List<string> files = GetSavedFilters().ToList();

                files.Sort(
                    (left, right) =>
                    {
                        if (left.Length != right.Length)
                            return left.Length - right.Length;

                        return string.CompareOrdinal(left, right);
                    });

                return files.Select(ReadFile).ToReadOnlyList();
            }
        }

        protected override void CreateFilter(AuditFilter filter)
        {
            Check.ObjectIsNotNull(filter, "filter");

            lock (syncRoot)
            {
                List<AuditFilter> filters = LoadFilters().ToList();

                filters.Add(filter);

                SaveFilterSet(filters.ToReadOnlyList());
            }
        }

        protected override void DeleteFilter(AuditFilter filter)
        {
            Check.ObjectIsNotNull(filter, "filter");

            lock (syncRoot)
            {
                List<AuditFilter> filters = LoadFilters().ToList();

                if (filters.Contains(filter))
                    filters.Remove(filter);

                SaveFilterSet(filters.ToReadOnlyList());
            }
        }

        protected override void ChangeFilter(AuditFilter fromFilter, AuditFilter toFilter)
        {
            Check.ObjectIsNotNull(fromFilter, "fromFilter");
            Check.ObjectIsNotNull(toFilter, "toFilter");

            lock (syncRoot)
            {
                List<AuditFilter> filters = LoadFilters().ToList();

                int index = filters.IndexOf(fromFilter);

                if (index >= 0)
                    filters[index] = toFilter;
                else
                    filters.Add(toFilter);

                SaveFilterSet(filters.ToReadOnlyList());
            }
        }

        protected override ReadOnlySet<DeviceDescription> GetAuditDevices()
        {
            return StorageObjects.DevicesManager.GetAllDevices();
        }

        protected override ReadOnlyList<AuditItem> GetItemsForFilter(AuditFilter filter, int topCount)
        {
            return StorageObjects.AuditItemsManager.GetItems(filter, topCount);
        }

        private IEnumerable<string> GetSavedFilters()
        {
            return Directory.EnumerateFiles(filterDirectory, "*.{0}".Combine(filterExtension));
        }

        private static AuditFilter ReadFile(string filePath)
        {
            using (Stream stream = File.OpenRead(filePath))
            {
                return BaseObjectSerializer.DeserializeFromXml<AuditFilter>(stream);
            }
        }

        private void SaveFile(AuditFilter filter, string fileName)
        {
            using (Stream stream = File.Create(Path.Combine(filterDirectory, fileName)))
            {
                filter.SerializeToXml(stream);
            }
        }
    }
}
