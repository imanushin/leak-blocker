using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LeakBlocker.AdminView.Desktop.Network;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Audit;
using LeakBlocker.Libraries.Common.Tests;
using LeakBlocker.Libraries.Common.Tests.Entities;
using LeakBlocker.Libraries.Common.Tests.Entities.Audit;
using LeakBlocker.Libraries.Storage;
using LeakBlocker.Server.Service.Network.AdminView;
using LeakBlocker.ServerShared.AdminViewCommunication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace LeakBlocker.Server.Service.Tests.Network.AdminView
{
    [TestClass]
    public sealed class AuditToolsServerTest : BaseAdminViewNetworkTest
    {
        private static readonly string directory = Path.Combine(new ConstantsStub().UserDataFolder, "AuditFilter");

        [TestInitialize]
        public void Init()
        {
            if (Directory.Exists(directory))
                Directory.Delete(directory, true);

            Initialize();
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (Directory.Exists(directory))
                Directory.Delete(directory, true);
        }

        [TestMethod]
        public void SaveLoadFilters()
        {
            Mocks.ReplayAll();

            ReadOnlyList<AuditFilter> targetFilters = AuditFilterTest.objects.ToReadOnlyList();

            using (InitServer<AuditToolsServer>())
            {
                using (IAuditTools client = new AuditToolsClient())
                {
                    ReadOnlyList<AuditFilter> filters = targetFilters;

                    client.SaveFilterSet(filters);
                }
            }

            using (InitServer<AuditToolsServer>())
            {
                using (IAuditTools client = new AuditToolsClient())
                {
                    ReadOnlyList<AuditFilter> result = client.LoadFilters();

                    Assert.AreEqual(targetFilters, result);
                }
            }
            Mocks.VerifyAll();
        }

        /// <summary>
        /// Схема такая:
        /// 1. Добавляем один фильтр
        /// 2. Заменяем на второй
        /// 3. Удаляем второй фильтр
        /// 4. Еще раз удаляем второй фильтр и добавляем первый (в итоге ничего не удалится, а первый будет опять добавлен)
        /// Каждый раз проверяем, что всё верно
        /// </summary>
        [TestMethod]
        public void ChangeFilter()
        {
            Mocks.ReplayAll();

            AuditFilter first = AuditFilterTest.objects.First();
            AuditFilter second = AuditFilterTest.objects.Skip(1).First();

            using (InitServer<AuditToolsServer>())
            {
                using (IAuditTools client = new AuditToolsClient())
                {
                    client.CreateFilter(first);

                    ReadOnlyList<AuditFilter> filters = client.LoadFilters();

                    Assert.AreEqual(new[] { first }.ToReadOnlyList(), filters);

                    client.ChangeFilter(first, second);

                    filters = client.LoadFilters();

                    Assert.AreEqual(new[] { second }.ToReadOnlyList(), filters);

                    client.DeleteFilter(second);

                    filters = client.LoadFilters();

                    Assert.AreEqual(new AuditFilter[0].ToReadOnlyList(), filters);

                    client.ChangeFilter(second, first);

                    filters = client.LoadFilters();

                    Assert.AreEqual(new[] { first }.ToReadOnlyList(), filters);
                }
            }

            Mocks.VerifyAll();
        }

        [TestMethod]
        public void GetAuditDevices()
        {
            ReadOnlySet<DeviceDescription> devices = DeviceDescriptionTest.objects.ToReadOnlySet();

            var deviceManager = Mocks.StrictMock<IDevicesManager>();
            StorageObjects.Singletons.DevicesManager.SetTestImplementation(deviceManager);

            deviceManager.Stub(x => x.GetAllDevices()).Return(devices);

            Mocks.ReplayAll();

            using (InitServer<AuditToolsServer>())
            {
                using (IAuditTools client = new AuditToolsClient())
                {
                    ReadOnlySet<DeviceDescription> resultDevices = client.GetAuditDevices();

                    Assert.AreEqual(devices, resultDevices);
                }
            }
        }

        [TestMethod]
        public void GetItemsForFilter()
        {
            AuditFilter filter = AuditFilterTest.objects.First();
            ReadOnlyList<AuditItem> items = AuditItemTest.objects.ToReadOnlyList();

            var auditItemsManager = Mocks.StrictMock<IAuditItemsManager>();
            StorageObjects.Singletons.AuditItemsManager.SetTestImplementation(auditItemsManager);

            auditItemsManager.Stub(x => x.GetItems(filter, 1000)).Return(items);

            Mocks.ReplayAll();

            using (InitServer<AuditToolsServer>())
            {
                using (IAuditTools client = new AuditToolsClient())
                {
                    ReadOnlyList<AuditItem> result = client.GetItemsForFilter(filter, 1000);

                    Assert.AreEqual(items, result);
                }
            }
        }
    }
}
