using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Audit;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Tests.Entities;
using LeakBlocker.Libraries.Common.Tests.Entities.Audit;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;
using LeakBlocker.Libraries.Storage.Entities;
using LeakBlocker.Libraries.Storage.InternalTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Libraries.Storage.Tests
{
    [TestClass]
    public sealed class AuditItemsManagerTest : BaseStorageTest
    {
        private static readonly BaseComputerAccount firstComputer = BaseComputerAccountTest.objects.First();
        private static readonly BaseComputerAccount secondComputer = BaseComputerAccountTest.objects.Skip(1).First();

        private static readonly BaseUserAccount firstUser = BaseUserAccountTest.objects.First();
        private static readonly BaseUserAccount secondUser = BaseUserAccountTest.objects.Skip(1).First();

        private static readonly DeviceDescription firstDevice = DeviceDescriptionTest.objects.First();
        private static readonly DeviceDescription secondDevice = DeviceDescriptionTest.objects.Skip(1).First();

        private const AuditItemType firstItemType = AuditItemType.AgentInstallationFailed;
        private const AuditItemType secondItemType = AuditItemType.AgentComputerTurnedOn;

        private static readonly AuditItemGroupType firstItemGroup = LinkedEnumHelper<AuditItemType, AuditItemGroupType>.GetLinkedEnum(firstItemType);

        private static readonly Time firstTime = new Time(new DateTime(2000, 1, 1));
        private static readonly Time secondTime = new Time(new DateTime(2010, 1, 1));

        private static readonly AuditItem firstAuditItem = new AuditItem(firstItemType, firstTime, firstComputer, firstUser, "123", "qwerty", firstDevice, 1);
        private static readonly AuditItem secondAuditItem = new AuditItem(secondItemType, secondTime, secondComputer, secondUser, "321", "qwerty", secondDevice, 12);


        [TestMethod]
        public void AddItems()
        {
            Mocks.ReplayAll();

            var target = new AuditItemsManager();

            ReadOnlySet<AuditItem> baseObjects = AuditItemTest.objects.ToReadOnlySet();

            CheckFirst<AuditItem>(target.AddItem);

            target.AddItems(baseObjects);

            using (IDatabaseModel model = StorageObjects.CreateDatabaseModel())
            {
                ReadOnlySet<AuditItem> selected = model.AuditItemSet.ToList().Select(item => item.GetAuditItem()).ToReadOnlySet();

                Assert.AreEqual(baseObjects, selected);
            }
        }

        [TestMethod]
        public void AddItem()
        {
            Mocks.ReplayAll();

            var target = new AuditItemsManager();

            AuditItem expected = AuditItemTest.objects.First();

            CheckFirst<List<AuditItem>>(target.AddItems);

            target.AddItem(expected);

            using (IDatabaseModel model = StorageObjects.CreateDatabaseModel())
            {
                ReadOnlySet<AuditItem> selected = model.AuditItemSet.ToList().Select(item => item.GetAuditItem()).ToReadOnlySet();

                Assert.AreEqual(1, selected.Count);

                Assert.AreEqual(expected, selected.First());
            }
        }

        [TestMethod]
        [Description("Тестирует базу со всеми возможными фильтрами. Также проверяет, что фильтры корректно работают с полями, в которых стоит null")]
        public void AllFiltersTest()
        {
            Mocks.ReplayAll();

            using (IDatabaseModel model = StorageObjects.CreateDatabaseModel())
            {
                AuditItemTest.objects.ForEach(item => DbAuditItem.ConvertFromAuditItem(item, model));

                model.SaveChanges();
            }

            var target = new AuditItemsManager();

            foreach (AuditFilter filter in AuditFilterTest.objects)
            {
                ReadOnlyList<AuditItem> allResults = target.GetItems(filter, -1);

                ReadOnlyList<AuditItem> singleResult = target.GetItems(filter, 1);

                Assert.IsNotNull(allResults);
                Assert.IsTrue(singleResult.Count <= 1);
            }
        }

        [TestMethod]
        public void ComputerFilterTest()
        {
            InsertAudit();

            var target = new AuditItemsManager();

            ReadOnlyList<AuditItem> result = target.GetItems(
                new AuditFilter(
                    "123",
                    new[] { firstComputer }.ToReadOnlySet(),
                    ReadOnlySet<BaseUserAccount>.Empty,
                    ReadOnlySet<DeviceDescription>.Empty,
                    Time.Unknown,
                    Time.Unknown,
                    false,
                    EnumHelper<AuditItemGroupType>.Values), 1000);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(firstAuditItem, result.First());
        }

        [TestMethod]
        public void DeviceFilterTest()
        {
            InsertAudit();

            var target = new AuditItemsManager();

            ReadOnlyList<AuditItem> result = target.GetItems(
                new AuditFilter(
                    "123",
                    ReadOnlySet<BaseComputerAccount>.Empty,
                    ReadOnlySet<BaseUserAccount>.Empty,
                    new[] { firstDevice }.ToReadOnlySet(),
                    Time.Unknown,
                    Time.Unknown,
                    false,
                    EnumHelper<AuditItemGroupType>.Values), 1000);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(firstAuditItem, result.First());
        }

        [TestMethod]
        public void ErrorFilterTest()
        {
            InsertAudit();

            var target = new AuditItemsManager();

            ReadOnlyList<AuditItem> result = target.GetItems(
                new AuditFilter(
                    "123",
                    ReadOnlySet<BaseComputerAccount>.Empty,
                    ReadOnlySet<BaseUserAccount>.Empty,
                    ReadOnlySet<DeviceDescription>.Empty,
                    Time.Unknown,
                    Time.Unknown,
                    true,
                    EnumHelper<AuditItemGroupType>.Values), 1000);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(AuditItemSeverityType.Error, LinkedEnumHelper<AuditItemType, AuditItemSeverityType>.GetLinkedEnum(result.First().EventType));
        }

        [TestMethod]
        public void GroupFilterTest()
        {
            InsertAudit();

            var target = new AuditItemsManager();

            ReadOnlyList<AuditItem> result = target.GetItems(
                new AuditFilter(
                    "123",
                    ReadOnlySet<BaseComputerAccount>.Empty,
                    ReadOnlySet<BaseUserAccount>.Empty,
                    ReadOnlySet<DeviceDescription>.Empty,
                    Time.Unknown,
                    Time.Unknown,
                    false,
                    new[] { firstItemGroup }.ToReadOnlySet()), 1000);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(firstAuditItem, result.First());
        }

        [TestMethod]
        public void TimeFilterTest()
        {
            InsertAudit();

            var target = new AuditItemsManager();

            ReadOnlyList<AuditItem> result = target.GetItems(
                new AuditFilter(
                    "123",
                    ReadOnlySet<BaseComputerAccount>.Empty,
                    ReadOnlySet<BaseUserAccount>.Empty,
                    ReadOnlySet<DeviceDescription>.Empty,
                    new Time(new DateTime(2005, 1, 1)),
                    new Time(new DateTime(2050, 1, 1)),
                    false,
                    EnumHelper<AuditItemGroupType>.Values), 1000);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(secondAuditItem, result.First());
        }

        [TestMethod]
        public void UserFilterTest()
        {
            InsertAudit();

            var target = new AuditItemsManager();

            ReadOnlyList<AuditItem> result = target.GetItems(
                new AuditFilter(
                    "123",
                    ReadOnlySet<BaseComputerAccount>.Empty,
                    new[] { firstUser }.ToReadOnlySet(),
                    ReadOnlySet<DeviceDescription>.Empty,
                    Time.Unknown,
                    Time.Unknown,
                    false,
                    EnumHelper<AuditItemGroupType>.Values), 1000);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(firstAuditItem, result.First());
        }

        [TestMethod]
        public void ReportFiltersTest()
        {
            InsertAudit();

            var target = new AuditItemsManager();

            foreach (ReportFilter filter in ReportFilterTest.objects)
            {
                ReadOnlyList<AuditItem> result = target.GetItems(filter, firstTime, secondTime);

                Assert.IsNotNull(result);
            }
        }

        private void InsertAudit()
        {
            Mocks.ReplayAll();

            using (IDatabaseModel model = StorageObjects.CreateDatabaseModel())
            {
                DbAuditItem.ConvertFromAuditItem(firstAuditItem, model);
                DbAuditItem.ConvertFromAuditItem(secondAuditItem, model);

                model.SaveChanges();
            }
        }
    }
}
