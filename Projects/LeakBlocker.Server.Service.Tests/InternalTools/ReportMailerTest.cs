using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Audit;
using LeakBlocker.Libraries.Common.Tests;
using LeakBlocker.Libraries.Common.Tests.Entities.Audit;
using LeakBlocker.Libraries.Storage;
using LeakBlocker.Libraries.SystemTools;
using LeakBlocker.Server.Service.InternalTools;
using LeakBlocker.Server.Service.InternalTools.LocalStorages;
using LeakBlocker.Server.Service.InternalTools.SingletonImplementation;
using LeakBlocker.ServerShared.AdminViewCommunication;
using LeakBlocker.ServerShared.AdminViewCommunication.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using LeakBlocker.Libraries.Common.Collections;

namespace LeakBlocker.Server.Service.Tests.InternalTools
{
    [TestClass]
    public sealed class ReportMailerTest : BaseTest
    {
        private IScheduler scheduler;
        private IAuditItemsManager auditItemManager;
        private IReportCreator reportCreator;
        private IMailer mailer;
        private IPrivateRegistryStorage privateRegistryStorage;
        private IReportConfigurationStorage reportConfigurationStorage;
        private IAuditItemHelper auditItemHelper;
        private IConfigurationStorage configurationStorage;

        [TestInitialize]
        public void Init()
        {
            scheduler = Mocks.StrictMock<IScheduler>();
            auditItemManager = Mocks.StrictMock<IAuditItemsManager>();
            reportCreator = Mocks.StrictMock<IReportCreator>();
            mailer = Mocks.StrictMock<IMailer>();
            privateRegistryStorage = Mocks.StrictMock<IPrivateRegistryStorage>();
            reportConfigurationStorage = Mocks.StrictMock<IReportConfigurationStorage>();
            auditItemHelper = Mocks.StrictMock<IAuditItemHelper>();
            configurationStorage = Mocks.StrictMock<IConfigurationStorage>();

            InternalObjects.Singletons.Mailer.SetTestImplementation(mailer);
            SharedObjects.Singletons.ReportCreator.SetTestImplementation(reportCreator);
            StorageObjects.Singletons.AuditItemsManager.SetTestImplementation(auditItemManager);
            SystemObjects.Factories.PrivateRegistryStorage.EnqueueTestImplementation(privateRegistryStorage);
            InternalObjects.Singletons.ReportConfigurationStorage.SetTestImplementation(reportConfigurationStorage);
            InternalObjects.Singletons.AuditItemHelper.SetTestImplementation(auditItemHelper);
            InternalObjects.Singletons.ConfigurationStorage.SetTestImplementation(configurationStorage);

            configurationStorage.Stub(x => x.Current).Return(SimpleConfigurationTest.First);
        }

        [TestMethod]
        public void StartObserving()
        {
            SharedObjects.Factories.Scheduler.EnqueueTestImplementation(scheduler);

            scheduler.Stub(x => x.RunNow());

            Mocks.ReplayAll();

            var target = new ReportMailer();

            target.StartObserving();
        }

        [TestMethod]
        public void SendTestReport_UnableToSendBecauseReportsNotConfigured()
        {
            Action scheduleAction = null;

            SharedObjects.Factories.Scheduler.EnqueueConstructor((func, time, b) =>
                {
                    scheduleAction = func;
                    return scheduler;
                });

            scheduler.Stub(x => x.RunNow());
            privateRegistryStorage.Stub(x => x.GetValue(null)).IgnoreArguments().Return(DateTime.Now.AddDays(-3).ToString(CultureInfo.InvariantCulture));
            reportConfigurationStorage.Stub(x => x.Current).Return(null);
            auditItemHelper.Expect(x => x.ReportsDoesNotConfigured());

            privateRegistryStorage.Expect(x => x.SetValue(null, null)).IgnoreArguments();

            Mocks.ReplayAll();

            new ReportMailer();//Здесь мы заполнили функцию scheduleAction

            scheduleAction();

            Mocks.VerifyAll();
        }

        [TestMethod]
        public void SendTestReport()
        {
            SharedObjects.Factories.Scheduler.EnqueueTestImplementation(scheduler);

            scheduler.Stub(x => x.RunNow());
            auditItemManager.Stub(x => x.GetItems(null, null, null)).IgnoreArguments().Return(AuditItemTest.objects.ToReadOnlyList());
            reportCreator.Stub(x => x.CreateReportHtml(null, null, null)).IgnoreArguments().Return("<html><html>");
            mailer.Stub(x => x.SendMessage(null, null)).IgnoreArguments();

            Mocks.ReplayAll();

            var target = new ReportMailer();

            target.SendTestReport(ReportConfigurationTest.First);
        }
    }
}
