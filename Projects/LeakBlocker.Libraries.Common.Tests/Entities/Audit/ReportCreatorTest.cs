using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Audit;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Libraries.Common.Tests.Entities.Audit
{
    [TestClass]
    public sealed class ReportCreatorTest : BaseTest
    {
        [TestMethod]
        public void CreateReportHtml()
        {
            Time startTime = TimeTest.objects.First();
            Time endTime = TimeTest.objects.Skip(1).First();

            Mocks.ReplayAll();

            var target = new ReportCreator();

            string report = target.CreateReportHtml(AuditItemTest.objects.Objects, startTime, endTime);

            Assert.IsNotNull(report);
        }

        [TestMethod]
        public void CreateReportHtml_Empty()
        {
            Time startTime = TimeTest.objects.First();
            Time endTime = TimeTest.objects.Skip(1).First();

            Mocks.ReplayAll();

            var target = new ReportCreator();

            string report = target.CreateReportHtml(ReadOnlySet<AuditItem>.Empty, startTime, endTime);

            Assert.IsNotNull(report);
        }


        [TestMethod]
        public void ExportAuditToHtml()
        {
            AuditFilter filter = AuditFilterTest.objects.First();

            Mocks.ReplayAll();

            var target = new ReportCreator();

            string report = target.ExportAuditToHtml(AuditItemTest.objects.Objects, filter);

            Assert.IsNotNull(report);
        }

        [TestMethod]
        public void ExportAuditToHtml_Empty()
        {
            AuditFilter filter = AuditFilterTest.objects.First();

            Mocks.ReplayAll();

            var target = new ReportCreator();

            string report = target.ExportAuditToHtml(ReadOnlySet<AuditItem>.Empty, filter);

            Assert.IsNotNull(report);
        }
    }
}
